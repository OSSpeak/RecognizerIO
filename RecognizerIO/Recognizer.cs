using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Threading;
using TextToSRG;

namespace RecognizerIO
{

    class Recognizer
    {
        public SpeechRecognitionEngine Engine;
        public Grammar RootGrammar;
        public Choices RootChoices;
        public GrammarBuilder RootBuilder;

        public Recognizer()
        {
            Engine = new SpeechRecognitionEngine();
            Engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            Engine.SetInputToDefaultAudioDevice();
            RootChoices = new Choices();
            var y = new List<string>() { "hello world" };
            var foo = new GrammarBuilder("foo");
            var bar = new GrammarBuilder("bar");
            var k = new SemanticResultKey("item", foo);
            RootChoices.Add(k);
            RootChoices.Add(bar);
            RootBuilder = new GrammarBuilder(RootChoices, 1, 99);
            RootGrammar = new Grammar(RootBuilder);
            Engine.LoadGrammar(RootGrammar);
        }

        void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result == null || e.Result.Confidence <= .9) return;
            Console.WriteLine(e.Result.Text);
            Console.WriteLine(e.Result.Words.ToArray()[0].Text);
        }
        public void Begin()
        {
            Engine.RecognizeAsync(RecognizeMode.Multiple);
        }

    }
}
