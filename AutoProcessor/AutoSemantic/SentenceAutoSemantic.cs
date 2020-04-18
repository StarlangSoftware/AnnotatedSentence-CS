namespace AnnotatedSentence.AutoProcessor.AutoSemantic
{
    public abstract class SentenceAutoSemantic
    {
        /**
         * <summary> The method should set the senses of all words, for which there is only one possible sense.</summary>
         * <param name="sentence">The sentence for which word sense disambiguation will be determined automatically.</param>
         */
        protected abstract void AutoLabelSingleSemantics(AnnotatedSentence sentence);

        public void AutoSemantic(AnnotatedSentence sentence)
        {
            AutoLabelSingleSemantics(sentence);
        }
    }
}