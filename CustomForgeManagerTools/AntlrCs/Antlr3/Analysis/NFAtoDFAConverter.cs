﻿/*
 * [The "BSD license"]
 * Copyright (c) 2011 Terence Parr
 * All rights reserved.
 *
 * Conversion to C#:
 * Copyright (c) 2011 Sam Harwell, Pixel Mine, Inc.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

namespace Antlr3.Analysis
{
    using System.Collections.Generic;
    using System.Linq;
    using Antlr3.Misc;

    using BitSet = Antlr3.Misc.BitSet;
    using Console = System.Console;
    using ErrorManager = Antlr3.Tool.ErrorManager;
    using IToken = Antlr.Runtime.IToken;

    /** Code that embodies the NFA conversion to DFA. A new object is needed
     *  per DFA (also required for thread safety if multiple conversions
     *  launched).
     */
    public class NFAToDFAConverter
    {
        public static bool debug = false;

        /** Should ANTLR launch multiple threads to convert NFAs to DFAs?
         *  With a 2-CPU box, I note that it's about the same single or
         *  multithreaded.  Both CPU meters are going even when single-threaded
         *  so I assume the GC is killing us.  Could be the compiler.  When I
         *  run java -Xint mode, I get about 15% speed improvement with multiple
         *  threads.
         */
        public static bool SINGLE_THREADED_NFA_CONVERSION = true;

        /** A list of DFA states we still need to process during NFA conversion */
        private readonly Queue<DFAState> _work = new Queue<DFAState>();

        /** While converting NFA, we must track states that
         *  reference other rule's NFAs so we know what to do
         *  at the end of a rule.  We need to know what context invoked
         *  this rule so we can know where to continue looking for NFA
         *  states.  I'm tracking a context tree (record of rule invocation
         *  stack trace) for each alternative that could be predicted.
         */
        private NFAContext[] _contextTrees;

        /** We are converting which DFA? */
        private readonly DFA _dfa;

        private bool _computingStartState = false;

        public NFAToDFAConverter( DFA dfa )
        {
            this._dfa = dfa;
            int nAlts = dfa.NumberOfAlts;
            InitContextTrees( nAlts );
        }

        public virtual void Convert()
        {
            //_dfa.conversionStartTime = System.DateTime.Now;

            // create the DFA start state
            _dfa.StartState = ComputeStartState();

            // while more DFA states to check, process them
            while ( _work.Count > 0 &&
                    !_dfa.Nfa.Grammar.NFAToDFAConversionExternallyAborted() )
            {
                DFAState d = (DFAState)_work.Peek();
                if ( _dfa.Nfa.Grammar.composite.WatchNFAConversion )
                {
                    Console.Out.WriteLine( "convert DFA state " + d.StateNumber +
                                       " (" + d.NfaConfigurations.Count + " nfa states)" );
                }
                int k = _dfa.UserMaxLookahead;
                if ( k > 0 && k == d.LookaheadDepth )
                {
                    // we've hit max lookahead, make this a stop state
                    //JSystem.@out.println("stop state @k="+k+" (terminated early)");
                    /*
                    IList<Label> sampleInputLabels = d.dfa.probe.getSampleNonDeterministicInputSequence(d);
                    String input = d.dfa.probe.getInputSequenceDisplay(sampleInputLabels);
                    JSystem.@out.println("sample input: "+input);
                     */
                    ResolveNonDeterminisms( d );
                    // Check to see if we need to add any semantic predicate transitions
                    if ( d.IsResolvedWithPredicates )
                    {
                        AddPredicateTransitions( d );
                    }
                    else
                    {
                        d.IsAcceptState = true; // must convert to accept state at k
                    }
                }
                else
                {
                    FindNewDFAStatesAndAddDFATransitions( d );
                }
                _work.Dequeue(); // done with it; remove from work list
            }

            // Find all manual syn preds (gated).  These are not discovered
            // in tryToResolveWithSemanticPredicates because they are implicitly
            // added to every edge by code gen, DOT generation etc...
            _dfa.FindAllGatedSynPredsUsedInDFAAcceptStates();
        }

        /** From this first NFA state of a decision, create a DFA.
         *  Walk each alt in decision and compute closure from the start of that
         *  rule, making sure that the closure does not include other alts within
         *  that same decision.  The idea is to associate a specific alt number
         *  with the starting closure so we can trace the alt number for all states
         *  derived from this.  At a stop state in the DFA, we can return this alt
         *  number, indicating which alt is predicted.
         *
         *  If this DFA is derived from an loop back NFA state, then the first
         *  transition is actually the exit branch of the loop.  Rather than make
         *  this alternative one, let's make this alt n+1 where n is the number of
         *  alts in this block.  This is nice to keep the alts of the block 1..n;
         *  helps with error messages.
         *
         *  I handle nongreedy in findNewDFAStatesAndAddDFATransitions
         *  when nongreedy and EOT transition.  Make state with EOT emanating
         *  from it the accept state.
         */
        protected virtual DFAState ComputeStartState()
        {
            NFAState alt = _dfa.NFADecisionStartState;
            DFAState startState = _dfa.NewState();
            _computingStartState = true;
            int i = 0;
            int altNum = 1;
            while ( alt != null )
            {
                // find the set of NFA states reachable without consuming
                // any input symbols for each alt.  Keep adding to same
                // overall closure that will represent the DFA start state,
                // but track the alt number
                NFAContext initialContext = _contextTrees[i];
                // if first alt is derived from loopback/exit branch of loop,
                // make alt=n+1 for n alts instead of 1
                if ( i == 0 &&
                     _dfa.NFADecisionStartState.decisionStateType == NFAState.LOOPBACK )
                {
                    int numAltsIncludingExitBranch = _dfa.Nfa.Grammar
                        .GetNumberOfAltsForDecisionNFA( _dfa.NFADecisionStartState );
                    altNum = numAltsIncludingExitBranch;
                    Closure( (NFAState)alt.transition[0].Target,
                            altNum,
                            initialContext,
                            SemanticContext.EmptySemanticContext,
                            startState,
                            true
                    );
                    altNum = 1; // make next alt the first
                }
                else
                {
                    Closure( (NFAState)alt.transition[0].Target,
                            altNum,
                            initialContext,
                            SemanticContext.EmptySemanticContext,
                            startState,
                            true
                    );
                    altNum++;
                }
                i++;

                // move to next alternative
                if ( alt.transition[1] == null )
                {
                    break;
                }
                alt = (NFAState)alt.transition[1].Target;
            }

            // now DFA start state has the complete closure for the decision
            // but we have tracked which alt is associated with which
            // NFA states.
            _dfa.AddState( startState ); // make sure dfa knows about this state
            _work.Enqueue( startState );
            _computingStartState = false;
            return startState;
        }

        /** From this node, add a d--a-->t transition for all
         *  labels 'a' where t is a DFA node created
         *  from the set of NFA states reachable from any NFA
         *  state in DFA state d.
         */
        protected virtual void FindNewDFAStatesAndAddDFATransitions( DFAState d )
        {
            //JSystem.@out.println("work on DFA state "+d);
            var labels = d.ReachableLabels;
            //JSystem.@out.println("reachable labels="+labels);

            /*
            JSystem.@out.println("|reachable|/|nfaconfigs|="+
                    labels.size()+"/"+d.getNFAConfigurations().size()+"="+
                    labels.size()/(float)d.getNFAConfigurations().size());
            */

            // normally EOT is the "default" clause and decisions just
            // choose that last clause when nothing else matches.  DFA conversion
            // continues searching for a unique sequence that predicts the
            // various alts or until it finds EOT.  So this rule
            //
            // DUH : ('x'|'y')* "xy!";
            //
            // does not need a greedy indicator.  The following rule works fine too
            //
            // A : ('x')+ ;
            //
            // When the follow branch could match what is in the loop, by default,
            // the nondeterminism is resolved in favor of the loop.  You don't
            // get a warning because the only way to get this condition is if
            // the DFA conversion hits the end of the token.  In that case,
            // we're not *sure* what will happen next, but it could be anything.
            // Anyway, EOT is the default case which means it will never be matched
            // as resolution goes to the lowest alt number.  Exit branches are
            // always alt n+1 for n alts in a block.
            //
            // When a loop is nongreedy and we find an EOT transition, the DFA
            // state should become an accept state, predicting exit of loop.  It's
            // just reversing the resolution of ambiguity.
            // TODO: should this be done in the resolveAmbig method?
            Label EOTLabel = new Label( Label.EOT );
            bool containsEOT = labels != null && labels.Contains( EOTLabel );
            if ( !_dfa.IsGreedy && containsEOT )
            {
                ConvertToEOTAcceptState( d );
                return; // no more work to do on this accept state
            }

            // if in filter mode for lexer, want to match shortest not longest
            // string so if we see an EOT edge emanating from this state, then
            // convert this state to an accept state.  This only counts for
            // The Tokens rule as all other decisions must continue to look for
            // longest match.
            // [Taking back out a few days later on Jan 17, 2006.  This could
            //  be an option for the future, but this was wrong soluion for
            //  filtering.]
#if false
            if ( dfa.nfa.grammar.type==Antlr3.Tool.GrammarType.Lexer && containsEOT ) {
                string filterOption = (string)dfa.nfa.grammar.getOption("filter");
                bool filterMode = filterOption!=null && filterOption.Equals("true");
                if ( filterMode && d.dfa.isTokensRuleDecision() ) {
                    DFAState t = reach(d, EOTLabel);
                    if ( t.nfaConfigurations.size()>0 ) {
                        convertToEOTAcceptState(d);
                        //JSystem.@out.println("state "+d+" has EOT target "+t.stateNumber);
                        return;
                    }
                }
            }
#endif

            int numberOfEdgesEmanating = 0;
            var targetToLabelMap = new Dictionary<int, Transition>();
            // for each label that could possibly emanate from NFAStates of d
            int numLabels = 0;
            if ( labels != null )
            {
                numLabels = labels.Count;
            }
            for ( int i = 0; i < numLabels; i++ )
            {
                Label label = (Label)labels.ElementAt( i );
                DFAState t = Reach( d, label );
                if ( debug )
                {
                    Console.Out.WriteLine( "DFA state after reach " + label + " " + d + "-" +
                                       label.ToString( _dfa.Nfa.Grammar ) + "->" + t );
                }
                if ( t == null )
                {
                    // nothing was reached by label due to conflict resolution
                    // EOT also seems to be in here occasionally probably due
                    // to an end-of-rule state seeing it even though we'll pop
                    // an invoking state off the state; don't bother to conflict
                    // as this labels set is a covering approximation only.
                    continue;
                }
                //JSystem.@out.println("dfa.k="+dfa.getUserMaxLookahead());
                if ( t.GetUniqueAlt() == NFA.INVALID_ALT_NUMBER )
                {
                    // Only compute closure if a unique alt number is not known.
                    // If a unique alternative is mentioned among all NFA
                    // configurations then there is no possibility of needing to look
                    // beyond this state; also no possibility of a nondeterminism.
                    // This optimization May 22, 2006 just dropped -Xint time
                    // for analysis of Java grammar from 11.5s to 2s!  Wow.
                    Closure( t );  // add any NFA states reachable via epsilon
                }

                /*
                JSystem.@out.println("DFA state after closure "+d+"-"+
                                   label.toString(dfa.nfa.grammar)+
                                   "->"+t);
                                   */

                // add if not in DFA yet and then make d-label->t
                DFAState targetState = AddDFAStateToWorkList( t );

                numberOfEdgesEmanating +=
                    AddTransition( d, label, targetState, targetToLabelMap );

                // lookahead of target must be one larger than d's k
                // We are possibly setting the depth of a pre-existing state
                // that is equal to one we just computed...not sure if that's
                // ok.
                targetState.LookaheadDepth = d.LookaheadDepth + 1;
            }

            //JSystem.@out.println("DFA after reach / closures:\n"+dfa);
            if ( !d.IsResolvedWithPredicates && numberOfEdgesEmanating == 0 )
            {
                //JSystem.@out.println("dangling DFA state "+d+"\nAfter reach / closures:\n"+dfa);
                // TODO: can fixed lookahead hit a dangling state case?
                // TODO: yes, with left recursion
                //Console.Error.WriteLine( "dangling state alts: " + d.getAltSet() );
                _dfa.Probe.ReportDanglingState( d );
                // turn off all configurations except for those associated with
                // min alt number; somebody has to win else some input will not
                // predict any alt.
                int minAlt = ResolveByPickingMinAlt( d, null );
                // force it to be an accept state
                // don't call convertToAcceptState() which merges stop states.
                // other states point at us; don't want them pointing to dead states
                d.IsAcceptState = true; // might be adding new accept state for alt
                _dfa.SetAcceptState( minAlt, d );
                //convertToAcceptState(d, minAlt); // force it to be an accept state
            }

            // Check to see if we need to add any semantic predicate transitions
            // might have both token and predicated edges from d
            if ( d.IsResolvedWithPredicates )
            {
                AddPredicateTransitions( d );
            }
        }

        /** Add a transition from state d to targetState with label in normal case.
         *  if COLLAPSE_ALL_INCIDENT_EDGES, however, try to merge all edges from
         *  d to targetState; this means merging their labels.  Another optimization
         *  is to reduce to a single EOT edge any set of edges from d to targetState
         *  where there exists an EOT state.  EOT is like the wildcard so don't
         *  bother to test any other edges.  Example:
         *
         *  NUM_INT
         *    : '1'..'9' ('0'..'9')* ('l'|'L')?
         *    | '0' ('x'|'X') ('0'..'9'|'a'..'f'|'A'..'F')+ ('l'|'L')?
         *    | '0' ('0'..'7')* ('l'|'L')?
         *    ;
         *
         *  The normal decision to predict alts 1, 2, 3 is:
         *
         *  if ( (input.LA(1)>='1' && input.LA(1)&lt;='9') ) {
         *       alt7=1;
         *  }
         *  else if ( input.LA(1)=='0' ) {
         *      if ( input.LA(2)=='X'||input.LA(2)=='x' ) {
         *          alt7=2;
         *      }
         *      else if ( (input.LA(2)>='0' && input.LA(2)&lt;='7') ) {
         *           alt7=3;
         *      }
         *      else if ( input.LA(2)=='L'||input.LA(2)=='l' ) {
         *           alt7=3;
         *      }
         *      else {
         *           alt7=3;
         *      }
         *  }
         *  else error
         *
         *  Clearly, alt 3 is predicted with extra work since it tests 0..7
         *  and [lL] before finally realizing that any character is actually
         *  ok at k=2.
         *
         *  A better decision is as follows:
         *
         *  if ( (input.LA(1)>='1' && input.LA(1)&lt;='9') ) {
         *      alt7=1;
         *  }
         *  else if ( input.LA(1)=='0' ) {
         *      if ( input.LA(2)=='X'||input.LA(2)=='x' ) {
         *          alt7=2;
         *      }
         *      else {
         *          alt7=3;
         *      }
         *  }
         *
         *  The DFA originally has 3 edges going to the state the predicts alt 3,
         *  but upon seeing the EOT edge (the "else"-clause), this method
         *  replaces the old merged label (which would have (0..7|l|L)) with EOT.
         *  The code generator then leaves alt 3 predicted with a simple else-
         *  clause. :)
         *
         *  The only time the EOT optimization makes no sense is in the Tokens
         *  rule.  We want EOT to truly mean you have matched an entire token
         *  so don't bother actually rewinding to execute that rule unless there
         *  are actions in that rule.  For now, since I am not preventing
         *  backtracking from Tokens rule, I will simply allow the optimization.
         */
        protected static int AddTransition( DFAState d,
                                           Label label,
                                           DFAState targetState,
                                           IDictionary<int, Transition> targetToLabelMap )
        {
            //JSystem.@out.println(d.stateNumber+"-"+label.toString(dfa.nfa.grammar)+"->"+targetState.stateNumber);
            int n = 0;
            if ( DFAOptimizer.COLLAPSE_ALL_PARALLEL_EDGES )
            {
                // track which targets we've hit
                int tI = targetState.StateNumber;
                Transition oldTransition;
                targetToLabelMap.TryGetValue(tI, out oldTransition);
                if ( oldTransition != null )
                {
                    //JSystem.@out.println("extra transition to "+tI+" upon "+label.toString(dfa.nfa.grammar));
                    // already seen state d to target transition, just add label
                    // to old label unless EOT
                    if ( label.Atom == Label.EOT )
                    {
                        // merge with EOT means old edge can go away
                        oldTransition.Label = new Label( Label.EOT );
                    }
                    else
                    {
                        // don't add anything to EOT, it's essentially the wildcard
                        if ( oldTransition.Label.Atom != Label.EOT )
                        {
                            // ok, not EOT, add in this label to old label
                            oldTransition.Label.Add( label );
                        }
                        //JSystem.@out.println("label updated to be "+oldTransition.label.toString(dfa.nfa.grammar));
                    }
                }
                else
                {
                    // make a transition from d to t upon 'a'
                    n = 1;
                    label = (Label)label.Clone(); // clone in case we alter later
                    int transitionIndex = d.AddTransition( targetState, label );
                    Transition trans = d.GetTransition( transitionIndex );
                    // track target/transition pairs
                    targetToLabelMap[tI] = trans;
                }
            }
            else
            {
                n = 1;
                d.AddTransition( targetState, label );
            }
            return n;
        }

        /** For all NFA states (configurations) merged in d,
         *  compute the epsilon closure; that is, find all NFA states reachable
         *  from the NFA states in d via purely epsilon transitions.
         */
        public virtual void Closure( DFAState d )
        {
            if ( debug )
            {
                Console.Out.WriteLine( "closure(" + d + ")" );
            }

            List<NFAConfiguration> configs = new List<NFAConfiguration>();
            // Because we are adding to the configurations in closure
            // must clone initial list so we know when to stop doing closure
            configs.AddRange( d.NfaConfigurations );
            // for each NFA configuration in d (abort if we detect non-LL(*) state)
            int numConfigs = configs.Count;
            for ( int i = 0; i < numConfigs; i++ )
            {
                NFAConfiguration c = (NFAConfiguration)configs[i];
                if ( c.SingleAtomTransitionEmanating )
                {
                    continue; // ignore NFA states w/o epsilon transitions
                }
                //JSystem.@out.println("go do reach for NFA state "+c.state);
                // figure out reachable NFA states from each of d's nfa states
                // via epsilon transitions.
                // Fill configsInClosure rather than altering d configs inline
                Closure( _dfa.Nfa.GetState( c.State ),
                        c.Alt,
                        c.Context,
                        c.SemanticContext,
                        d,
                        false );
            }
            //JSystem.@out.println("after closure d="+d);
            d.ClosureBusy = null; // wack all that memory used during closure
        }

        /** Where can we get from NFA state p traversing only epsilon transitions?
         *  Add new NFA states + context to DFA state d.  Also add semantic
         *  predicates to semantic context if collectPredicates is set.  We only
         *  collect predicates at hoisting depth 0, meaning before any token/char
         *  have been recognized.  This corresponds, during analysis, to the
         *  initial DFA start state construction closure() invocation.
         *
         *  There are four cases of interest (the last being the usual transition):
         *
         *   1. Traverse an edge that takes us to the start state of another
         *      rule, r.  We must push this state so that if the DFA
         *      conversion hits the end of rule r, then it knows to continue
         *      the conversion at state following state that "invoked" r. By
         *      construction, there is a single transition emanating from a rule
         *      ref node.
         *
         *   2. Reach an NFA state associated with the end of a rule, r, in the
         *      grammar from which it was built.  We must add an implicit (i.e.,
         *      don't actually add an epsilon transition) epsilon transition
         *      from r's end state to the NFA state following the NFA state
         *      that transitioned to rule r's start state.  Because there are
         *      many states that could reach r, the context for a rule invocation
         *      is part of a call tree not a simple stack.  When we fall off end
         *      of rule, "pop" a state off the call tree and add that state's
         *      "following" node to d's NFA configuration list.  The context
         *      for this new addition will be the new "stack top" in the call tree.
         *
         *   3. Like case 2, we reach an NFA state associated with the end of a
         *      rule, r, in the grammar from which NFA was built.  In this case,
         *      however, we realize that during this NFA->DFA conversion, no state
         *      invoked the current rule's NFA.  There is no choice but to add
         *      all NFA states that follow references to r's start state.  This is
         *      analogous to computing the FOLLOW(r) in the LL(k) world.  By
         *      construction, even rule stop state has a chain of nodes emanating
         *      from it that points to every possible following node.  This case
         *      is conveniently handled then by the 4th case.
         *
         *   4. Normal case.  If p can reach another NFA state q, then add
         *      q to d's configuration list, copying p's context for q's context.
         *      If there is a semantic predicate on the transition, then AND it
         *      with any existing semantic context.
         *
         *   Current state p is always added to d's configuration list as it's part
         *   of the closure as well.
         *
         *  When is a closure operation in a cycle condition?  While it is
         *  very possible to have the same NFA state mentioned twice
         *  within the same DFA state, there are two situations that
         *  would lead to nontermination of closure operation:
         *
         *  o   Whenever closure reaches a configuration where the same state
         *      with same or a suffix context already exists.  This catches
         *      the IF-THEN-ELSE tail recursion cycle and things like
         *
         *      a : A a | B ;
         *
         *      the context will be $ (empty stack).
         *
         *      We have to check
         *      larger context stacks because of (...)+ loops.  For
         *      example, the context of a (...)+ can be nonempty if the
         *      surrounding rule is invoked by another rule:
         *
         *      a : b A | X ;
         *      b : (B|)+ ;  // nondeterministic by the way
         *
         *      The context of the (B|)+ loop is "invoked from item
         *      a : . b A ;" and then the empty alt of the loop can reach back
         *      to itself.  The context stack will have one "return
         *      address" element and so we must check for same state, same
         *      context for arbitrary context stacks.
         *
         *      Idea: If we've seen this configuration before during closure, stop.
         *      We also need to avoid reaching same state with conflicting context.
         *      Ultimately analysis would stop and we'd find the conflict, but we
         *      should stop the computation.  Previously I only checked for
         *      exact config.  Need to check for same state, suffix context
         * 		not just exact context.
         *
         *  o   Whenever closure reaches a configuration where state p
         *      is present in its own context stack.  This means that
         *      p is a rule invocation state and the target rule has
         *      been called before.  NFAContext.MAX_RECURSIVE_INVOCATIONS
         *      (See the comment there also) determines how many times
         *      it's possible to recurse; clearly we cannot recurse forever.
         *      Some grammars such as the following actually require at
         *      least one recursive call to correctly compute the lookahead:
         *
         *      a : L ID R
         *        | b
         *        ;
         *      b : ID
         *        | L a R
         *        ;
         *
         *      Input L ID R is ambiguous but to figure this out, ANTLR
         *      needs to go a->b->a->b to find the L ID sequence.
         *
         *      Do not allow closure to add a configuration that would
         *      allow too much recursion.
         *
         *      This case also catches infinite left recursion.
         */
        public virtual void Closure( NFAState p,
                            int alt,
                            NFAContext context,
                            SemanticContext semanticContext,
                            DFAState d,
                            bool collectPredicates )
        {
            if ( debug )
            {
                Console.Out.WriteLine( "closure at " + p.enclosingRule.Name + " state " + p.StateNumber + "|" +
                                   alt + " filling DFA state " + d.StateNumber + " with context " + context
                                   );
            }

            //if ( DFA.MAX_TIME_PER_DFA_CREATION > System.TimeSpan.Zero &&
            //     System.DateTime.Now - d.dfa.conversionStartTime >=
            //     DFA.MAX_TIME_PER_DFA_CREATION )
            //{
            //    // bail way out; we've blown up somehow
            //    throw new AnalysisTimeoutException( d.dfa );
            //}

            NFAConfiguration proposedNFAConfiguration =
                    new NFAConfiguration( p.StateNumber,
                            alt,
                            context,
                            semanticContext );

            // Avoid infinite recursion
            if ( ClosureIsBusy( d, proposedNFAConfiguration ) )
            {
                if ( debug )
                {
                    Console.Out.WriteLine( "avoid visiting exact closure computation NFA config: " +
                                       proposedNFAConfiguration + " in " + p.enclosingRule.Name );
                    Console.Out.WriteLine( "state is " + d.Dfa.DecisionNumber + "." + d.StateNumber );
                }
                return;
            }

            // set closure to be busy for this NFA configuration
            d.ClosureBusy.Add( proposedNFAConfiguration );

            // p itself is always in closure
            d.AddNFAConfiguration( p, proposedNFAConfiguration );

            // Case 1: are we a reference to another rule?
            Transition transition0 = p.transition[0];
            if ( transition0 is RuleClosureTransition )
            {
                int depth = context.RecursionDepthEmanatingFromState( p.StateNumber );
                // Detect recursion by more than a single alt, which indicates
                // that the decision's lookahead language is potentially non-regular; terminate
                if ( depth == 1 && d.Dfa.UserMaxLookahead == 0 )
                { // k=* only
                    d.Dfa.RecursiveAltSet.Add( alt ); // indicate that this alt is recursive
                    if ( d.Dfa.RecursiveAltSet.Count > 1 )
                    {
                        //JSystem.@out.println("recursive alts: "+d.dfa.recursiveAltSet.toString());
                        d.AbortedDueToMultipleRecursiveAlts = true;
                        throw new NonLLStarDecisionException( d.Dfa );
                    }
                    /*
                    JSystem.@out.println("alt "+alt+" in rule "+p.enclosingRule+" dec "+d.dfa.decisionNumber+
                        " ctx: "+context);
                    JSystem.@out.println("d="+d);
                    */
                }
                // Detect an attempt to recurse too high
                // if this context has hit the max recursions for p.stateNumber,
                // don't allow it to enter p.stateNumber again
                if ( depth >= NFAContext.MAX_SAME_RULE_INVOCATIONS_PER_NFA_CONFIG_STACK )
                {
                    /*
                    JSystem.@out.println("OVF state "+d);
                    JSystem.@out.println("proposed "+proposedNFAConfiguration);
                    */
                    d.AbortedDueToRecursionOverflow = true;
                    d.Dfa.Probe.ReportRecursionOverflow( d, proposedNFAConfiguration );
                    if ( debug )
                    {
                        Console.Out.WriteLine( "analysis overflow in closure(" + d.StateNumber + ")" );
                    }
                    return;
                }

                // otherwise, it's cool to (re)enter target of this rule ref
                RuleClosureTransition @ref = (RuleClosureTransition)transition0;
                // first create a new context and push onto call tree,
                // recording the fact that we are invoking a rule and
                // from which state (case 2 below will get the following state
                // via the RuleClosureTransition emanating from the invoking state
                // pushed on the stack).
                // Reset the context to reflect the fact we invoked rule
                NFAContext newContext = new NFAContext( context, p );
                //JSystem.@out.println("invoking rule "+ref.rule.name);
                // JSystem.@out.println(" context="+context);
                // traverse epsilon edge to new rule
                NFAState ruleTarget = (NFAState)@ref.Target;
                Closure( ruleTarget, alt, newContext, semanticContext, d, collectPredicates );
            }
            // Case 2: end of rule state, context (i.e., an invoker) exists
            else if ( p.IsAcceptState && context.Parent != null )
            {
                NFAState whichStateInvokedRule = context.InvokingState;
                RuleClosureTransition edgeToRule =
                    (RuleClosureTransition)whichStateInvokedRule.transition[0];
                NFAState continueState = edgeToRule.FollowState;
                NFAContext newContext = context.Parent; // "pop" invoking state
                Closure( continueState, alt, newContext, semanticContext, d, collectPredicates );
            }
            // Case 3: end of rule state, nobody invoked this rule (no context)
            //    Fall thru to be handled by case 4 automagically.
            // Case 4: ordinary NFA->DFA conversion case: simple epsilon transition
            else
            {
                // recurse down any epsilon transitions
                if ( transition0 != null && transition0.IsEpsilon )
                {
                    bool collectPredicatesAfterAction = collectPredicates;
                    if ( transition0.IsAction && collectPredicates )
                    {
                        collectPredicatesAfterAction = false;
                        /*
                        if ( computingStartState ) {
                            JSystem.@out.println("found action during prediction closure "+((ActionLabel)transition0.label).actionAST.token);
                        }
                         */
                    }
                    Closure( (NFAState)transition0.Target,
                            alt,
                            context,
                            semanticContext,
                            d,
                            collectPredicatesAfterAction
                    );
                }
                else if ( transition0 != null && transition0.IsSemanticPredicate )
                {
                    if ( _computingStartState )
                    {
                        if ( collectPredicates )
                        {
                            // only indicate we can see a predicate if we're collecting preds
                            // Could be computing start state & seen an action before this.
                            _dfa.PredicateVisible = true;
                        }
                        else
                        {
                            // this state has a pred, but we can't see it.
                            _dfa.HasPredicateBlockedByAction = true;
                            // JSystem.@out.println("found pred during prediction but blocked by action found previously");
                        }
                    }
                    // continue closure here too, but add the sem pred to ctx
                    SemanticContext newSemanticContext = semanticContext;
                    if ( collectPredicates )
                    {
                        // AND the previous semantic context with new pred
                        SemanticContext labelContext =
                            transition0.Label.SemanticContext;
                        // do not hoist syn preds from other rules; only get if in
                        // starting state's rule (i.e., context is empty)
                        int walkAlt =
                            _dfa.NFADecisionStartState.TranslateDisplayAltToWalkAlt( alt );
                        NFAState altLeftEdge =
                            _dfa.Nfa.Grammar.GetNFAStateForAltOfDecision( _dfa.NFADecisionStartState, walkAlt );
                        /*
                        JSystem.@out.println("state "+p.stateNumber+" alt "+alt+" walkAlt "+walkAlt+" trans to "+transition0.target);
                        JSystem.@out.println("DFA start state "+dfa.decisionNFAStartState.stateNumber);
                        JSystem.@out.println("alt left edge "+altLeftEdge.stateNumber+
                            ", epsilon target "+
                            altLeftEdge.transition(0).target.stateNumber);
                        */
                        if ( !labelContext.IsSyntacticPredicate ||
                             p == altLeftEdge.transition[0].Target )
                        {
                            //JSystem.@out.println("&"+labelContext+" enclosingRule="+p.enclosingRule);
                            newSemanticContext =
                                SemanticContext.And( semanticContext, labelContext );
                        }
                    }
                    Closure( (NFAState)transition0.Target,
                            alt,
                            context,
                            newSemanticContext,
                            d,
                            collectPredicates );
                }
                Transition transition1 = p.transition[1];
                if ( transition1 != null && transition1.IsEpsilon )
                {
                    Closure( (NFAState)transition1.Target,
                            alt,
                            context,
                            semanticContext,
                            d,
                            collectPredicates );
                }
            }

            // don't remove "busy" flag as we want to prevent all
            // references to same config of state|alt|ctx|semCtx even
            // if resulting from another NFA state
        }

        /** A closure operation should abort if that computation has already
         *  been done or a computation with a conflicting context has already
         *  been done.  If proposed NFA config's state and alt are the same
         *  there is potentially a problem.  If the stack context is identical
         *  then clearly the exact same computation is proposed.  If a context
         *  is a suffix of the other, then again the computation is in an
         *  identical context.  ?$ and ??$ are considered the same stack.
         *  We could walk configurations linearly doing the comparison instead
         *  of a set for exact matches but it's much slower because you can't
         *  do a Set lookup.  I use exact match as ANTLR
         *  always detect the conflict later when checking for context suffixes...
         *  I check for left-recursive stuff and terminate before analysis to
         *  avoid need to do this more expensive computation.
         *
         *  12-31-2007: I had to use the loop again rather than simple
         *  closureBusy.contains(proposedNFAConfiguration) lookup.  The
         *  semantic context should not be considered when determining if
         *  a closure operation is busy.  I saw a FOLLOW closure operation
         *  spin until time out because the predicate context kept increasing
         *  in size even though it's same boolean value.  This seems faster also
         *  because I'm not doing String.equals on the preds all the time.
         *
         *  05-05-2008: Hmm...well, i think it was a mistake to remove the sem
         *  ctx check below...adding back in.  Coincides with report of ANTLR
         *  getting super slow: http://www.antlr.org:8888/browse/ANTLR-235
         *  This could be because it doesn't properly compute then resolve
         *  a predicate expression.  Seems to fix unit test:
         *  TestSemanticPredicates.testSemanticContextPreventsEarlyTerminationOfClosure()
         *  Changing back to Set from List.  Changed a large grammar from 8 minutes
         *  to 11 seconds.  Cool.  Closing ANTLR-235.
         */
        public static bool ClosureIsBusy( DFAState d,
                                            NFAConfiguration proposedNFAConfiguration )
        {
            return d.ClosureBusy.Contains( proposedNFAConfiguration );
            /*
                    int numConfigs = d.closureBusy.size();
                    // Check epsilon cycle (same state, same alt, same context)
                    for (int i = 0; i < numConfigs; i++) {
                        NFAConfiguration c = (NFAConfiguration) d.closureBusy.get(i);
                        if ( proposedNFAConfiguration.state==c.state &&
                             proposedNFAConfiguration.alt==c.alt &&
                             proposedNFAConfiguration.semanticContext.equals(c.semanticContext) &&
                             proposedNFAConfiguration.context.suffix(c.context) )
                        {
                            return true;
                        }
                    }
                    return false;
                    */
        }

        /** Given the set of NFA states in DFA state d, find all NFA states
         *  reachable traversing label arcs.  By definition, there can be
         *  only one DFA state reachable by an atom from DFA state d so we must
         *  find and merge all NFA states reachable via label.  Return a new
         *  DFAState that has all of those NFA states with their context (i.e.,
         *  which alt do they predict and where to return to if they fall off
         *  end of a rule).
         *
         *  Because we cannot jump to another rule nor fall off the end of a rule
         *  via a non-epsilon transition, NFA states reachable from d have the
         *  same configuration as the NFA state in d.  So if NFA state 7 in d's
         *  configurations can reach NFA state 13 then 13 will be added to the
         *  new DFAState (labelDFATarget) with the same configuration as state
         *  7 had.
         *
         *  This method does not see EOT transitions off the end of token rule
         *  accept states if the rule was invoked by somebody.
         */
        public virtual DFAState Reach( DFAState d, Label label )
        {
            //JSystem.@out.println("reach "+label.toString(dfa.nfa.grammar)+" from "+d.stateNumber);
            DFAState labelDFATarget = _dfa.NewState();

            // for each NFA state in d with a labeled edge,
            // add in target states for label
            //JSystem.@out.println("size(d.state="+d.stateNumber+")="+d.nfaConfigurations.size());
            //JSystem.@out.println("size(labeled edge states)="+d.configurationsWithLabeledEdges.size());
            IList<NFAConfiguration> configs = d.ConfigurationsWithLabeledEdges;
            int numConfigs = configs.Count;
            for ( int i = 0; i < numConfigs; i++ )
            {
                NFAConfiguration c = configs[i];
                if ( c.Resolved || c.ResolveWithPredicate )
                {
                    continue; // the conflict resolver indicates we must leave alone
                }
                NFAState p = _dfa.Nfa.GetState( c.State );
                // by design of the grammar->NFA conversion, only transition 0
                // may have a non-epsilon edge.
                Transition edge = p.transition[0];
                if ( edge == null || !c.SingleAtomTransitionEmanating )
                {
                    continue;
                }
                Label edgeLabel = edge.Label;

                // SPECIAL CASE
                // if it's an EOT transition on end of lexer rule, but context
                // stack is not empty, then don't see the EOT; the closure
                // will have added in the proper states following the reference
                // to this rule in the invoking rule.  In other words, if
                // somebody called this rule, don't see the EOT emanating from
                // this accept state.
                if ( c.Context.Parent != null && edgeLabel.label == Label.EOT )
                {
                    continue;
                }

                // Labels not unique at this point (not until addReachableLabels)
                // so try simple int label match before general set intersection
                //JSystem.@out.println("comparing "+edgeLabel+" with "+label);
                if ( Label.Intersect( label, edgeLabel ) )
                {
                    // found a transition with label;
                    // add NFA target to (potentially) new DFA state
                    NFAConfiguration newC = labelDFATarget.AddNFAConfiguration(
                        (NFAState)edge.Target,
                        c.Alt,
                        c.Context,
                        c.SemanticContext );
                }
            }
            if ( labelDFATarget.NfaConfigurations.Count == 0 )
            {
                // kill; it's empty
                _dfa.SetState( labelDFATarget.StateNumber, null );
                labelDFATarget = null;
            }
            return labelDFATarget;
        }

        /** Walk the configurations of this DFA state d looking for the
         *  configuration, c, that has a transition on EOT.  State d should
         *  be converted to an accept state predicting the c.alt.  Blast
         *  d's current configuration set and make it just have config c.
         *
         *  TODO: can there be more than one config with EOT transition?
         *  That would mean that two NFA configurations could reach the
         *  end of the token with possibly different predicted alts.
         *  Seems like that would be rare or impossible.  Perhaps convert
         *  this routine to find all such configs and give error if >1.
         */
        protected virtual void ConvertToEOTAcceptState( DFAState d )
        {
            Label eot = new Label( Label.EOT );
            int numConfigs = d.NfaConfigurations.Count;
            for ( int i = 0; i < numConfigs; i++ )
            {
                NFAConfiguration c = d.NfaConfigurations[i];
                if ( c.Resolved || c.ResolveWithPredicate )
                {
                    continue; // the conflict resolver indicates we must leave alone
                }
                NFAState p = _dfa.Nfa.GetState( c.State );
                Transition edge = p.transition[0];
                Label edgeLabel = edge.Label;
                if ( edgeLabel.Equals( eot ) )
                {
                    //JSystem.@out.println("config with EOT: "+c);
                    d.IsAcceptState = true;
                    //JSystem.@out.println("d goes from "+d);
                    d.NfaConfigurations.Clear();
                    d.AddNFAConfiguration( p, c.Alt, c.Context, c.SemanticContext );
                    //JSystem.@out.println("to "+d);
                    return; // assume only one EOT transition
                }
            }
        }

        /** Add a new DFA state to the DFA if not already present.
         *  If the DFA state uniquely predicts a single alternative, it
         *  becomes a stop state; don't add to work list.  Further, if
         *  there exists an NFA state predicted by > 1 different alternatives
         *  and with the same syn and sem context, the DFA is nondeterministic for
         *  at least one input sequence reaching that NFA state.
         */
        protected virtual DFAState AddDFAStateToWorkList( DFAState d )
        {
            DFAState existingState = _dfa.AddState( d );
            if ( d != existingState )
            {
                // already there...use/return the existing DFA state.
                // But also set the states[d.stateNumber] to the existing
                // DFA state because the closureIsBusy must report
                // infinite recursion on a state before it knows
                // whether or not the state will already be
                // found after closure on it finishes.  It could be
                // referring to a state that will ultimately not make it
                // into the reachable state space and the error
                // reporting must be able to compute the path from
                // start to the error state with infinite recursion
                _dfa.SetState( d.StateNumber, existingState );
                return existingState;
            }

            // if not there, then examine new state.

            // resolve syntactic conflicts by choosing a single alt or
            // by using semantic predicates if present.
            ResolveNonDeterminisms( d );

            // If deterministic, don't add this state; it's an accept state
            // Just return as a valid DFA state
            int alt = d.GetUniquelyPredictedAlt();
            if ( alt != NFA.INVALID_ALT_NUMBER )
            { // uniquely predicts an alt?
                d = ConvertToAcceptState( d, alt );
                /*
                JSystem.@out.println("convert to accept; DFA "+d.dfa.decisionNumber+" state "+d.stateNumber+" uniquely predicts alt "+
                    d.getUniquelyPredictedAlt());
                    */
            }
            else
            {
                // unresolved, add to work list to continue NFA conversion
                _work.Enqueue( d );
            }
            return d;
        }

        protected virtual DFAState ConvertToAcceptState( DFAState d, int alt )
        {
            // only merge stop states if they are deterministic and no
            // recursion problems and only if they have the same gated pred
            // context!
            // Later, the error reporting may want to trace the path from
            // the start state to the nondet state
            if ( DFAOptimizer.MergeStopStates &&
                d.GetNonDeterministicAlts() == null &&
                !d.AbortedDueToRecursionOverflow &&
                !d.AbortedDueToMultipleRecursiveAlts )
            {
                // check to see if we already have an accept state for this alt
                // [must do this after we resolve nondeterminisms in general]
                DFAState acceptStateForAlt = _dfa.GetAcceptState( alt );
                if ( acceptStateForAlt != null )
                {
                    // we already have an accept state for alt;
                    // Are their gate sem pred contexts the same?
                    // For now we assume a braindead version: both must not
                    // have gated preds or share exactly same single gated pred.
                    // The equals() method is only defined on Predicate contexts not
                    // OR etc...
                    SemanticContext gatedPreds = d.GetGatedPredicatesInNFAConfigurations();
                    SemanticContext existingStateGatedPreds =
                        acceptStateForAlt.GetGatedPredicatesInNFAConfigurations();
                    if ( ( gatedPreds == null && existingStateGatedPreds == null ) ||
                         ( ( gatedPreds != null && existingStateGatedPreds != null ) &&
                          gatedPreds.Equals( existingStateGatedPreds ) ) )
                    {
                        // make this d.statenumber point at old DFA state
                        _dfa.SetState( d.StateNumber, acceptStateForAlt );
                        _dfa.RemoveState( d );    // remove this state from unique DFA state set
                        d = acceptStateForAlt; // use old accept state; throw this one out
                        return d;
                    }
                    // else consider it a new accept state; fall through.
                }
            }
            d.IsAcceptState = true; // new accept state for alt
            _dfa.SetAcceptState( alt, d );
            return d;
        }

        /** If > 1 NFA configurations within this DFA state have identical
         *  NFA state and context, but differ in their predicted
         *  TODO update for new context suffix stuff 3-9-2005
         *  alternative then a single input sequence predicts multiple alts.
         *  The NFA decision is therefore syntactically indistinguishable
         *  from the left edge upon at least one input sequence.  We may
         *  terminate the NFA to DFA conversion for these paths since no
         *  paths emanating from those NFA states can possibly separate
         *  these conjoined twins once interwined to make things
         *  deterministic (unless there are semantic predicates; see below).
         *
         *  Upon a nondeterministic set of NFA configurations, we should
         *  report a problem to the grammar designer and resolve the issue
         *  by aribitrarily picking the first alternative (this usually
         *  ends up producing the most natural behavior).  Pick the lowest
         *  alt number and just turn off all NFA configurations
         *  associated with the other alts. Rather than remove conflicting
         *  NFA configurations, I set the "resolved" bit so that future
         *  computations will ignore them.  In this way, we maintain the
         *  complete DFA state with all its configurations, but prevent
         *  future DFA conversion operations from pursuing undesirable
         *  paths.  Remember that we want to terminate DFA conversion as
         *  soon as we know the decision is deterministic *or*
         *  nondeterministic.
         *
         *  [BTW, I have convinced myself that there can be at most one
         *  set of nondeterministic configurations in a DFA state.  Only NFA
         *  configurations arising from the same input sequence can appear
         *  in a DFA state.  There is no way to have another complete set
         *  of nondeterministic NFA configurations without another input
         *  sequence, which would reach a different DFA state.  Therefore,
         *  the two nondeterministic NFA configuration sets cannot collide
         *  in the same DFA state.]
         *
         *  Consider DFA state {(s|1),(s|2),(s|3),(t|3),(v|4)} where (s|a)
         *  is state 's' and alternative 'a'.  Here, configuration set
         *  {(s|1),(s|2),(s|3)} predicts 3 different alts.  Configurations
         *  (s|2) and (s|3) are "resolved", leaving {(s|1),(t|3),(v|4)} as
         *  items that must still be considered by the DFA conversion
         *  algorithm in DFA.findNewDFAStatesAndAddDFATransitions().
         *
         *  Consider the following grammar where alts 1 and 2 are no
         *  problem because of the 2nd lookahead symbol.  Alts 3 and 4 are
         *  identical and will therefore reach the rule end NFA state but
         *  predicting 2 different alts (no amount of future lookahead
         *  will render them deterministic/separable):
         *
         *  a : A B
         *    | A C
         *    | A
         *    | A
         *    ;
         *
         *  Here is a (slightly reduced) NFA of this grammar:
         *
         *  (1)-A->(2)-B->(end)-EOF->(8)
         *   |              ^
         *  (2)-A->(3)-C----|
         *   |              ^
         *  (4)-A->(5)------|
         *   |              ^
         *  (6)-A->(7)------|
         *
         *  where (n) is NFA state n.  To begin DFA conversion, the start
         *  state is created:
         *
         *  {(1|1),(2|2),(4|3),(6|4)}
         *
         *  Upon A, all NFA configurations lead to new NFA states yielding
         *  new DFA state:
         *
         *  {(2|1),(3|2),(5|3),(7|4),(end|3),(end|4)}
         *
         *  where the configurations with state end in them are added
         *  during the epsilon closure operation.  State end predicts both
         *  alts 3 and 4.  An error is reported, the latter configuration is
         *  flagged as resolved leaving the DFA state as:
         *
         *  {(2|1),(3|2),(5|3),(7|4|resolved),(end|3),(end|4|resolved)}
         *
         *  As NFA configurations are added to a DFA state during its
         *  construction, the reachable set of labels is computed.  Here
         *  reachable is {B,C,EOF} because there is at least one NFA state
         *  in the DFA state that can transition upon those symbols.
         *
         *  The final DFA looks like:
         *
         *  {(1|1),(2|2),(4|3),(6|4)}
         *              |
         *              v
         *  {(2|1),(3|2),(5|3),(7|4),(end|3),(end|4)} -B-> (end|1)
         *              |                        |
         *              C                        ----EOF-> (8,3)
         *              |
         *              v
         *           (end|2)
         *
         *  Upon AB, alt 1 is predicted.  Upon AC, alt 2 is predicted.
         *  Upon A EOF, alt 3 is predicted.  Alt 4 is not a viable
         *  alternative.
         *
         *  The algorithm is essentially to walk all the configurations
         *  looking for a conflict of the form (s|i) and (s|j) for i!=j.
         *  Use a hash table to track state+context pairs for collisions
         *  so that we have O(n) to walk the n configurations looking for
         *  a conflict.  Upon every conflict, track the alt number so
         *  we have a list of all nondeterministically predicted alts. Also
         *  track the minimum alt.  Next go back over the configurations, setting
         *  the "resolved" bit for any that have an alt that is a member of
         *  the nondeterministic set.  This will effectively remove any alts
         *  but the one we want from future consideration.
         *
         *  See resolveWithSemanticPredicates()
         *
         *  AMBIGUOUS TOKENS
         *
         *  With keywords and ID tokens, there is an inherit ambiguity in that
         *  "int" can be matched by ID also.  Each lexer rule has an EOT
         *  transition emanating from it which is used whenever the end of
         *  a rule is reached and another token rule did not invoke it.  EOT
         *  is the only thing that can be seen next.  If two rules are identical
         *  like "int" and "int" then the 2nd def is unreachable and you'll get
         *  a warning.  We prevent a warning though for the keyword/ID issue as
         *  ID is still reachable.  This can be a bit weird.  '+' rule then a
         *  '+'|'+=' rule will fail to match '+' for the 2nd rule.
         *
         *  If all NFA states in this DFA state are targets of EOT transitions,
         *  (and there is more than one state plus no unique alt is predicted)
         *  then DFA conversion will leave this state as a dead state as nothing
         *  can be reached from this state.  To resolve the ambiguity, just do
         *  what flex and friends do: pick the first rule (alt in this case) to
         *  win.  This means you should put keywords before the ID rule.
         *  If the DFA state has only one NFA state then there is no issue:
         *  it uniquely predicts one alt. :)  Problem
         *  states will look like this during conversion:
         *
         *  DFA 1:{9|1, 19|2, 14|3, 20|2, 23|2, 24|2, ...}-<EOT>->5:{41|3, 42|2}
         *
         *  Worse, when you have two identical literal rules, you will see 3 alts
         *  in the EOT state (one for ID and one each for the identical rules).
         */
        public virtual void ResolveNonDeterminisms( DFAState d )
        {
            if ( debug )
            {
                Console.Out.WriteLine( "resolveNonDeterminisms " + d.ToString() );
            }
            bool conflictingLexerRules = false;
            HashSet<int> nondeterministicAlts = d.GetNonDeterministicAlts();
            if ( debug && nondeterministicAlts != null )
            {
                Console.Out.WriteLine( "nondet alts=" + nondeterministicAlts );
            }

            // CHECK FOR AMBIGUOUS EOT (if |allAlts|>1 and EOT state, resolve)
            // grab any config to see if EOT state; any other configs must
            // transition on EOT to get to this DFA state as well so all
            // states in d must be targets of EOT.  These are the end states
            // created in NFAFactory.build_EOFState
            NFAConfiguration anyConfig = d.NfaConfigurations[0];
            NFAState anyState = _dfa.Nfa.GetState( anyConfig.State );

            // if d is target of EOT and more than one predicted alt
            // indicate that d is nondeterministic on all alts otherwise
            // it looks like state has no problem
            if ( anyState.IsEOTTargetState )
            {
                HashSet<int> allAlts = new HashSet<int>( d.AltSet );
                // is more than 1 alt predicted?
                if ( allAlts != null && allAlts.Count > 1 )
                {
                    nondeterministicAlts = allAlts;
                    // track Tokens rule issues differently than other decisions
                    if ( d.Dfa.IsTokensRuleDecision )
                    {
                        _dfa.Probe.ReportLexerRuleNondeterminism( d, allAlts );
                        //JSystem.@out.println("Tokens rule DFA state "+d+" nondeterministic");
                        conflictingLexerRules = true;
                    }
                }
            }

            // if no problems return unless we aborted work on d to avoid inf recursion
            if ( !d.AbortedDueToRecursionOverflow && nondeterministicAlts == null )
            {
                return; // no problems, return
            }

            // if we're not a conflicting lexer rule and we didn't abort, report ambig
            // We should get a report for abort so don't give another
            if ( !d.AbortedDueToRecursionOverflow && !conflictingLexerRules )
            {
                // TODO: with k=x option set, this is called twice for same state
                _dfa.Probe.ReportNondeterminism( d, nondeterministicAlts );
                // TODO: how to turn off when it's only the FOLLOW that is
                // conflicting.  This used to shut off even alts i,j < n
                // conflict warnings. :(
            }

            // ATTEMPT TO RESOLVE WITH SEMANTIC PREDICATES
            bool resolved =
                TryToResolveWithSemanticPredicates( d, nondeterministicAlts );
            if ( resolved )
            {
                if ( debug )
                {
                    Console.Out.WriteLine( "resolved DFA state " + d.StateNumber + " with pred" );
                }
                d.IsResolvedWithPredicates = true;
                _dfa.Probe.ReportNondeterminismResolvedWithSemanticPredicate( d );
                return;
            }

            // RESOLVE SYNTACTIC CONFLICT BY REMOVING ALL BUT ONE ALT
            ResolveByChoosingFirstAlt( d, nondeterministicAlts );

            //JSystem.@out.println("state "+d.stateNumber+" resolved to alt "+winningAlt);
        }

        protected virtual int ResolveByChoosingFirstAlt( DFAState d, ICollection<int> nondeterministicAlts )
        {
            int winningAlt = 0;
            if ( _dfa.IsGreedy )
            {
                winningAlt = ResolveByPickingMinAlt( d, nondeterministicAlts );
            }
            else
            {
                // If nongreedy, the exit alt shout win, but only if it's
                // involved in the nondeterminism!
                /*
                JSystem.@out.println("resolving exit alt for decision="+
                    dfa.decisionNumber+" state="+d);
                JSystem.@out.println("nondet="+nondeterministicAlts);
                JSystem.@out.println("exit alt "+exitAlt);
                */
                int exitAlt = _dfa.NumberOfAlts;
                if ( nondeterministicAlts.Contains( exitAlt ) )
                {
                    // if nongreedy and exit alt is one of those nondeterministic alts
                    // predicted, resolve in favor of what follows block
                    winningAlt = ResolveByPickingExitAlt( d, nondeterministicAlts );
                }
                else
                {
                    winningAlt = ResolveByPickingMinAlt( d, nondeterministicAlts );
                }
            }
            return winningAlt;
        }

        /** Turn off all configurations associated with the
         *  set of incoming nondeterministic alts except the min alt number.
         *  There may be many alts among the configurations but only turn off
         *  the ones with problems (other than the min alt of course).
         *
         *  If nondeterministicAlts is null then turn off all configs 'cept those
         *  associated with the minimum alt.
         *
         *  Return the min alt found.
         */
        protected virtual int ResolveByPickingMinAlt( DFAState d, ICollection<int> nondeterministicAlts )
        {
            int min = int.MaxValue;
            if ( nondeterministicAlts != null )
            {
                min = GetMinAlt( nondeterministicAlts );
            }
            else
            {
                min = d.MinAltInConfigurations;
            }

            TurnOffOtherAlts( d, min, nondeterministicAlts );

            return min;
        }

        /** Resolve state d by choosing exit alt, which is same value as the
         *  number of alternatives.  Return that exit alt.
         */
        protected virtual int ResolveByPickingExitAlt( DFAState d, ICollection<int> nondeterministicAlts )
        {
            int exitAlt = _dfa.NumberOfAlts;
            TurnOffOtherAlts( d, exitAlt, nondeterministicAlts );
            return exitAlt;
        }

        /** turn off all states associated with alts other than the good one
         *  (as long as they are one of the nondeterministic ones)
         */
        protected static void TurnOffOtherAlts( DFAState d, int min, ICollection<int> nondeterministicAlts )
        {
            int numConfigs = d.NfaConfigurations.Count;
            for ( int i = 0; i < numConfigs; i++ )
            {
                NFAConfiguration configuration = d.NfaConfigurations[i];
                if ( configuration.Alt != min )
                {
                    if ( nondeterministicAlts == null ||
                         nondeterministicAlts.Contains( configuration.Alt ) )
                    {
                        configuration.Resolved = true;
                    }
                }
            }
        }

        protected static int GetMinAlt( ICollection<int> nondeterministicAlts )
        {
            int min = int.MaxValue;
            foreach ( int altI in nondeterministicAlts )
            {
                int alt = altI;
                if ( alt < min )
                {
                    min = alt;
                }
            }
            return min;
        }

        /** See if a set of nondeterministic alternatives can be disambiguated
         *  with the semantic predicate contexts of the alternatives.
         *
         *  Without semantic predicates, syntactic conflicts are resolved
         *  by simply choosing the first viable alternative.  In the
         *  presence of semantic predicates, you can resolve the issue by
         *  evaluating boolean expressions at run time.  During analysis,
         *  this amounts to suppressing grammar error messages to the
         *  developer.  NFA configurations are always marked as "to be
         *  resolved with predicates" so that
         *  DFA.findNewDFAStatesAndAddDFATransitions() will know to ignore
         *  these configurations and add predicate transitions to the DFA
         *  after adding token/char labels.
         *
         *  During analysis, we can simply make sure that for n
         *  ambiguously predicted alternatives there are at least n-1
         *  unique predicate sets.  The nth alternative can be predicted
         *  with "not" the "or" of all other predicates.  NFA configurations without
         *  predicates are assumed to have the default predicate of
         *  "true" from a user point of view.  When true is combined via || with
         *  another predicate, the predicate is a tautology and must be removed
         *  from consideration for disambiguation:
         *
         *  a : b | B ; // hoisting p1||true out of rule b, yields no predicate
         *  b : {p1}? B | B ;
         *
         *  This is done down in getPredicatesPerNonDeterministicAlt().
         */
        protected virtual bool TryToResolveWithSemanticPredicates( DFAState d,
                                                             ICollection<int> nondeterministicAlts )
        {
            IDictionary<int, SemanticContext> altToPredMap =
                    GetPredicatesPerNonDeterministicAlt( d, nondeterministicAlts );

            if ( altToPredMap.Count == 0 )
            {
                return false;
            }

            //JSystem.@out.println("nondeterministic alts with predicates: "+altToPredMap);
            _dfa.Probe.ReportAltPredicateContext( d, altToPredMap );

            if ( nondeterministicAlts.Count - altToPredMap.Count > 1 )
            {
                // too few predicates to resolve; just return
                return false;
            }

            // Handle case where 1 predicate is missing
            // Case 1. Semantic predicates
            // If the missing pred is on nth alt, !(union of other preds)==true
            // so we can avoid that computation.  If naked alt is ith, then must
            // test it with !(union) since semantic predicated alts are order
            // independent
            // Case 2: Syntactic predicates
            // The naked alt is always assumed to be true as the order of
            // alts is the order of precedence.  The naked alt will be a tautology
            // anyway as it's !(union of other preds).  This implies
            // that there is no such thing as noviable alt for synpred edges
            // emanating from a DFA state.
            if ( altToPredMap.Count == nondeterministicAlts.Count - 1 )
            {
                // if there are n-1 predicates for n nondeterministic alts, can fix
                BitSet ndSet = BitSet.Of( nondeterministicAlts );
                BitSet predSet = BitSet.Of( altToPredMap.Keys );
                int nakedAlt = ndSet.Subtract( predSet ).GetSingleElement();
                SemanticContext nakedAltPred = null;
                if ( nakedAlt == Max( nondeterministicAlts ) )
                {
                    // the naked alt is the last nondet alt and will be the default clause
                    nakedAltPred = new SemanticContext.TruePredicate();
                }
                else
                {
                    // pretend naked alternative is covered with !(union other preds)
                    // unless one of preds from other alts is a manually specified synpred
                    // since those have precedence same as alt order.  Missing synpred
                    // is true so that alt wins (or is at least attempted).
                    // Note: can't miss any preds on alts (can't be here) if auto backtrack
                    // since it prefixes all.
                    // In LL(*) paper, i'll just have algorithm emit warning about uncovered
                    // pred
                    SemanticContext unionOfPredicatesFromAllAlts =
                        GetUnionOfPredicates( altToPredMap );
                    //JSystem.@out.println("all predicates "+unionOfPredicatesFromAllAlts);
                    if ( unionOfPredicatesFromAllAlts.IsSyntacticPredicate )
                    {
                        nakedAltPred = new SemanticContext.TruePredicate();
                    }
                    else
                    {
                        nakedAltPred =
                            SemanticContext.Not( unionOfPredicatesFromAllAlts );
                    }
                }

                //JSystem.@out.println("covering naked alt="+nakedAlt+" with "+nakedAltPred);

                altToPredMap[nakedAlt] = nakedAltPred;
                // set all config with alt=nakedAlt to have the computed predicate
                int numConfigs = d.NfaConfigurations.Count;
                for ( int i = 0; i < numConfigs; i++ )
                {
                    // TODO: I don't think we need to do this; altToPredMap has it
                    //7/27/10  theok, I removed it and it still seems to work with everything; leave in anyway just in case
                    NFAConfiguration configuration = d.NfaConfigurations[i];
                    if ( configuration.Alt == nakedAlt )
                    {
                        configuration.SemanticContext = nakedAltPred;
                    }
                }
            }

            if ( altToPredMap.Count == nondeterministicAlts.Count )
            {
                // RESOLVE CONFLICT by picking one NFA configuration for each alt
                // and setting its resolvedWithPredicate flag
                // First, prevent a recursion warning on this state due to
                // pred resolution
                if ( d.AbortedDueToRecursionOverflow )
                {
                    d.Dfa.Probe.RemoveRecursiveOverflowState( d );
                }
                int numConfigs = d.NfaConfigurations.Count;
                //System.out.println("pred map="+altToPredMap);
                for ( int i = 0; i < numConfigs; i++ )
                {
                    NFAConfiguration configuration = d.NfaConfigurations[i];
                    SemanticContext semCtx;
                    altToPredMap.TryGetValue(configuration.Alt, out semCtx);
                    if ( semCtx != null )
                    {
                        // resolve (first found) with pred
                        // and remove alt from problem list
                        //System.out.println("c="+configuration);
                        configuration.ResolveWithPredicate = true;
                        // altToPredMap has preds from all alts; store into "annointed" config
                        configuration.SemanticContext = semCtx; // reset to combined
                        altToPredMap.Remove( configuration.Alt );

                        // notify grammar that we've used the preds contained in semCtx
                        if ( semCtx.IsSyntacticPredicate )
                        {
                            _dfa.Nfa.Grammar.SynPredUsedInDFA( _dfa, semCtx );
                        }
                    }
                    else if ( nondeterministicAlts.Contains( configuration.Alt ) )
                    {
                        // resolve all configurations for nondeterministic alts
                        // for which there is no predicate context by turning it off
                        configuration.Resolved = true;
                    }
                }
                return true;
            }

            return false;  // couldn't fix the problem with predicates
        }

        /** Return a mapping from nondeterministc alt to combined list of predicates.
         *  If both (s|i|semCtx1) and (t|i|semCtx2) exist, then the proper predicate
         *  for alt i is semCtx1||semCtx2 because you have arrived at this single
         *  DFA state via two NFA paths, both of which have semantic predicates.
         *  We ignore deterministic alts because syntax alone is sufficient
         *  to predict those.  Do not include their predicates.
         *
         *  Alts with no predicate are assumed to have {true}? pred.
         *
         *  When combining via || with "true", all predicates are removed from
         *  consideration since the expression will always be true and hence
         *  not tell us how to resolve anything.  So, if any NFA configuration
         *  in this DFA state does not have a semantic context, the alt cannot
         *  be resolved with a predicate.
         *
         *  If nonnull, incidentEdgeLabel tells us what NFA transition label
         *  we did a reach on to compute state d.  d may have insufficient
         *  preds, so we really want this for the error message.
         */
        protected virtual IDictionary<int, SemanticContext> GetPredicatesPerNonDeterministicAlt( DFAState d,
                                                                                    ICollection<int> nondeterministicAlts )
        {
            // map alt to combined SemanticContext
            IDictionary<int, SemanticContext> altToPredicateContextMap =
                new Dictionary<int, SemanticContext>();
            // init the alt to predicate set map
            IDictionary<int, OrderedHashSet<SemanticContext>> altToSetOfContextsMap =
                new Dictionary<int, OrderedHashSet<SemanticContext>>();
            foreach ( int altI in nondeterministicAlts )
            {
                altToSetOfContextsMap[altI] = new OrderedHashSet<SemanticContext>();
            }

            /*
            IList<Label> sampleInputLabels = d.dfa.probe.getSampleNonDeterministicInputSequence(d);
            String input = d.dfa.probe.getInputSequenceDisplay(sampleInputLabels);
            JSystem.@out.println("sample input: "+input);
            */

            // for each configuration, create a unique set of predicates
            // Also, track the alts with at least one uncovered configuration
            // (one w/o a predicate); tracks tautologies like p1||true
            IDictionary<int, ICollection<IToken>> altToLocationsReachableWithoutPredicate = new Dictionary<int, ICollection<IToken>>();
            HashSet<int> nondetAltsWithUncoveredConfiguration = new HashSet<int>();
            //JSystem.@out.println("configs="+d.nfaConfigurations);
            //JSystem.@out.println("configs with preds?"+d.atLeastOneConfigurationHasAPredicate);
            //JSystem.@out.println("configs with preds="+d.configurationsWithPredicateEdges);
            int numConfigs = d.NfaConfigurations.Count;
            for ( int i = 0; i < numConfigs; i++ )
            {
                NFAConfiguration configuration = d.NfaConfigurations[i];
                int altI = configuration.Alt;
                // if alt is nondeterministic, combine its predicates
                if ( nondeterministicAlts.Contains( altI ) )
                {
                    // if there is a predicate for this NFA configuration, OR in
                    if ( configuration.SemanticContext !=
                         SemanticContext.EmptySemanticContext )
                    {
                        OrderedHashSet<SemanticContext> predSet;
                        altToSetOfContextsMap.TryGetValue(altI, out predSet);
                        predSet.Add( configuration.SemanticContext );
                    }
                    else
                    {
                        // if no predicate, but it's part of nondeterministic alt
                        // then at least one path exists not covered by a predicate.
                        // must remove predicate for this alt; track incomplete alts
                        nondetAltsWithUncoveredConfiguration.Add( altI );
                        /*
                        NFAState s = dfa.nfa.getState(configuration.state);
                        JSystem.@out.println("###\ndec "+dfa.decisionNumber+" alt "+configuration.alt+
                                           " enclosing rule for nfa state not covered "+
                                           s.enclosingRule);
                        if ( s.associatedASTNode!=null ) {
                            JSystem.@out.println("token="+s.associatedASTNode.token);
                        }
                        JSystem.@out.println("nfa state="+s);

                        if ( s.incidentEdgeLabel!=null && Label.intersect(incidentEdgeLabel, s.incidentEdgeLabel) ) {
                            Set<Token> locations = altToLocationsReachableWithoutPredicate.get(altI);
                            if ( locations==null ) {
                                locations = new HashSet<Token>();
                                altToLocationsReachableWithoutPredicate.put(altI, locations);
                            }
                            locations.add(s.associatedASTNode.token);
                        }
                        */
                    }
                }
            }

            // For each alt, OR together all unique predicates associated with
            // all configurations
            // Also, track the list of incompletely covered alts: those alts
            // with at least 1 predicate and at least one configuration w/o a
            // predicate. We want this in order to report to the decision probe.
            IList<int> incompletelyCoveredAlts = new List<int>();
            foreach ( int altI in nondeterministicAlts )
            {
                OrderedHashSet<SemanticContext> contextsForThisAlt;
                altToSetOfContextsMap.TryGetValue(altI, out contextsForThisAlt);
                if ( nondetAltsWithUncoveredConfiguration.Contains( altI ) )
                { // >= 1 config has no ctx
                    if ( contextsForThisAlt.Count > 0 )
                    {    // && at least one pred
                        incompletelyCoveredAlts.Add( altI );  // this alt incompleted covered
                    }
                    continue; // don't include at least 1 config has no ctx
                }
                SemanticContext combinedContext = null;
                foreach ( SemanticContext ctx in contextsForThisAlt )
                {
                    combinedContext = SemanticContext.Or( combinedContext, ctx );
                }
                altToPredicateContextMap[altI] = combinedContext;
            }

            if ( incompletelyCoveredAlts.Count > 0 )
            {
                /*
                JSystem.@out.println("prob in dec "+dfa.decisionNumber+" state="+d);
                FASerializer serializer = new FASerializer(dfa.nfa.grammar);
                String result = serializer.serialize(dfa.startState);
                JSystem.@out.println("dfa: "+result);
                JSystem.@out.println("incomplete alts: "+incompletelyCoveredAlts);
                JSystem.@out.println("nondet="+nondeterministicAlts);
                JSystem.@out.println("nondetAltsWithUncoveredConfiguration="+ nondetAltsWithUncoveredConfiguration);
                JSystem.@out.println("altToCtxMap="+altToSetOfContextsMap);
                JSystem.@out.println("altToPredicateContextMap="+altToPredicateContextMap);
                */
                for ( int i = 0; i < numConfigs; i++ )
                {
                    NFAConfiguration configuration = d.NfaConfigurations[i];
                    int altI = configuration.Alt;
                    if ( incompletelyCoveredAlts.Contains( altI ) &&
                         configuration.SemanticContext == SemanticContext.EmptySemanticContext )
                    {
                        NFAState s = _dfa.Nfa.GetState( configuration.State );
                        /*
                        JSystem.@out.print("nondet config w/o context "+configuration+
                                         " incident "+(s.incidentEdgeLabel!=null?s.incidentEdgeLabel.toString(dfa.nfa.grammar):null));
                        if ( s.associatedASTNode!=null ) {
                            JSystem.@out.print(" token="+s.associatedASTNode.token);
                        }
                        else JSystem.@out.println();
                        */
                        // We want to report getting to an NFA state with an
                        // incoming label, unless it's EOF, w/o a predicate.
                        if ( s.incidentEdgeLabel != null && s.incidentEdgeLabel.label != Label.EOF )
                        {
                            if ( s.associatedASTNode == null || s.associatedASTNode.Token == null )
                            {
                                ErrorManager.InternalError( "no AST/token for nonepsilon target w/o predicate" );
                            }
                            else
                            {
                                ICollection<IToken> locations;
                                altToLocationsReachableWithoutPredicate.TryGetValue(altI, out locations);
                                if ( locations == null )
                                {
                                    locations = new HashSet<IToken>();
                                    altToLocationsReachableWithoutPredicate[altI] = locations;
                                }
                                locations.Add( s.associatedASTNode.Token );
                            }
                        }
                    }
                }
                _dfa.Probe.ReportIncompletelyCoveredAlts( d,
                                                        altToLocationsReachableWithoutPredicate );
            }

            return altToPredicateContextMap;
        }

        /** OR together all predicates from the alts.  Note that the predicate
         *  for an alt could itself be a combination of predicates.
         */
        protected static SemanticContext GetUnionOfPredicates( IDictionary<int, SemanticContext> altToPredMap )
        {
            SemanticContext unionOfPredicatesFromAllAlts = null;
            foreach ( SemanticContext semCtx in altToPredMap.Values )
            {
                if ( unionOfPredicatesFromAllAlts == null )
                {
                    unionOfPredicatesFromAllAlts = semCtx;
                }
                else
                {
                    unionOfPredicatesFromAllAlts =
                            SemanticContext.Or( unionOfPredicatesFromAllAlts, semCtx );
                }
            }
            return unionOfPredicatesFromAllAlts;
        }

        class AddPredicateTransitionsComparer : IComparer<NFAConfiguration>
        {
            public virtual int Compare( NFAConfiguration ca, NFAConfiguration cb )
            {
                if ( ca.Alt < cb.Alt )
                    return -1;
                else if ( ca.Alt > cb.Alt )
                    return 1;
                return 0;
            }
        }

        /** for each NFA config in d, look for "predicate required" sign set
         *  during nondeterminism resolution.
         *
         *  Add the predicate edges sorted by the alternative number; I'm fairly
         *  sure that I could walk the configs backwards so they are added to
         *  the predDFATarget in the right order, but it's best to make sure.
         *  Predicates succeed in the order they are specifed.  Alt i wins
         *  over alt i+1 if both predicates are true.
         */
        protected virtual void AddPredicateTransitions( DFAState d )
        {
            List<NFAConfiguration> configsWithPreds = new List<NFAConfiguration>();
            // get a list of all configs with predicates
            int numConfigs = d.NfaConfigurations.Count;
            for ( int i = 0; i < numConfigs; i++ )
            {
                NFAConfiguration c = d.NfaConfigurations[i];
                if ( c.ResolveWithPredicate )
                {
                    configsWithPreds.Add( c );
                }
            }
            // Sort ascending according to alt; alt i has higher precedence than i+1
            configsWithPreds.Sort( new AddPredicateTransitionsComparer() );
            //Collections.sort( configsWithPreds, new AddPredicateTransitionsComparer() );
            IList<NFAConfiguration> predConfigsSortedByAlt = configsWithPreds;
            // Now, we can add edges emanating from d for these preds in right order
            for ( int i = 0; i < predConfigsSortedByAlt.Count; i++ )
            {
                NFAConfiguration c = predConfigsSortedByAlt[i];
                DFAState predDFATarget = d.Dfa.GetAcceptState( c.Alt );
                if ( predDFATarget == null )
                {
                    predDFATarget = _dfa.NewState(); // create if not there.
                    // create a new DFA state that is a target of the predicate from d
                    predDFATarget.AddNFAConfiguration( _dfa.Nfa.GetState( c.State ),
                                                      c.Alt,
                                                      c.Context,
                                                      c.SemanticContext );
                    predDFATarget.IsAcceptState = true;
                    _dfa.SetAcceptState( c.Alt, predDFATarget );
                    DFAState existingState = _dfa.AddState( predDFATarget );
                    if ( predDFATarget != existingState )
                    {
                        // already there...use/return the existing DFA state that
                        // is a target of this predicate.  Make this state number
                        // point at the existing state
                        _dfa.SetState( predDFATarget.StateNumber, existingState );
                        predDFATarget = existingState;
                    }
                }
                // add a transition to pred target from d
                d.AddTransition( predDFATarget, new PredicateLabel( c.SemanticContext ) );
            }
        }

        protected virtual void InitContextTrees( int numberOfAlts )
        {
            _contextTrees = new NFAContext[numberOfAlts];
            for ( int i = 0; i < _contextTrees.Length; i++ )
            {
                int alt = i + 1;
                // add a dummy root node so that an NFA configuration can
                // always point at an NFAContext.  If a context refers to this
                // node then it implies there is no call stack for
                // that configuration
                _contextTrees[i] = new NFAContext( null, null );
            }
        }

        public static int Max( ICollection<int> s )
        {
            if ( s == null )
                return int.MinValue;

            if ( s.Count == 0 )
                return 0;

            return s.Max();
        }
    }
}
