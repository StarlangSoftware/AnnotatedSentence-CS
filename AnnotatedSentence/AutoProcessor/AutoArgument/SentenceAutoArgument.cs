namespace AnnotatedSentence.AutoProcessor.AutoArgument
{
    public abstract class SentenceAutoArgument
    {
        /**
         * <summary> The method should set all the semantic role labels in the sentence. The method assumes that the predicates
         * of the sentences were determined previously.</summary>
         * <param name="sentence">The sentence for which semantic roles will be determined automatically.</param>
         */
        public abstract bool AutoArgument(AnnotatedSentence sentence);
    }
}