For Developers
============

You can also see [Java](https://github.com/starlangsoftware/AnnotatedSentence), [Python](https://github.com/starlangsoftware/AnnotatedSentence-Py), [Cython](https://github.com/starlangsoftware/AnnotatedSentence-Cy), or [C++](https://github.com/starlangsoftware/AnnotatedSentence-CPP) repository.

## Requirements

* C# Editor
* [Git](#git)

### Git

Install the [latest version of Git](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git).

## Download Code

In order to work on code, create a fork from GitHub page. 
Use Git for cloning the code to your local or below line for Ubuntu:

	git clone <your-fork-git-link>

A directory called AnnotatedSentence-CS will be created. Or you can use below link for exploring the code:

	git clone https://github.com/starlangsoftware/AnnotatedSentence-CS.git

## Open project with Rider IDE

To import projects from Git with version control:

* Open Rider IDE, select Get From Version Control.

* In the Import window, click URL tab and paste github URL.

* Click open as Project.

Result: The imported project is listed in the Project Explorer view and files are loaded.


## Compile

**From IDE**

After being done with the downloading and opening project, select **Build Solution** option from **Build** menu. After compilation process, user can run AnnotatedSentence-CS.

Detailed Description
============

+ [AnnotatedCorpus](#annotatedcorpus)
+ [AnnotatedSentence](#annotatedsentence)
+ [AnnotatedWord](#annotatedword)
+ [Automatic Annotation](#automatic-annotation)


## AnnotatedCorpus

To load the annotated corpus:

	AnnotatedCorpus(string folder, string pattern)
	a = AnnotatedCorpus("/Turkish-Phrase", ".train")

	AnnotatedCorpus(string folder)
	a = AnnotatedCorpus("/Turkish-Phrase")

To access all the sentences in a AnnotatedCorpus:

	for (int i = 0; i < a.SentenceCount(); i++){
		AnnotatedSentence annotatedSentence = (AnnotatedSentence) a.GetSentence(i);
		....
	}

## AnnotatedSentence

To access all the words in a AnnotatedSentence:

	for (int j = 0; j < annotatedSentence.WordCount(); j++){
		AnnotatedWord annotatedWord = (AnnotatedWord) annotatedSentence.GetWord(j);
		...
	}

## AnnotatedWord

An annotated word is kept in AnnotatedWord class. To access the morphological analysis of 
the annotated word:

	MorphologicalParse GetParse()

Meaning of the annotated word:

	String GetSemantic()

NER annotation of the annotated word:

	NamedEntityType GetNamedEntityType()

Shallow parse tag of the annotated word (e.g., subject, indirect object):

	String GetShallowParse()

Dependency annotation of the annotated word:

	UniversalDependencyRelation GetUniversalDependency()
	
## Automatic Annotation

To detect predicates of a sentence automatically:

	TurkishSentenceAutoPredicate(FramesetList framesetList)

this class is used. For example, with

	a = TurkishSentenceAutoPredicate(new FramesetList());
	a.AutoPredicate(sentence);

the predicates of the sentence "sentence" are annotated automatically.

To detect arguments of a sentence automatically

	TurkishSentenceAutoArgument()

this class is used. For example, with

	a = TurkishSentenceAutoArgument();
	a.AutoArgument(sentence);

arguments of the sentence "sentence" are annotated automatically.

To disambiguate the morphological ambiguity in a sentence automatically

	TurkishSentenceAutoDisambiguator(RootWordStatistics rootWordStatistics)
	TurkishSentenceAutoDisambiguator(FsmMorphologicalAnalyzer fsm, RootWordStatistics rootWordStatistics)
								  
this class is used. For example, with 

	a = TurkishSentenceAutoDisambiguator(new RootWordStatistics());
	a.AutoDisambiguate(sentence);

morphological disambugiation of the sentence "sentence" is done automatically.

To make a named entity recognition in a sentence

	TurkishSentenceAutoNER()

this class is used. For example, with

	a = TurkishSentenceAutoNER();
	a.AutoNER(sentence);

named entity recognition in the sentence "sentence" is done automatically.

To make a semantic annotation in a sentence

	TurkishSentenceAutoSemantic()

this class is used. For example, with

	a = TurkishSentenceAutoSemantic();
	a.AutoSemantic(sentence);

semantic annotation of the sentence "sentence" is done automatically.
