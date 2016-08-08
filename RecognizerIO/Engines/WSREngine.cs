﻿using System;
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
        private Dictionary<string, string> ResultMapping;

        public WSREngine()
        {
            RootChoices = new Choices();
            ResultMapping = new Dictionary<string, string>();
            Engine = new SpeechRecognitionEngine();
            Engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            Engine.SetInputToDefaultAudioDevice();
        }

        void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result == null || e.Result.Confidence <= .9) return;
            var patternKey = e.Result.Semantics.ToArray()[0].Key;
            string action = ResultMapping[patternKey];
            Console.WriteLine(action);
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
                    ExpressionNode rootAST = Api.TextToAST(pattern);
                    GrammarBuilder gb = rootAST.ToGrammarBuilder();
                    var guid = Guid.NewGuid().ToString();
                    ResultMapping[guid] = cmd.Value;
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
