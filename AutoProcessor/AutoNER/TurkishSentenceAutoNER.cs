using System.Globalization;
using Dictionary.Dictionary;
using MorphologicalAnalysis;

namespace AnnotatedSentence.AutoProcessor.AutoNER
{
    public class TurkishSentenceAutoNER : SentenceAutoNER
    {
        /**
         * <summary> The method assigns the words "bay" and "bayan" PERSON tag. The method also checks the PERSON gazetteer, and if
         * the word exists in the gazetteer, it assigns PERSON tag.</summary>
         * <param name="sentence">The sentence for which PERSON named entities checked.</param>
         */
        protected override void AutoDetectPerson(AnnotatedSentence sentence)
        {
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = (AnnotatedWord) sentence.GetWord(i);
                if (word.GetParse() != null)
                {
                    if (Word.IsHonorific(word.GetName()))
                    {
                        word.SetNamedEntityType("PERSON");
                    }

                    word.CheckGazetteer(personGazetteer);
                }
            }
        }

        /**
         * <summary> The method checks the LOCATION gazetteer, and if the word exists in the gazetteer, it assigns the LOCATION tag.</summary>
         * <param name="sentence">The sentence for which LOCATION named entities checked.</param>
         */
        protected override void AutoDetectLocation(AnnotatedSentence sentence)
        {
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = (AnnotatedWord) sentence.GetWord(i);
                if (word.GetParse() != null)
                {
                    word.CheckGazetteer(locationGazetteer);
                }
            }
        }

        /**
         * <summary> The method assigns the words "corp.", "inc.", and "co" ORGANIZATION tag. The method also checks the
         * ORGANIZATION gazetteer, and if the word exists in the gazetteer, it assigns ORGANIZATION tag.</summary>
         * <param name="sentence">The sentence for which ORGANIZATION named entities checked.</param>
         */
        protected override void AutoDetectOrganization(AnnotatedSentence sentence)
        {
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = (AnnotatedWord) sentence.GetWord(i);
                if (word.GetParse() != null)
                {
                    if (Word.IsOrganization(word.GetName()))
                    {
                        word.SetNamedEntityType("ORGANIZATION");
                    }

                    word.CheckGazetteer(organizationGazetteer);
                }
            }
        }

        /**
         * <summary> The method checks for the TIME entities using regular expressions. After that, if the expression is a TIME
         * expression, it also assigns the previous texts, which are numbers, TIME tag.</summary>
         * <param name="sentence">The sentence for which TIME named entities checked.</param>
         */
        protected override void AutoDetectTime(AnnotatedSentence sentence)
        {
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = (AnnotatedWord) sentence.GetWord(i);
                var wordLowercase = word.GetName().ToLower(new CultureInfo("tr"));
                if (word.GetParse() != null)
                {
                    if (Word.IsTime(wordLowercase))
                    {
                        word.SetNamedEntityType("TIME");
                        if (i > 0)
                        {
                            AnnotatedWord previous = (AnnotatedWord) sentence.GetWord(i - 1);
                            if (previous.GetParse().ContainsTag(MorphologicalTag.CARDINAL))
                            {
                                previous.SetNamedEntityType("TIME");
                            }
                        }
                    }
                }
            }
        }


        /**
         * <summary> The method checks for the MONEY entities using regular expressions. After that, if the expression is a MONEY
         * expression, it also assigns the previous text, which may included numbers or some monetarial texts, MONEY tag.</summary>
         * <param name="sentence">The sentence for which MONEY named entities checked.</param>
         */
        protected override void AutoDetectMoney(AnnotatedSentence sentence)
        {
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = (AnnotatedWord) sentence.GetWord(i);
                var wordLowercase = word.GetName().ToLower(new CultureInfo("tr"));
                if (word.GetParse() != null)
                {
                    if (Word.IsMoney(wordLowercase))
                    {
                        word.SetNamedEntityType("MONEY");
                        var j = i - 1;
                        while (j >= 0)
                        {
                            AnnotatedWord previous = (AnnotatedWord) sentence.GetWord(j);
                            if (previous.GetParse() != null && (previous.GetName().Equals("amerikan") ||
                                                                previous.GetParse()
                                                                    .ContainsTag(MorphologicalTag.REAL) ||
                                                                previous.GetParse()
                                                                    .ContainsTag(MorphologicalTag.CARDINAL) ||
                                                                previous.GetParse()
                                                                    .ContainsTag(MorphologicalTag.NUMBER)))
                            {
                                previous.SetNamedEntityType("MONEY");
                            }
                            else
                            {
                                break;
                            }

                            j--;
                        }
                    }
                }
            }
        }
    }
}