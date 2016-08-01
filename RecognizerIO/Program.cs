using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;
using TextToSRG;

namespace RecognizerIO
{
    class Program
    {
        static void Main(string[] args)
        {
            var loader = new CommandLoader();
            var speechEngine = new Engines.WSREngine();
            speechEngine.BuildGrammarFromCommands(loader);
            speechEngine.Begin();
            Console.ReadLine();
        }
    }
}
