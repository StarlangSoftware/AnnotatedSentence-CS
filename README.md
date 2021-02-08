This resource allows for matching of Turkish words or expressions with their corresponding entries within the Turkish dictionary, the Turkish PropBank TRopBank, morphological analysis, named entity recognition, word senses from Turkish WordNet KeNet, shallow parsing, and universal dependency relation.

## Data Format

The structure of a sample annotated word is as follows:

	{turkish=Gelir}
	{morphologicalAnalysis=gelir+NOUN+A3SG+PNON+NOM}
	{metaMorphemes=gelir}
	{semantics=TUR10-0289950}
	{namedEntity=NONE}
	{propbank=ARG0$TUR10-0798130}
	{shallowParse=ÖZNE}
	{universalDependency=10$NSUBJ}

As is self-explanatory, 'turkish' tag shows the original Turkish word; 'morphologicalAnalysis' tag shows the correct morphological parse of that word; 'semantics' tag shows the ID of the correct sense of that word; 'namedEntity' tag shows the named entity tag of that word; 'shallowParse' tag shows the semantic role of that word; 'universalDependency' tag shows the index of the head word and the universal dependency for this word; 'propbank' tag shows the semantic role of that word for the verb synset id (frame id in the frame file) which is also given in that tag.

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
