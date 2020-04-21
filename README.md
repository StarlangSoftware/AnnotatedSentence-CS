# AnnotatedSentence-CS

This resource allows for matching of Turkish words or expressions with their corresponding entries within the Turkish dictionary and the Turkish PropBank, morphological analysis, named entity recognition and shallow parsing.

## Data Format

The structure of a sample annotated word is as follows:

	{turkish=yatırımcılar}
	{analysis=yatırımcı+NOUN+A3PL+PNON+NOM}
	{semantics=0841060}
	{namedEntity=NONE}
	{shallowParse=ÖZNE}
	{propbank=ARG0:0006410}

As is self-explanatory, 'turkish' tag shows the original Turkish word; 'analysis' tag shows the correct morphological parse of that word; 'semantics' tag shows the ID of the correct sense of that word; 'namedEntity' tag shows the named entity tag of that word; 'shallowParse' tag shows the semantic role of that word; 'propbank' tag shows the semantic role of that word for the verb synset id (frame id in the frame file) which is also given in that tag.

------------------------------------------------

Detailed Description
============
+ [AnnotatedCorpus](#annotatedcorpus)
+ [AnnotatedSentence](#annotatedsentence)
+ [AnnotatedWord](#annotatedword)
+ [Automatic Annotation](#automatic-annotation)


## AnnotatedCorpus

İşaretlenmiş corpusu yüklemek için

	AnnotatedCorpus(string folder, string pattern)
	a = AnnotatedCorpus("/Turkish-Phrase", ".train")

	AnnotatedCorpus(string folder)
	a = AnnotatedCorpus("/Turkish-Phrase")

Bir AnnotatedCorpus'daki tüm cümlelere erişmek için

	for (int i = 0; i < a.SentenceCount(); i++){
		AnnotatedSentence annotatedSentence = (AnnotatedSentence) a.GetSentence(i);
		....
	}

## AnnotatedSentence

Bir AnnotatedSentence'daki tüm kelimelere ulaşmak için de

	for (int j = 0; j < annotatedSentence.WordCount(); j++){
		AnnotatedWord annotatedWord = (AnnotatedWord) annotatedSentence.GetWord(j);
		...
	}

## AnnotatedWord

İşaretlenmiş bir kelime AnnotatedWord sınıfında tutulur. İşaretlenmiş kelimenin morfolojik
analizi

	MorphologicalParse GetParse()

İşaretlenmiş kelimenin anlamı

	String GetSemantic()

İşaretlenmiş kelimenin NER anotasyonu

	NamedEntityType GetNamedEntityType()

İşaretlenmiş kelimenin özne, dolaylı tümleç, vs. shallow parse tagı

	String GetShallowParse()

İşaretlenmiş kelimenin dependency anotasyonu

	UniversalDependencyRelation GetUniversalDependency()
	
## Automatic Annotation

Bir cümlenin Predicatelarını otomatik olarak belirlemek için

	TurkishSentenceAutoPredicate(FramesetList framesetList)

sınıfı kullanılır. Örneğin,

	a = TurkishSentenceAutoPredicate(new FramesetList());
	a.AutoPredicate(sentence);

ile sentence cümlesinin predicateları otomatik olarak işaretlenir.

Bir cümlenin argümanlarını otomatik olarak belirlemek için

	TurkishSentenceAutoArgument()

sınıfı kullanılır. Örneğin,

	a = TurkishSentenceAutoArgument();
	a.AutoArgument(sentence);

ile sentence cümlesinin argümanları otomatik olarak işaretlenir.

Bir cümlede otomatik olarak morfolojik belirsizlik gidermek için

	TurkishSentenceAutoDisambiguator(RootWordStatistics rootWordStatistics)
	TurkishSentenceAutoDisambiguator(FsmMorphologicalAnalyzer fsm, RootWordStatistics rootWordStatistics)
								  
sınıfı kullanılır. Örneğin,

	a = TurkishSentenceAutoDisambiguator(new RootWordStatistics());
	a.AutoDisambiguate(sentence);

ile sentence cümlesinin morfolojik belirsizlik gidermesi otomatik olarak yapılır.

Bir cümlede adlandırılmış varlık tanıma yapmak için

	TurkishSentenceAutoNER()

sınıfı kullanılır. Örneğin,

	a = TurkishSentenceAutoNER();
	a.AutoNER(sentence);

ile sentence cümlesinde varlık tanıma otomatik olarak yapılır.

Bir cümlede anlamsal işaretleme için

	TurkishSentenceAutoSemantic()

sınıfı kullanılır. Örneğin,

	a = TurkishSentenceAutoSemantic();
	a.AutoSemantic(sentence);

ile sentence cümlesinde anlamsal işaretleme otomatik olarak yapılır.

## Cite
If you use this resource on your research, please cite the following paper: 

```
@INPROCEEDINGS{yildiz18, 
	author={O. T. {Yıldız} and K. {Ak} and G. {Ercan} and O. {Topsakal} and C. {Asmazoğlu}}, 
	booktitle={2018 2nd International Conference on Natural Language and Speech Processing (ICNLSP)}, 
	title={A multilayer annotated corpus for Turkish}, 
	year={2018}, 
	pages={1-6}
}

@article{acikgoz,
	title={All-words word sense disambiguation for {T}urkish},
	author={O. Açıkg{\"o}z and A. T. G{\"u}rkan and B. Ertopçu and O. Topsakal and B. {\"O}zenç and A. B. Kanburoğlu and {\.{I}}. Çam and B. Avar and G. Ercan and O. T. Y{\i}ld{\i}z},
	journal={2017 International Conference on Computer Science and Engineering (UBMK)},
	year={2017},
	pages={490-495}
}

@inproceedings{ertopcu17,  
	author={B. {Ertopçu} and A. B. {Kanburoğlu} and O. {Topsakal} and O. {Açıkgöz} and A. T. {Gürkan} and B. {Özenç} and İ. {Çam} and B. {Avar} and G. {Ercan} and O. T. {Yıldız}},  
	booktitle={2017 International Conference on Computer Science and Engineering (UBMK)},  title={A new approach for named entity recognition},   
	year={2017},  
	pages={474-479}
}

@INPROCEEDINGS{topsakal17,
	author={O. {Topsakal} and O. {Açıkgöz} and A. T. {Gürkan} and A. B. {Kanburoğlu} and B. {Ertopçu} and B. {Özenç} and İ. {Çam} and B. {Avar} and G. {Ercan} and O. T. {Yıldız}}, 
	booktitle={2017 International Conference on Computer Science and Engineering (UBMK)}, 
	title={Shallow parsing in Turkish}, 
	year={2017}, 
	pages={480-485}
}
