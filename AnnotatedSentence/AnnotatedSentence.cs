using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Corpus;
using Dictionary.Dictionary;
using MorphologicalAnalysis;
using PropBank;
using WordNet;

namespace AnnotatedSentence
{
    public class AnnotatedSentence : Sentence
    {
        private readonly FileInfo _file;

        /**
         * <summary> Reads an annotated sentence from a text file.</summary>
         * <param name="file">File containing the annotated sentence.</param>
         */
        public AnnotatedSentence(FileInfo file)
        {
            this._file = file;
            words = new List<Word>();
            var streamReader = new StreamReader(file.FullName);
            while (!streamReader.EndOfStream)
            {
                string[] line = streamReader.ReadLine().Split(" ");
                foreach (var word in line)
                {
                    words.Add(new AnnotatedWord(word));
                }
            }

            streamReader.Close();
        }

        /**
         * <summary> Converts a simple sentence to an annotated sentence</summary>
         * <param name="sentence">Simple sentence</param>
         */
        public AnnotatedSentence(string sentence)
        {
            words = new List<Word>();
            var wordArray = sentence.Split(" ");
            foreach (var word in wordArray)
            {
                if (word != "")
                {
                    words.Add(new AnnotatedWord(word));
                }
            }
        }

        /**
         * <summary> The method constructs all possible shallow parse groups of a sentence.</summary>
         * <returns>Shallow parse groups of a sentence.</returns>
         */
        public List<AnnotatedPhrase> GetShallowParseGroups()
        {
            List<AnnotatedPhrase> shallowParseGroups = new List<AnnotatedPhrase>();
            AnnotatedWord previousWord = null;
            AnnotatedPhrase current = null;
            for (var i = 0; i < WordCount(); i++)
            {
                var annotatedWord = (AnnotatedWord) GetWord(i);
                if (previousWord == null)
                {
                    current = new AnnotatedPhrase(i, annotatedWord.GetShallowParse());
                }
                else
                {
                    if (previousWord.GetShallowParse() != null &&
                        !previousWord.GetShallowParse().Equals(annotatedWord.GetShallowParse()))
                    {
                        shallowParseGroups.Add(current);
                        current = new AnnotatedPhrase(i, annotatedWord.GetShallowParse());
                    }
                }

                current.AddWord(annotatedWord);
                previousWord = annotatedWord;
            }

            shallowParseGroups.Add(current);
            return shallowParseGroups;
        }

        /**
         * <summary> The method checks all words in the sentence and returns true if at least one of the words is annotated with
         * PREDICATE tag.</summary>
         * <returns>True if at least one of the words is annotated with PREDICATE tag; false otherwise.</returns>
         */
        public bool ContainsPredicate()
        {
            foreach (var word in words)
            {
                var annotatedWord = (AnnotatedWord) word;
                if (annotatedWord.GetArgument() != null &&
                    annotatedWord.GetArgument().GetArgumentType().Equals("PREDICATE"))
                {
                    return true;
                }
            }

            return false;
        }

        public bool UpdateConnectedPredicate(string previousId, string currentId)
        {
            var modified = false;
            foreach (var word in words)
            {
                var annotatedWord = (AnnotatedWord) word;
                if (annotatedWord.GetArgument() != null && annotatedWord.GetArgument().GetId() != null &&
                    annotatedWord.GetArgument().GetId().Equals(previousId))
                {
                    annotatedWord.SetArgument(annotatedWord.GetArgument().GetArgumentType() + "$" + currentId);
                    modified = true;
                }
            }

            return modified;
        }

        /**
         * <summary> The method returns all possible words, which is
         * 1. Verb
         * 2. Its semantic tag is assigned
         * 3. A frameset exists for that semantic tag</summary>
         * <param name="framesetList">Frameset list that contains all frames for Turkish</param>
         * <returns>An array of words, which are verbs, semantic tags assigned, and framesetlist assigned for that tag.</returns>
         */
        public List<AnnotatedWord> PredicateCandidates(FramesetList framesetList)
        {
            var candidateList = new List<AnnotatedWord>();
            foreach (var word in words)
            {
                var annotatedWord = (AnnotatedWord) word;
                if (annotatedWord.GetParse() != null && annotatedWord.GetParse().IsVerb() &&
                    annotatedWord.GetSemantic() != null && framesetList.FrameExists(annotatedWord.GetSemantic()))
                {
                    candidateList.Add(annotatedWord);
                }
            }

            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < words.Count - i - 1; j++)
                {
                    var annotatedWord = (AnnotatedWord) words[j];
                    var nextAnnotatedWord = (AnnotatedWord) words[j + 1];
                    if (!candidateList.Contains(annotatedWord) && candidateList.Contains(nextAnnotatedWord) &&
                        annotatedWord.GetSemantic() != null &&
                        annotatedWord.GetSemantic().Equals(nextAnnotatedWord.GetSemantic()))
                    {
                        candidateList.Add(annotatedWord);
                    }
                }
            }

            return candidateList;
        }

        /**
         * <summary> Returns the i'th predicate in the sentence.</summary>
         * <param name="index">Predicate index</param>
         * <returns>The predicate with index index in the sentence.</returns>
         */
        public string GetPredicate(int index)
        {
            int count1 = 0, count2 = 0;
            var data = "";
            var word = new List<AnnotatedWord>();
            var parse = new List<MorphologicalParse>();
            if (index < WordCount())
            {
                for (var i = 0; i < WordCount(); i++)
                {
                    word.Add((AnnotatedWord) GetWord(i));
                    parse.Add(word[i].GetParse());
                }

                for (var i = index; i >= 0; i--)
                {
                    if (parse[i] != null && parse[i].GetRootPos() != null && parse[i].GetPos() != null &&
                        parse[i].GetRootPos().Equals("VERB") && parse[i].GetPos().Equals("VERB"))
                    {
                        count1 = index - i;
                        break;
                    }
                }

                for (var i = index; i < WordCount() - index; i++)
                {
                    if (parse[i] != null && parse[i].GetRootPos() != null && parse[i].GetPos() != null &&
                        parse[i].GetRootPos().Equals("VERB") && parse[i].GetPos().Equals("VERB"))
                    {
                        count2 = i - index;
                        break;
                    }
                }

                if (count1 > count2)
                {
                    data = word[count1].GetName();
                }
                else
                {
                    data = word[count2].GetName();
                }
            }

            return data;
        }

        /**
         * <summary> Returns file name of the sentence</summary>
         * <returns>File name of the sentence</returns>
         */
        public string GetFileName()
        {
            return _file.FullName;
        }

        /**
         * <summary> Removes the i'th word from the sentence</summary>
         * <param name="index">Word index</param>
         */
        public void RemoveWord(int index)
        {
            words.RemoveAt(index);
        }


        /**
         * <summary>The toStems method returns an accumulated string of each word's stems in words {@link ArrayList}.
         * If the parse of the word does not exist, the method adds the surfaceform to the resulting string.</summary>
         *
         * <returns>String result which has all the stems of each item in words {@link ArrayList}.</returns>
         */
        public string ToStems()
        {
            if (words.Count > 0)
            {
                var annotatedWord = (AnnotatedWord) words[0];
                string result;
                if (annotatedWord.GetParse() != null)
                {
                    result = annotatedWord.GetParse().GetWord().GetName();
                }
                else
                {
                    result = annotatedWord.GetName();
                }

                for (var i = 1; i < words.Count; i++)
                {
                    annotatedWord = (AnnotatedWord) words[i];
                    if (annotatedWord.GetParse() != null)
                    {
                        result = result + " " + annotatedWord.GetParse().GetWord().GetName();
                    }
                    else
                    {
                        result = result + " " + annotatedWord.GetName();
                    }
                }

                return result;
            }

            return "";
        }

        /**
         * <summary> Saves the current sentence.</summary>
         */
        public void Save()
        {
            WriteToFile(new StreamWriter(_file.FullName));
        }

        /**
         * <summary> Creates a list of literal candidates for the i'th word in the sentence. It combines the results of
         * 1. All possible root forms of the i'th word in the sentence
         * 2. All possible 2-word expressions containing the i'th word in the sentence
         * 3. All possible 3-word expressions containing the i'th word in the sentence</summary>
         * <param name="wordNet">Turkish wordnet</param>
         * <param name="fsm">Turkish morphological analyzer</param>
         * <param name="wordIndex">Word index</param>
         * <returns>List of literal candidates containing all possible root forms and multi-word expressions.</returns>
         */
        public List<Literal> ConstructLiterals(WordNet.WordNet wordNet, FsmMorphologicalAnalyzer fsm, int wordIndex)
        {
            var word = (AnnotatedWord) GetWord(wordIndex);
            var possibleLiterals = new List<Literal>();
            var morphologicalParse = word.GetParse();
            var metamorphicParse = word.GetMetamorphicParse();
            possibleLiterals = possibleLiterals.Union(wordNet.ConstructLiterals(morphologicalParse.GetWord().GetName(),
                morphologicalParse, metamorphicParse, fsm)).ToList();
            AnnotatedWord firstSucceedingWord = null;
            AnnotatedWord secondSucceedingWord = null;
            if (WordCount() > wordIndex + 1)
            {
                firstSucceedingWord = (AnnotatedWord) GetWord(wordIndex + 1);
                if (WordCount() > wordIndex + 2)
                {
                    secondSucceedingWord = (AnnotatedWord) GetWord(wordIndex + 2);
                }
            }

            if (firstSucceedingWord != null)
            {
                if (secondSucceedingWord != null)
                {
                    possibleLiterals = possibleLiterals.Union(wordNet.ConstructIdiomLiterals(word.GetParse(),
                            firstSucceedingWord.GetParse(), secondSucceedingWord.GetParse(), word.GetMetamorphicParse(),
                            firstSucceedingWord.GetMetamorphicParse(), secondSucceedingWord.GetMetamorphicParse(), fsm))
                        .ToList();
                }

                possibleLiterals = possibleLiterals.Union(wordNet.ConstructIdiomLiterals(word.GetParse(),
                    firstSucceedingWord.GetParse(),
                    word.GetMetamorphicParse(), firstSucceedingWord.GetMetamorphicParse(), fsm)).ToList();
            }

            possibleLiterals.Sort(new LiteralWithSenseComparator(new CultureInfo("tr")));
            return possibleLiterals;
        }

        /**
         * <summary> Creates a list of synset candidates for the i'th word in the sentence. It combines the results of
         * 1. All possible synsets containing the i'th word in the sentence
         * 2. All possible synsets containing 2-word expressions, which contains the i'th word in the sentence
         * 3. All possible synsets containing 3-word expressions, which contains the i'th word in the sentence</summary>
         * <param name="wordNet">Turkish wordnet</param>
         * <param name="fsm">Turkish morphological analyzer</param>
         * <param name="wordIndex">Word index</param>
         * <returns>List of synset candidates containing all possible root forms and multi-word expressions.</returns>
         */
        public List<SynSet> ConstructSynSets(WordNet.WordNet wordNet, FsmMorphologicalAnalyzer fsm, int wordIndex)
        {
            var word = (AnnotatedWord) GetWord(wordIndex);
            var possibleSynSets = new List<SynSet>();

            var morphologicalParse = word.GetParse();

            var metamorphicParse = word.GetMetamorphicParse();
            possibleSynSets = possibleSynSets.Union(wordNet.ConstructSynSets(morphologicalParse.GetWord(
            ).GetName(), morphologicalParse, metamorphicParse, fsm)).ToList();
            AnnotatedWord firstPrecedingWord = null;
            AnnotatedWord secondPrecedingWord = null;
            AnnotatedWord firstSucceedingWord = null;

            AnnotatedWord secondSucceedingWord = null;
            if (wordIndex > 0)
            {
                firstPrecedingWord = (AnnotatedWord) GetWord(wordIndex - 1);

                if (wordIndex > 1)
                {
                    secondPrecedingWord = (AnnotatedWord) GetWord(wordIndex - 2);
                }
            }

            if (WordCount() > wordIndex + 1)
            {
                firstSucceedingWord = (AnnotatedWord) GetWord(wordIndex + 1);

                if (WordCount() > wordIndex + 2)
                {
                    secondSucceedingWord = (AnnotatedWord) GetWord(wordIndex + 2);
                }
            }

            if (firstPrecedingWord != null)
            {
                if (secondPrecedingWord != null)
                {
                    possibleSynSets = possibleSynSets.Union(wordNet.ConstructIdiomSynSets(
                        secondPrecedingWord.GetParse(),
                        firstPrecedingWord.GetParse(), word.GetParse(), secondPrecedingWord.GetMetamorphicParse(),
                        firstPrecedingWord.GetMetamorphicParse(), word.GetMetamorphicParse(), fsm)).ToList();
                }

                possibleSynSets = possibleSynSets.Union(wordNet.ConstructIdiomSynSets(firstPrecedingWord.GetParse(),
                    word.GetParse(),
                    firstPrecedingWord.GetMetamorphicParse(), word.GetMetamorphicParse(), fsm)).ToList();
            }

            if (firstPrecedingWord != null && firstSucceedingWord != null)
            {
                possibleSynSets = possibleSynSets.Union(wordNet.ConstructIdiomSynSets(firstPrecedingWord.GetParse(),
                    word.GetParse(),
                    firstSucceedingWord.GetParse(), firstPrecedingWord.GetMetamorphicParse(),
                    word.GetMetamorphicParse(),
                    firstSucceedingWord.GetMetamorphicParse(), fsm)).ToList();
            }

            if (firstSucceedingWord != null)
            {
                if (secondSucceedingWord != null)
                {
                    possibleSynSets = possibleSynSets.Union(wordNet.ConstructIdiomSynSets(word.GetParse(),
                            firstSucceedingWord.GetParse(),
                            secondSucceedingWord.GetParse(), word.GetMetamorphicParse(),
                            firstSucceedingWord.GetMetamorphicParse(), secondSucceedingWord.GetMetamorphicParse(), fsm))
                        .ToList();
                }

                possibleSynSets = possibleSynSets.Union(wordNet.ConstructIdiomSynSets(word.GetParse(),
                    firstSucceedingWord.GetParse(),
                    word.GetMetamorphicParse(), firstSucceedingWord.GetMetamorphicParse(), fsm)).ToList();
            }

            possibleSynSets.Sort(new SynSetComparator());
            return possibleSynSets;
        }
    }
}