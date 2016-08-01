using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;

namespace TextToSRG
{
    public class Parser
    {
        
        private TokenStream TokStream;
        private List<ExpressionNode> ExpressionStack;

        public Parser()
        {
            
            //this.Input = input;
        }
        public ExpressionNode ParseTopLevel(string input)
        {
            var topLevelNode = new ExpressionNode();
            ExpressionStack = new List<ExpressionNode>() { topLevelNode };
            var inputStream = new InputStream(input);
            TokStream = new TokenStream(inputStream);

            while (!TokStream.Eof())
            {
                Token t = TokStream.Next();
                ParseToken(t);
            }
            return topLevelNode;
        }
        void ParseToken(Token tok)
        {
            if (tok.GetType() == typeof(WordToken))
            {
                ParseAtom(tok);
            }
            else if (tok.GetType() == typeof(ParenToken))
            {
                ParseParen((tok as ParenToken).IsClose);
            }
        }
        void ParseAtom(Token tok)
        {
             //ASTNode atom;
            switch (tok.GetType().ToString())
            {
                case "TextToSRG.WordToken":
                    Console.WriteLine((tok as WordToken).Text);
                    var atom = new WordNode((tok as WordToken).Text);
                    ExpressionStack.Last().ChildNodes.Add(atom);
                    return;
                case "TextToSRG.OrToken":
                    ExpressionStack.Last().ChildNodes.Add(new OrNode());
                    return;
            }
        }
        void ParseParen(bool isClose)
        {
            Console.WriteLine(isClose);
            if (!isClose)
            {
                var expr = new ExpressionNode();
                ExpressionStack.Last().ChildNodes.Add(expr);
                ExpressionStack.Add(expr);
                return;
            }
            ExpressionStack.RemoveAt(ExpressionStack.Count - 1);
        }
    }
}
