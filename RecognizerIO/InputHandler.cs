using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognizerIO
{
    class InputHandler { 

        public Engines.EngineManager EngManager { get; set; }


        public InputHandler()
        {
            EngManager = new Engines.EngineManager();
            //EngManager.LoadGrammar(@"C:\Users\evan\AppData\Local\Temp\8da0e5d7-f8ea-4b87-bb58-6aff4c735606.xml");
        }

        public void ProcessIncomingInput(string input)
        {
            var splitInput = input.Split(' ');
            switch (splitInput[0])
            {
                case "grammar_content":
                    string xml = String.Join(" ", splitInput.Skip(1).ToArray());
                    string tmpPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".xml";
                    //tmpPath = @"C:\Users\evan\AppData\Local\Temp\6cc971c3-b0df-45f6-b409-e0ce74ebf545.xml";
                    System.IO.File.WriteAllText(tmpPath, xml);
                    EngManager.LoadGrammar(tmpPath);
                    EngManager.Begin();
                    break;
            }
        }
    }
}
