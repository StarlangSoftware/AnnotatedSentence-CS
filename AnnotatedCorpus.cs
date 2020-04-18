using System;
using System.Collections.Generic;
using System.IO;
using Corpus;
using DataStructure;
using Dictionary.Dictionary;
using MorphologicalAnalysis;
using MorphologicalDisambiguation;

namespace AnnotatedSentence
{
    public class AnnotatedCorpus : Corpus.Corpus
    {
        /**
         * <summary> A constructor of {@link AnnotatedCorpus} class which reads all {@link AnnotatedSentence} files with the file
         * name satisfying the given pattern inside the given folder. For each file inside that folder, the constructor
         * creates an AnnotatedSentence and puts in inside the list parseTrees.</summary>
         * <param name="folder">Folder where all sentences reside.</param>
         * <param name="pattern">File pattern such as "." ".train" ".test".</param>
         */
        public AnnotatedCorpus(string folder, string pattern)
        {
            sentences = new List<Sentence>();
            var listOfFiles = Directory.GetFiles(folder);
            foreach (var file in listOfFiles)
            {
                if (!file.Contains(pattern))
                    continue;
                var sentence = new AnnotatedSentence(new FileInfo(file));
                sentences.Add(sentence);
            }
        }

        /**
         * <summary> A constructor of {@link AnnotatedCorpus} class which reads all {@link AnnotatedSentence} files inside the given
         * folder. For each file inside that folder, the constructor creates an AnnotatedSentence and puts in inside the
         * list sentences.</summary>
         */
        public AnnotatedCorpus(string folder)
        {
            sentences = new List<Sentence>();
            var listOfFiles = Directory.GetFiles(folder);
            foreach (var file in listOfFiles)
            {
                var sentence = new AnnotatedSentence(new FileInfo(file));
                sentences.Add(sentence);
            }
        }

        /**
         * <summary> The method removes all empty words from the sentences.</summary>
         */
        public void ClearNullWords()
        {
            for (var i = 0; i < SentenceCount(); i++)
            {
                var sentence = (AnnotatedSentence) GetSentence(i);
                var changed = false;
                for (var j = 0; j < sentence.WordCount(); j++)
                {
                    if (sentence.GetWord(j).GetName() == null || sentence.GetWord(j).GetName().Length == 0)
                    {
                        sentence.RemoveWord(j);
                        j--;
                        changed = true;
                    }
                }

                if (changed)
                {
                    sentence.Save();
                }
            }
        }

        /**
         * <summary> The method traverses all words in all sentences and prints the words which do not have a morphological analysis.</summary>
         */
        public void CheckMorphologicalAnalysis()
        {
            for (var i = 0; i < SentenceCount(); i++)
            {
                var sentence = (AnnotatedSentence) GetSentence(i);
                for (var j = 0; j < sentence.WordCount(); j++)
                {
                    var word = (AnnotatedWord) sentence.GetWord(j);
                    if (word.GetParse() == null)
                    {
                        Console.WriteLine("Morphological Analysis does not exist for sentence " +
                                          sentence.GetFileName());
                        break;
                    }
                }
            }
        }

        /**
         * <summary> The method traverses all words in all sentences and prints the words which do not have shallow parse annotation.</summary>
         */
        public void CheckShallowParse()
        {
            for (var i = 0; i < SentenceCount(); i++)
            {
                var sentence = (AnnotatedSentence) GetSentence(i);
                for (var j = 0; j < sentence.WordCount(); j++)
                {
                    var word = (AnnotatedWord) sentence.GetWord(j);
                    if (word.GetShallowParse() == null)
                    {
                        Console.WriteLine("Shallow Parse annotation does not exist for sentence " +
                                          sentence.GetFileName());
                        break;
                    }
                }
            }
        }

        /**
         * <summary> The method traverses all words in all sentences and prints the words which do not have sense annotation.</summary>
         */
        public void CheckSemantic()
        {
            for (var i = 0; i < SentenceCount(); i++)
            {
                var sentence = (AnnotatedSentence) GetSentence(i);
                for (var j = 0; j < sentence.WordCount(); j++)
                {
                    var word = (AnnotatedWord) sentence.GetWord(j);
                    if (word.GetSemantic() == null)
                    {
                        Console.WriteLine("Semantic annotation does not exist for sentence " + sentence.GetFileName());
                        break;
                    }
                }
            }
        }

        /**
         * <summary> Creates a dictionary from the morphologically annotated words.</summary>
         * <returns>A dictionary containing root forms of the morphological annotations of words.</returns>
         */
        public TxtDictionary CreateDictionary()
        {
            var dictionary = new TxtDictionary(new TurkishWordComparator());
            for (var i = 0; i < SentenceCount(); i++)
            {
                var sentence = (AnnotatedSentence) GetSentence(i);
                for (var j = 0; j < sentence.WordCount(); j++)
                {
                    var word = (AnnotatedWord) sentence.GetWord(j);
                    if (word.GetParse() != null)
                    {
                        var morphologicalParse = word.GetParse();
                        var pos = morphologicalParse.GetRootPos();
                        var name = morphologicalParse.GetWord().GetName();
                        switch (pos)
                        {
                            case "NOUN":
                                if (morphologicalParse.IsProperNoun())
                                {
                                    dictionary.AddProperNoun(name);
                                }
                                else
                                {
                                    dictionary.AddNoun(name);
                                }

                                break;
                            case "VERB":
                                dictionary.AddVerb(name);
                                break;
                            case "ADJ":
                                dictionary.AddAdjective(name);
                                break;
                            case "ADV":
                                dictionary.AddAdverb(name);
                                break;
                        }
                    }
                }
            }

            return dictionary;
        }

        public RootWordStatistics ExtractRootWordStatistics(FsmMorphologicalAnalyzer fsm)
        {
            var statistics = new RootWordStatistics();
            var rootWordFiles = new Dictionary<string, List<string>>();
            for (var i = 0; i < SentenceCount(); i++)
            {
                var sentence = (AnnotatedSentence) GetSentence(i);
                for (var j = 0; j < sentence.WordCount(); j++)
                {
                    var word = (AnnotatedWord) sentence.GetWord(j);
                    if (word.GetName() != null)
                    {
                        var parseList = fsm.MorphologicalAnalysis(word.GetName());
                        var parse = word.GetParse();
                        if (parseList.Size() > 0 && parse != null)
                        {
                            var rootWords = parseList.RootWords();
                            if (rootWords.Contains("$"))
                            {
                                CounterHashMap<string> rootWordStatistics;
                                List<string> fileNames;
                                if (!statistics.ContainsKey(rootWords))
                                {
                                    rootWordStatistics = new CounterHashMap<string>();
                                    fileNames = new List<string>();
                                }
                                else
                                {
                                    rootWordStatistics = statistics.Get(rootWords);
                                    fileNames = rootWordFiles[rootWords];
                                }

                                fileNames.Add(sentence.GetFileName());
                                rootWordFiles[rootWords] = fileNames;
                                rootWordStatistics.Put(parse.GetWord().GetName());
                                statistics.Put(rootWords, rootWordStatistics);
                            }
                        }
                    }
                }
            }

            return statistics;
        }
    }
}