/*
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

namespace Antlr3.Tool
{
    using System.Collections.Generic;
    using Antlr3.Analysis;

    using Environment = System.Environment;
    using StringBuilder = System.Text.StringBuilder;

    /** An aspect of FA (finite automata) that knows how to dump them to serialized
     *  strings.
     */
    public class FASerializer
    {
        /** To prevent infinite recursion when walking state machines, record
         *  which states we've visited.  Make a new set every time you start
         *  walking in case you reuse this object.  Multiple threads will trash
         *  this shared variable.  Use a different FASerializer per thread.
         */
        protected HashSet<State> markedStates;

        /** Each state we walk will get a new state number for serialization
         *  purposes.  This is the variable that tracks state numbers.
         */
        protected int stateCounter = 0;

        /** Rather than add a new instance variable to NFA and DFA just for
         *  serializing machines, map old state numbers to new state numbers
         *  by a State object -> Integer new state number HashMap.
         */
        protected IDictionary<State, int> stateNumberTranslator;

        protected Grammar grammar;

        /** This aspect is associated with a grammar; used to get token names */
        public FASerializer( Grammar grammar )
        {
            this.grammar = grammar;
        }

        public virtual string Serialize( State s )
        {
            if ( s == null )
            {
                return "<no automaton>";
            }
            return Serialize( s, true );
        }

        /** Return a string representation of a state machine.  Two identical
         *  NFAs or DFAs will have identical serialized representations.  The
         *  state numbers inside the state are not used; instead, a new number
         *  is computed and because the serialization will walk the two
         *  machines using the same specific algorithm, then the state numbers
         *  will be identical.  Accept states are distinguished from regular
         *  states.
         */
        public virtual string Serialize( State s, bool renumber )
        {
            markedStates = new HashSet<State>();
            stateCounter = 0;
            if ( renumber )
            {
                stateNumberTranslator = new Dictionary<State, int>();
                WalkFANormalizingStateNumbers( s );
            }
            List<string> lines = new List<string>();
            if ( s.NumberOfTransitions > 0 )
            {
                WalkSerializingFA( lines, s );
            }
            else
            {
                // special case: s0 is an accept
                string s0 = GetStateString( 0, s );
                lines.Add( s0 + Environment.NewLine );
            }
            StringBuilder buf = new StringBuilder( 0 );
            // sort lines to normalize; makes states come out ordered
            // and then ordered by edge labels then by target state number :)
            lines.Sort( System.StringComparer.Ordinal );
            for ( int i = 0; i < lines.Count; i++ )
            {
                string line = (string)lines[i];
                buf.Append( line );
            }
            return buf.ToString();
        }

        /** In stateNumberTranslator, get a map from State to new, normalized
         *  state number.  Used by walkSerializingFA to make sure any two
         *  identical state machines will serialize the same way.
         */
        protected virtual void WalkFANormalizingStateNumbers( State s )
        {
            if ( s == null )
            {
                ErrorManager.InternalError( "null state s" );
                return;
            }
            if ( stateNumberTranslator.ContainsKey( s ) )
            {
                return; // already did this state
            }
            // assign a new state number for this node if there isn't one
            stateNumberTranslator[s] = stateCounter;
            stateCounter++;

            // visit nodes pointed to by each transition;
            for ( int i = 0; i < s.NumberOfTransitions; i++ )
            {
                Transition edge = (Transition)s.GetTransition( i );
                WalkFANormalizingStateNumbers( edge.Target ); // keep walkin'
                // if this transition is a rule reference, the node "following" this state
                // will not be found and appear to be not in graph.  Must explicitly jump
                // to it, but don't "draw" an edge.
                if ( edge is RuleClosureTransition )
                {
                    WalkFANormalizingStateNumbers( ( (RuleClosureTransition)edge ).FollowState );
                }
            }
        }

        protected virtual void WalkSerializingFA( IList<string> lines, State s )
        {
            if ( markedStates.Contains( s ) )
            {
                return; // already visited this node
            }

            markedStates.Add( s ); // mark this node as completed.

            int normalizedStateNumber = s.StateNumber;
            if ( stateNumberTranslator != null )
            {
                normalizedStateNumber = stateNumberTranslator[s];
            }

            string stateStr = GetStateString( normalizedStateNumber, s );

            // depth first walk each transition, printing its edge first
            for ( int i = 0; i < s.NumberOfTransitions; i++ )
            {
                Transition edge = (Transition)s.GetTransition( i );
                StringBuilder buf = new StringBuilder();
                buf.Append( stateStr );
                if ( edge.IsAction )
                {
                    buf.Append( "-{}->" );
                }
                else if ( edge.IsEpsilon )
                {
                    buf.Append( "->" );
                }
                else if ( edge.IsSemanticPredicate )
                {
                    buf.Append( "-{" + edge.Label.SemanticContext + "}?->" );
                }
                else
                {
                    string predsStr = "";
                    if ( edge.Target is DFAState )
                    {
                        // look for gated predicates; don't add gated to simple sempred edges
                        SemanticContext preds =
                            ( (DFAState)edge.Target ).GetGatedPredicatesInNFAConfigurations();
                        if ( preds != null )
                        {
                            predsStr = "&&{" +
                                preds.GenExpr( grammar.generator,
                                              grammar.generator.Templates, null ).Render()
                                + "}?";
                        }
                    }
                    buf.Append( "-" + edge.Label.ToString( grammar ) + predsStr + "->" );
                }

                int normalizedTargetStateNumber = edge.Target.StateNumber;
                if ( stateNumberTranslator != null )
                {
                    normalizedTargetStateNumber = stateNumberTranslator[edge.Target];
                }
                buf.Append( GetStateString( normalizedTargetStateNumber, edge.Target ) );
                buf.AppendLine();
                lines.Add( buf.ToString() );

                // walk this transition
                WalkSerializingFA( lines, edge.Target );

                // if this transition is a rule reference, the node "following" this state
                // will not be found and appear to be not in graph.  Must explicitly jump
                // to it, but don't "draw" an edge.
                if ( edge is RuleClosureTransition )
                {
                    WalkSerializingFA( lines, ( (RuleClosureTransition)edge ).FollowState );
                }
            }

        }

        private string GetStateString( int n, State s )
        {
            string stateStr = ".s" + n;
            if ( s.IsAcceptState )
            {
                if ( s is DFAState )
                {
                    stateStr = ":s" + n + "=>" + ( (DFAState)s ).GetUniquelyPredictedAlt();
                }
                else
                {
                    stateStr = ":s" + n;
                }
            }
            return stateStr;
        }


    }
}
