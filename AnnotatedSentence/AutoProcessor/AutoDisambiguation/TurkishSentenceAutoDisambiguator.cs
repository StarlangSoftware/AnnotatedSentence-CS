using MorphologicalAnalysis;
using MorphologicalDisambiguation;

namespace AnnotatedSentence.AutoProcessor.AutoDisambiguation
{
    public class TurkishSentenceAutoDisambiguator : SentenceAutoDisambiguator
    {
        /**
         * <summary> Constructor for the class.</summary>
         * <param name="rootWordStatistics">The object contains information about the selected correct root words in a corpus for a set
         *                           of possible lemma. For example, the lemma
         *                           `günü': 2 possible root words `gün' and `günü'
         *                           `çağlar' : 2 possible root words `çağ' and `çağlar'</param>
         */
        public TurkishSentenceAutoDisambiguator(RootWordStatistics rootWordStatistics) : base(new FsmMorphologicalAnalyzer(), rootWordStatistics)
        {
        }

        /**
         * <summary> Constructor for the class.</summary>
         * <param name="fsm">               Finite State Machine based morphological analyzer</param>
         * <param name="rootWordStatistics">The object contains information about the selected correct root words in a corpus for a set
         *                           of possible lemma. For example, the lemma
         *                           `günü': 2 possible root words `gün' and `günü'
         *                           `çağlar' : 2 possible root words `çağ' and `çağlar'</param>
         */
        public TurkishSentenceAutoDisambiguator(FsmMorphologicalAnalyzer fsm, RootWordStatistics rootWordStatistics) : base(fsm, rootWordStatistics)
        {
        }

        /**
         * <summary> The method disambiguates the words with a single morphological analysis. Basically the
         * method sets the morphological analysis of the words with one possible morphological analysis. If the word
         * is already morphologically disambiguated, the method does not disambiguate that word.</summary>
         * <param name="sentence">The sentence to be disambiguated automatically.</param>
         */
        protected override void AutoFillSingleAnalysis(AnnotatedSentence sentence)
        {
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = (AnnotatedWord) sentence.GetWord(i);
                if (word.GetParse() == null)
                {
                    var fsmParseList = morphologicalAnalyzer.RobustMorphologicalAnalysis(word.GetName());
                    if (fsmParseList.Size() == 1)
                    {
                        word.SetParse(fsmParseList.GetFsmParse(0).TransitionList());
                        word.SetMetamorphicParse(fsmParseList.GetFsmParse(0).WithList());
                    }
                }
            }
        }

        /**
         * <summary> If the words has only single root in its possible parses, the method disambiguates by looking special cases.
         * The cases are implemented in the caseDisambiguator method.</summary>
         * <param name="fsmParseList">Morphological parses of the word.</param>
         * <param name="word">Word to be disambiguated.</param>
         */
        private void SetParseAutomatically(FsmParseList fsmParseList, AnnotatedWord word)
        {
            if (fsmParseList.Size() > 0 && !fsmParseList.RootWords().Contains("$"))
            {
                var disambiguatedParse = fsmParseList.CaseDisambiguator();
                if (disambiguatedParse != null)
                {
                    word.SetParse(disambiguatedParse.TransitionList());
                    word.SetMetamorphicParse(disambiguatedParse.WithList());
                }
            }
        }

        /**
         * <summary> The method disambiguates words with multiple possible root words in its morphological parses. If the word
         * is already morphologically disambiguated, the method does not disambiguate that word. The method first check
         * for multiple root words by using rootWords method. If there are multiple root words, the method select the most
         * occurring root word (if its occurence wrt other root words occurence is above some threshold) for that word
         * using the bestRootWord method. If root word is selected, then the case for single root word is called.</summary>
         * <param name="sentence">The sentence to be disambiguated automatically.</param>
         */
        protected override void AutoDisambiguateMultipleRootWords(AnnotatedSentence sentence)
        {
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = (AnnotatedWord) sentence.GetWord(i);
                if (word.GetParse() == null)
                {
                    var fsmParseList = morphologicalAnalyzer.RobustMorphologicalAnalysis(word.GetName());
                    if (fsmParseList.RootWords().Contains("$"))
                    {
                        var bestRootWord = rootWordStatistics.BestRootWord(fsmParseList, 0.0);
                        if (bestRootWord != null)
                        {
                            fsmParseList.ReduceToParsesWithSameRoot(bestRootWord);
                        }
                    }

                    SetParseAutomatically(fsmParseList, word);
                }
            }
        }

        /**
         * <summary> The method disambiguates words with single possible root word in its morphological parses. If the word
         * is already morphologically disambiguated, the method does not disambiguate that word. Basically calls
         * setParseAutomatically method.</summary>
         * <param name="sentence">The sentence to be disambiguated automatically.</param>
         */
        protected override void AutoDisambiguateSingleRootWords(AnnotatedSentence sentence)
        {
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = (AnnotatedWord) sentence.GetWord(i);
                if (word.GetParse() == null)
                {
                    var fsmParseList = morphologicalAnalyzer.RobustMorphologicalAnalysis(word.GetName());
                    SetParseAutomatically(fsmParseList, word);
                }
            }
        }
    }
}