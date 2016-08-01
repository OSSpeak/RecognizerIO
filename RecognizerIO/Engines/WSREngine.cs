using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using TextToSRG;
using System.Threading.Tasks;

namespace RecognizerIO.Engines
{
    class WSREngine
    {
        public SpeechRecognitionEngine Engine;
        public Grammar RootGrammar;
        public Choices RootChoices;
        public GrammarBuilder RootBuilder;
        private CommandLoader CmdLoader;
        private Dictionary<string, SemanticResultValue> ResultMapping;

        public WSREngine()
        {
            RootChoices = new Choices();
            Engine = new SpeechRecognitionEngine();
            Engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            Engine.SetInputToDefaultAudioDevice();
        }

        void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result == null || e.Result.Confidence <= .9) return;
            Console.WriteLine(e.Result.Text);
            Console.WriteLine(e.Result.Words.ToArray()[0].Text);
        }

        public void BuildGrammarFromCommands(CommandLoader loader)
        {
            CmdLoader = loader;
            CmdLoader.LoadModules();
            foreach (var module in CmdLoader.CmdModules)
            {
                foreach (var cmd in module.Commands)
                {
                    string pattern = cmd.Key;
                    string action = cmd.Value;
                    ExpressionNode rootAST = Api.TextToAST(pattern);
                    GrammarBuilder gb = rootAST.ToGrammarBuilder();
                    var guid = new Guid().ToString();
                    var resultKey = new SemanticResultKey(guid, gb);
                    RootChoices.Add(resultKey);

                }
            }
            RootBuilder = new GrammarBuilder(RootChoices, 1, 99);
            RootGrammar = new Grammar(RootBuilder);
            Engine.LoadGrammar(RootGrammar);
        }

        public void Begin()
        {
            Engine.RecognizeAsync(RecognizeMode.Multiple);
        }

    }
}
