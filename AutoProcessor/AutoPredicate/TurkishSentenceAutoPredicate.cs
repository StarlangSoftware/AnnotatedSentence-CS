using PropBank;

namespace AnnotatedSentence.AutoProcessor.AutoPredicate
{
    public class TurkishSentenceAutoPredicate : SentenceAutoPredicate
    {
        private readonly FramesetList _framesetList;

        /**
         * <summary> Constructor for {@link TurkishSentenceAutoPredicate}. Gets the FrameSets as input from the user, and sets
         * the corresponding attribute.</summary>
         * <param name="framesetList">FramesetList containing the Turkish propbank frames.</param>
         */
        public TurkishSentenceAutoPredicate(FramesetList framesetList)
        {
            this._framesetList = framesetList;
        }

        /**
         * <summary> The method uses predicateCandidates method to predict possible predicates. For each candidate, it sets for that
         * word PREDICATE tag.</summary>
         * <param name="sentence">The sentence for which predicates will be determined automatically.</param>
         * <returns>If at least one word has been tagged, true; false otherwise.</returns>
         */
        public override bool AutoPredicate(AnnotatedSentence sentence)
        {
            var candidateList = sentence.PredicateCandidates(_framesetList);
            foreach (var word in candidateList){
                word.SetArgument("PREDICATE$" + word.GetSemantic());
            }
            if (candidateList.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}