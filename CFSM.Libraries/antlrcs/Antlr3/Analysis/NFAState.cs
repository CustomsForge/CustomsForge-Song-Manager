﻿/*
 * [The "BSD licence"]
 * Copyright (c) 2005-2008 Terence Parr
 * All rights reserved.
 *
 * Conversion to C#:
 * Copyright (c) 2008 Sam Harwell, Pixel Mine, Inc.
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
    using ErrorManager = Antlr3.Tool.ErrorManager;
    using GrammarAST = Antlr3.Tool.GrammarAST;
    using ArgumentException = System.ArgumentException;
    using Rule = Antlr3.Tool.Rule;

    /** A state within an NFA. At most 2 transitions emanate from any NFA state. */
    // sealed to make functions non-virtual
    public sealed class NFAState : State
    {
        // I need to distinguish between NFA decision states for (...)* and (...)+
        // during NFA interpretation.
        public const int LOOPBACK = 1;
        public const int BLOCK_START = 2;
        public const int OPTIONAL_BLOCK_START = 3;
        public const int BYPASS = 4;
        public const int RIGHT_EDGE_OF_BLOCK = 5;

        public const int MAX_TRANSITIONS = 2;

        /** How many transitions; 0, 1, or 2 transitions */
        int _numTransitions = 0;
        public Transition[] transition = new Transition[MAX_TRANSITIONS];

        /** For o-A->o type NFA tranitions, record the label that leads to this
         *  state.  Useful for creating rich error messages when we find
         *  insufficiently (with preds) covered states.
         */
        public Label incidentEdgeLabel;

        /** Which NFA are we in? */
        public NFA nfa;

        /** What's its decision number from 1..n? */
        int _decisionNumber;

        /** Subrules (...)* and (...)+ have more than one decision point in
         *  the NFA created for them.  They both have a loop-exit-or-stay-in
         *  decision node (the loop back node).  They both have a normal
         *  alternative block decision node at the left edge.  The (...)* is
         *  worse as it even has a bypass decision (2 alts: stay in or bypass)
         *  node at the extreme left edge.  This is not how they get generated
         *  in code as a while-loop or whatever deals nicely with either.  For
         *  error messages (where I need to print the nondeterministic alts)
         *  and for interpretation, I need to use the single DFA that is created
         *  (for efficiency) but interpret the results differently depending
         *  on which of the 2 or 3 decision states uses the DFA.  For example,
         *  the DFA will always report alt n+1 as the exit branch for n real
         *  alts, so I need to translate that depending on the decision state.
         *
         *  If decisionNumber>0 then this var tells you what kind of decision
         *  state it is.
         */
        public int decisionStateType;

        /** What rule do we live in? */
        public Rule enclosingRule;

        /** During debugging and for nondeterminism warnings, it's useful
         *  to know what relationship this node has to the original grammar.
         *  For example, "start of alt 1 of rule a".
         */
        string _description;

        /** Associate this NFAState with the corresponding GrammarAST node
         *  from which this node was created.  This is useful not only for
         *  associating the eventual lookahead DFA with the associated
         *  Grammar position, but also for providing users with
         *  nondeterminism warnings.  Mainly used by decision states to
         *  report line:col info.  Could also be used to track line:col
         *  for elements such as token refs.
         */
        public GrammarAST associatedASTNode;

        /** Is this state the sole target of an EOT transition? */
        bool _eotTargetState = false;

        /** Jean Bovet needs in the GUI to know which state pairs correspond
         *  to the start/stop of a block.
          */
        public int endOfBlockStateNumber = State.INVALID_STATE_NUMBER;

        public NFAState( NFA nfa )
        {
            this.nfa = nfa;
        }

        #region Properties
        public int DecisionNumber
        {
            get
            {
                return _decisionNumber;
            }
            set
            {
                _decisionNumber = value;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        public bool IsDecisionState
        {
            get
            {
                return decisionStateType > 0;
            }
        }
        public bool IsEOTTargetState
        {
            get
            {
                return _eotTargetState;
            }
            set
            {
                _eotTargetState = value;
            }
        }
        #endregion

        public override int NumberOfTransitions
        {
            get
            {
                return _numTransitions;
            }
        }

        public override void AddTransition( Transition e )
        {
            if ( e == null )
            {
                throw new ArgumentException( "You can't add a null transition" );
            }
            if ( _numTransitions > transition.Length )
            {
                throw new ArgumentException( "You can only have " + transition.Length + " transitions" );
            }
            if ( e != null )
            {
                transition[_numTransitions] = e;
                _numTransitions++;
                // Set the "back pointer" of the target state so that it
                // knows about the label of the incoming edge.
                Label label = e.Label;
                if ( label.IsAtom || label.IsSet )
                {
                    if ( ( (NFAState)e.Target ).incidentEdgeLabel != null )
                    {
                        ErrorManager.InternalError( "Clobbered incident edge" );
                    }
                    ( (NFAState)e.Target ).incidentEdgeLabel = e.Label;
                }
            }
        }

        /** Used during optimization to reset a state to have the (single)
         *  transition another state has.
         */
        public void SetTransition0( Transition e )
        {
            if ( e == null )
            {
                throw new ArgumentException( "You can't use a solitary null transition" );
            }
            transition[0] = e;
            transition[1] = null;
            _numTransitions = 1;
        }

        public override Transition GetTransition( int i )
        {
            return transition[i];
        }

        /** The DFA decision for this NFA decision state always has
         *  an exit path for loops as n+1 for n alts in the loop.
         *  That is really useful for displaying nondeterministic alts
         *  and so on, but for walking the NFA to get a sequence of edge
         *  labels or for actually parsing, we need to get the real alt
         *  number.  The real alt number for exiting a loop is always 1
         *  as transition 0 points at the exit branch (we compute DFAs
         *  always for loops at the loopback state).
         *
         *  For walking/parsing the loopback state:
         * 		1 2 3 display alt (for human consumption)
         * 		2 3 1 walk alt
         *
         *  For walking the block start:
         * 		1 2 3 display alt
         * 		1 2 3
         *
         *  For walking the bypass state of a (...)* loop:
         * 		1 2 3 display alt
         * 		1 1 2 all block alts map to entering loop exit means take bypass
         *
         *  Non loop EBNF do not need to be translated; they are ignored by
         *  this method as decisionStateType==0.
         *
         *  Return same alt if we can't translate.
         */
        public int TranslateDisplayAltToWalkAlt( int displayAlt )
        {
            NFAState nfaStart = this;
            if ( _decisionNumber == 0 || decisionStateType == 0 )
            {
                return displayAlt;
            }
            int walkAlt = 0;
            // find the NFA loopback state associated with this DFA
            // and count number of alts (all alt numbers are computed
            // based upon the loopback's NFA state.
            /*
            DFA dfa = nfa.grammar.getLookaheadDFA(decisionNumber);
            if ( dfa==null ) {
                ErrorManager.internalError("can't get DFA for decision "+decisionNumber);
            }
            */
            int nAlts = nfa.Grammar.GetNumberOfAltsForDecisionNFA( nfaStart );
            switch ( nfaStart.decisionStateType )
            {
            case LOOPBACK:
                walkAlt = displayAlt % nAlts + 1; // rotate right mod 1..3
                break;
            case BLOCK_START:
            case OPTIONAL_BLOCK_START:
                walkAlt = displayAlt; // identity transformation
                break;
            case BYPASS:
                if ( displayAlt == nAlts )
                {
                    walkAlt = 2; // bypass
                }
                else
                {
                    walkAlt = 1; // any non exit branch alt predicts entering
                }
                break;
            }
            return walkAlt;
        }

        // Setter/Getters

        /** What AST node is associated with this NFAState?  When you
         *  set the AST node, I set the node to point back to this NFA state.
         */
        public void SetDecisionASTNode( GrammarAST decisionASTNode )
        {
            decisionASTNode.NFAStartState = this;
            this.associatedASTNode = decisionASTNode;
        }

        public override string ToString()
        {
            return StateNumber.ToString();
        }

    }

}
