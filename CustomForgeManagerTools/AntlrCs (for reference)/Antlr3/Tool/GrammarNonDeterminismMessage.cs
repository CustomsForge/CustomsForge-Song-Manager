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
    using Antlr3.Extensions;

    using DecisionProbe = Antlr3.Analysis.DecisionProbe;
    using DFAState = Antlr3.Analysis.DFAState;
    using NFAState = Antlr3.Analysis.NFAState;
    using StringTemplate = Antlr4.StringTemplate.Template;

    /** Reports a potential parsing issue with a decision; the decision is
     *  nondeterministic in some way.
     */
    public class GrammarNonDeterminismMessage : Message
    {
        public DecisionProbe probe;
        public DFAState problemState;

        public GrammarNonDeterminismMessage( DecisionProbe probe,
                                            DFAState problemState )
            : base( ErrorManager.MSG_GRAMMAR_NONDETERMINISM )
        {
            this.probe = probe;
            this.problemState = problemState;
            // flip msg ID if alts are actually token refs in Tokens rule
            if ( probe.Dfa.IsTokensRuleDecision )
            {
                MessageID = ErrorManager.MSG_TOKEN_NONDETERMINISM;
            }
        }

        public override string ToString()
        {
            GrammarAST decisionASTNode = probe.Dfa.DecisionASTNode;
            line = decisionASTNode.Line;
            charPositionInLine = decisionASTNode.CharPositionInLine;
            string fileName = probe.Dfa.Nfa.Grammar.FileName;
            if ( fileName != null )
            {
                file = fileName;
            }

            StringTemplate st = GetMessageTemplate();
            // Now fill template with information about problemState
            var labels = probe.GetSampleNonDeterministicInputSequence( problemState );
            string input = probe.GetInputSequenceDisplay( labels );
            st.SetAttribute( "input", input );

            if ( probe.Dfa.IsTokensRuleDecision )
            {
                var disabledAlts = probe.GetDisabledAlternatives( problemState );
                foreach ( int altI in disabledAlts )
                {
                    string tokenName =
                        probe.GetTokenNameForTokensRuleAlt( (int)altI );
                    // reset the line/col to the token definition (pick last one)
                    NFAState ruleStart =
                        probe.Dfa.Nfa.Grammar.GetRuleStartState( tokenName );
                    line = ruleStart.associatedASTNode.Line;
                    charPositionInLine = ruleStart.associatedASTNode.CharPositionInLine;
                    st.SetAttribute( "disabled", tokenName );
                }
            }
            else
            {
                st.SetAttribute( "disabled", probe.GetDisabledAlternatives( problemState ) );
            }

            var nondetAlts = probe.GetNonDeterministicAltsForState( problemState );
            NFAState nfaStart = probe.Dfa.NFADecisionStartState;
            // all state paths have to begin with same NFA state
            int firstAlt = 0;
            if ( nondetAlts != null )
            {
                foreach ( int displayAltI in nondetAlts )
                {
                    if ( DecisionProbe.verbose )
                    {
                        int tracePathAlt =
                            nfaStart.TranslateDisplayAltToWalkAlt( (int)displayAltI );
                        if ( firstAlt == 0 )
                        {
                            firstAlt = tracePathAlt;
                        }
                        IList<NFAState> path =
                            probe.GetNFAPathStatesForAlt( firstAlt,
                                                         tracePathAlt,
                                                         labels );
                        st.SetAttribute( "paths.{alt,states}",
                                        displayAltI, path );
                    }
                    else
                    {
                        if ( probe.Dfa.IsTokensRuleDecision )
                        {
                            // alts are token rules, convert to the names instead of numbers
                            string tokenName =
                                probe.GetTokenNameForTokensRuleAlt( (int)displayAltI );
                            st.SetAttribute( "conflictingTokens", tokenName );
                        }
                        else
                        {
                            st.SetAttribute( "conflictingAlts", displayAltI );
                        }
                    }
                }
            }
            st.SetAttribute( "hasPredicateBlockedByAction", problemState.Dfa.HasPredicateBlockedByAction );
            return base.ToString( st );
        }

    }
}
