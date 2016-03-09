﻿/*
 * [The "BSD licence"]
 * Copyright (c) 2005-2008 Terence Parr
 * All rights reserved.
 *
 * Conversion to C#:
 * Copyright (c) 2008-2009 Sam Harwell, Pixel Mine, Inc.
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

#if BUILD_SPELUNKER
    using Console = System.Console;
#endif
    using CLSCompliant = System.CLSCompliantAttribute;
    using Exception = System.Exception;
    using Path = System.IO.Path;
    using StringBuilder = System.Text.StringBuilder;
    using TextReader = System.IO.TextReader;

    /** Load a grammar file and scan it just until we learn a few items
     *  of interest.  Currently: name, type, imports, tokenVocab, language option.
     *
     *  GrammarScanner (at bottom of this class) converts grammar to stuff like:
     *
     *   grammar Java ; options { backtrack true memoize true }
     *   import JavaDecl JavaAnnotations JavaExpr ;
     *   ... : ...
     *
     *  First ':' or '@' indicates we can stop looking for imports/options.
     *
     *  Then we just grab interesting grammar properties.
     */
    public class GrammarSpelunker
    {
        protected string grammarFileName;
        protected string token;
        [CLSCompliant(false)]
        protected Scanner scanner;

        // grammar info / properties
        protected string grammarModifier;
        protected string grammarName;
        protected string tokenVocab;
        protected string language = "Java"; // default
        protected string inputDirectory;
        protected List<string> importedGrammars;

        public GrammarSpelunker( string inputDirectory, string grammarFileName )
        {
            this.inputDirectory = inputDirectory;
            this.grammarFileName = grammarFileName;
        }

        void Consume()
        {
            token = scanner.NextToken();
        }

        protected virtual void Match( string expecting )
        {
            //System.Console.Out.WriteLine( "match " + expecting + "; is " + token );
            if ( token.Equals( expecting ) )
                Consume();
            else
                throw new Exception( "Error parsing " + grammarFileName + ": '" + token +
                                "' not expected '" + expecting + "'" );
        }

        public virtual void Parse()
        {
            string fileName = grammarFileName;
            if ( inputDirectory != null )
                fileName = Path.Combine( inputDirectory, grammarFileName );

            TextReader r = new System.IO.StreamReader( fileName );
            try
            {
                scanner = new Scanner( r );
                Consume();
                GrammarHeader();
                // scan until imports or options
                while ( token != null && !token.Equals( "@" ) && !token.Equals( ":" ) &&
                        !token.Equals( "import" ) && !token.Equals( "options" ) )
                {
                    Consume();
                }
                if ( token.Equals( "options" ) )
                    Options();
                // scan until options or first rule
                while ( token != null && !token.Equals( "@" ) && !token.Equals( ":" ) &&
                        !token.Equals( "import" ) )
                {
                    Consume();
                }
                if ( token.Equals( "import" ) )
                    Imports();
                // ignore rest of input; close up shop
            }
            finally
            {
                if ( r != null )
                    r.Close();
            }
        }

        protected virtual void GrammarHeader()
        {
            if ( token == null )
                return;
            if ( token.Equals( "tree" ) || token.Equals( "parser" ) || token.Equals( "lexer" ) )
            {
                grammarModifier = token;
                Consume();
            }
            Match( "grammar" );
            grammarName = token;
            Consume(); // move beyond name
        }

        // looks like "options { backtrack true ; tokenVocab MyTokens ; }"
        protected virtual void Options()
        {
            Match( "options" );
            Match( "{" );
            while ( token != null && !token.Equals( "}" ) )
            {
                string name = token;
                Consume();
                string value = token;
                Consume();
                Match( ";" );
                if ( name.Equals( "tokenVocab" ) )
                    tokenVocab = value;
                if ( name.Equals( "language" ) )
                    language = value;
            }
            Match( "}" );
        }

        // looks like "import JavaDecl JavaAnnotations JavaExpr ;"
        protected virtual void Imports()
        {
            Match( "import" );
            importedGrammars = new List<string>();
            while ( token != null && !token.Equals( ";" ) )
            {
                importedGrammars.Add( token );
                Consume();
            }
            Match( ";" );
            if ( importedGrammars.Count == 0 )
                importedGrammars = null;
        }

        [CLSCompliant(false)]
        public virtual string GrammarModifier
        {
            get
            {
                return grammarModifier;
            }
        }
        [CLSCompliant(false)]
        public virtual string GrammarName
        {
            get
            {
                return grammarName;
            }
        }
        [CLSCompliant(false)]
        public virtual string TokenVocab
        {
            get
            {
                return tokenVocab;
            }
        }
        [CLSCompliant(false)]
        public virtual string Language
        {
            get
            {
                return language;
            }
        }
        [CLSCompliant(false)]
        public virtual List<string> ImportedGrammars
        {
            get
            {
                return importedGrammars;
            }
        }

        /** Strip comments and then return stream of words and
         *  tokens {';', ':', '{', '}'}
         */
        public class Scanner
        {
            public const int EOF = -1;
            TextReader input;
            int c;

            public Scanner( TextReader input )
            {
                this.input = input;
                Consume();
            }

            bool IsDigit()
            {
                return c >= '0' && c <= '9';
            }
            bool IsIdStart()
            {
                return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
            }
            bool IsIdLetter()
            {
                return IsIdStart() || c >= '0' && c <= '9' || c == '_';
            }

            void Consume()
            {
                c = input.Read();
            }

            public virtual string NextToken()
            {
                while ( c != EOF )
                {
                    //System.Console.Out.WriteLine( "check " + (char)c );
                    switch ( c )
                    {
                    case ';':
                        Consume();
                        return ";";
                    case '{':
                        Consume();
                        return "{";
                    case '}':
                        Consume();
                        return "}";
                    case ':':
                        Consume();
                        return ":";
                    case '@':
                        Consume();
                        return "@";
                    case '/':
                        Comment();
                        break;
                    case '\'':
                        return String();
                    default:
                        if ( IsIdStart() )
                            return Id();
                        else if ( IsDigit() )
                            return Int();
                        Consume(); // ignore anything else
                        break;
                    }
                }
                return null;
            }

            /** NAME : LETTER+ ; // NAME is sequence of >=1 letter */
            string Id()
            {
                StringBuilder buf = new StringBuilder();
                while ( c != EOF && IsIdLetter() )
                {
                    buf.Append( (char)c );
                    Consume();
                }
                return buf.ToString();
            }

            string Int()
            {
                StringBuilder buf = new StringBuilder();
                while ( c != EOF && IsDigit() )
                {
                    buf.Append( (char)c );
                    Consume();
                }
                return buf.ToString();
            }

            string String()
            {
                StringBuilder buf = new StringBuilder();
                Consume();
                while ( c != EOF && c != '\'' )
                {
                    if ( c == '\\' )
                    {
                        buf.Append( (char)c );
                        Consume();
                    }
                    buf.Append( (char)c );
                    Consume();
                }
                Consume(); // scan past '
                return buf.ToString();
            }

            void Comment()
            {
                if ( c == '/' )
                {
                    Consume();
                    if ( c == '*' )
                    {
                        Consume();
                        for ( ; ; )
                        {
                            if ( c == '*' )
                            {
                                Consume();
                                if ( c == '/' )
                                {
                                    Consume();
                                    break;
                                }
                            }
                            else
                            {
                                while ( c != EOF && c != '*' )
                                    Consume();
                            }
                        }
                    }
                    else if ( c == '/' )
                    {
                        while ( c != EOF && c != '\n' )
                            Consume();
                    }
                }
            }
        }

#if BUILD_SPELUNKER
        /** Tester; Give grammar filename as arg */
        public static void Main( string[] args )
        {
            GrammarSpelunker g = new GrammarSpelunker( ".", args[0] );
            g.parse();
            Console.Out.WriteLine( g.grammarModifier + " grammar " + g.grammarName );
            Console.Out.WriteLine( "language=" + g.language );
            Console.Out.WriteLine( "tokenVocab=" + g.tokenVocab );
            Console.Out.WriteLine( "imports=" + g.importedGrammars );
        }
#endif
    }
}
