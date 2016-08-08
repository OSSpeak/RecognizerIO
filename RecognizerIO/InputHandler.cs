using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognizerIO
{
    class InputHandler
    {
        public void ProcessIncomingInput(string input)
        {
            var splitInput = input.Split(' ');
            switch (splitInput[0])
            {
                case "init":
                    var loader = new CommandLoader();
                    var speechEngine = new Engines.WSREngine();
                    speechEngine.BuildGrammarFromCommands(loader);
                    speechEngine.Begin();
                    break;
                case "load_command_module":
                    Console.WriteLine("gf");
                    break;
            }
        }
    }
}
