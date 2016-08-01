using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToSRG
{
    class TokenStream
    {

        private InputStream Input;
        private int Pos;
        private Token Current;
        private Regex WordRegex;

        public TokenStream(InputStream input)
        {
            this.Input = input;
            this.Pos = 0;
            this.Current = null;
            WordRegex = new Regex(@"[a-zA-Z]");
        }

        private string ReadWhile(Func<string, bool> predicate)
        {
            var str = "";
            while (!Input.Eof() && predicate(Input.Peek()))
                str += Input.Next();
            return str;
        }

        private bool IsWhitespace(string ch)
        {
            return " \t\n".Contains(ch);
        }

        private Token ReadWord()
        {
            var str = ReadWhile((ch) => WordRegex.IsMatch(ch));
            return new WordToken(str);
        }

        private Token ReadParen()
        {
            var ch = Input.Next();
            return new ParenToken(ch == ")");
        }

        private Token ReadOr()
        {
            var ch = Input.Next();
            return new OrToken();
        }

        private Token ReadNext()
        {
            ReadWhile(IsWhitespace);
            if (Input.Eof() == true) return null;
            string ch = Input.Peek();
            if (WordRegex.IsMatch(ch)) return ReadWord();
            if (ch == "(" || ch == ")") return ReadParen();
            if (ch == "|") return ReadOr();
            Croak("Invalid Character: " + ch);
            return null;
        }
        
        public Token Next()
        {
            var tok = Current;
            Current = null;
            return tok != null ? tok : ReadNext();
        }
        public Token Peek()
        {
            if (Current != null) return Current;
            Current = ReadNext();
            return Current;
        }
        public bool Eof()
        {
            return Peek() == null;
        }
        public void Croak(string msg)
        {
            Input.Croak(msg);
        }
     
    }
}
