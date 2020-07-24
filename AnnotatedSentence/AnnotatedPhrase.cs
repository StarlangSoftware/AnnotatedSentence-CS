using Corpus;

namespace AnnotatedSentence
{
    public class AnnotatedPhrase : Sentence
    {
        private readonly int _wordIndex;
        private readonly string _tag;

        /**
         * <summary> Constructor for AnnotatedPhrase. AnnotatedPhrase stores information about phrases such as
         * Shallow Parse phrases or named entity phrases.</summary>
         * <param name="wordIndex">Starting index of the first word in the phrase w.r.t. original sentence the phrase occurs.</param>
         * <param name="tag">Tag of the phrase. Corresponds to the shallow parse or named entity tag.</param>
         */
        public AnnotatedPhrase(int wordIndex, string tag)
        {
            this._wordIndex = wordIndex;
            this._tag = tag;
        }

        /**
         * <summary> Accessor for the wordIndex attribute.</summary>
         * <returns>Starting index of the first word in the phrase w.r.t. original sentence the phrase occurs.</returns>
         */
        public int GetWordIndex()
        {
            return _wordIndex;
        }

        /**
         * <summary> Accessor for the tag attribute.</summary>
         * <returns>Tag of the phrase. Corresponds to the shallow parse or named entity tag.</returns>
         */
        public string GetTag()
        {
            return _tag;
        }
    }
}