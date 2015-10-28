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
    using Grammar = Antlr3.Tool.Grammar;
    using NFAFactory = Antlr3.Tool.NFAFactory;

    /** An NFA (collection of NFAStates) constructed from a grammar.  This
     *  NFA is one big machine for entire grammar.  Decision points are recorded
     *  by the Grammar object so we can, for example, convert to DFA or simulate
     *  the NFA (interpret a decision).
     */
    public class NFA
    {
        public const int INVALID_ALT_NUMBER = -1;

        /** This NFA represents which grammar? */
        private readonly Grammar _grammar;

        /** Which factory created this NFA? */
        private readonly NFAFactory _factory;

        private bool _complete;

        public NFA( Grammar grammar )
        {
            this._grammar = grammar;
            this._factory = new NFAFactory(this);
        }

        public Grammar Grammar
        {
            get
            {
                return _grammar;
            }
        }

        public NFAFactory Factory
        {
            get
            {
                return _factory;
            }
        }

        public bool Complete
        {
            get
            {
                return _complete;
            }

            set
            {
                _complete = value;
            }
        }

        public int GetNewNFAStateNumber()
        {
            return Grammar.composite.GetNewNFAStateNumber();
        }

        public void AddState( NFAState state )
        {
            Grammar.composite.AddState( state );
        }

        public NFAState GetState( int s )
        {
            return Grammar.composite.GetState( s );
        }
    }
}
