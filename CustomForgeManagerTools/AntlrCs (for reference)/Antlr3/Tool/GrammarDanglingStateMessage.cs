/*
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

namespace Antlr3.Tool
{
    using System.Collections.Generic;
    using Antlr3.Extensions;

    using DecisionProbe = Antlr3.Analysis.DecisionProbe;
    using DFAState = Antlr3.Analysis.DFAState;
    using StringTemplate = Antlr4.StringTemplate.Template;

    /** Reports a potential parsing issue with a decision; the decision is
     *  nondeterministic in some way.
     */
    public class GrammarDanglingStateMessage : Message
    {
        public DecisionProbe probe;
        public DFAState problemState;

        public GrammarDanglingStateMessage( DecisionProbe probe,
                                           DFAState problemState )
            : base( ErrorManager.MSG_DANGLING_STATE )
        {
            this.probe = probe;
            this.problemState = problemState;
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
            var labels = probe.GetSampleNonDeterministicInputSequence( problemState );
            string input = probe.GetInputSequenceDisplay( labels );
            StringTemplate st = GetMessageTemplate();
            List<int> alts = new List<int>();
            alts.AddRange( problemState.AltSet );
            alts.Sort();
            st.SetAttribute( "danglingAlts", alts );
            st.SetAttribute( "input", input );

            return base.ToString( st );
        }

    }
}
