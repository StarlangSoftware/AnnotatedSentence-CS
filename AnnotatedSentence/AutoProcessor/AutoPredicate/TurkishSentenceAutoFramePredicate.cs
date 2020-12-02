namespace AnnotatedSentence.AutoProcessor.AutoPredicate
{
    public class TurkishSentenceAutoFramePredicate : SentenceAutoFramePredicate
    {
        private readonly FrameNet.FrameNet _frameNet;

        /**
         * <summary> Constructor for {@link TurkishSentenceAutoPredicate}. Gets the FrameSets as input from the user, and sets
         * the corresponding attribute.</summary>
         * <param name="frameNet">FrameNet containing the Turkish framenet frames.</param>
         */
        public TurkishSentenceAutoFramePredicate(FrameNet.FrameNet frameNet)
        {
            this._frameNet = frameNet;
        }

        /**
         * <summary> The method uses predicateCandidates method to predict possible predicates. For each candidate, it sets for that
         * word PREDICATE tag.</summary>
         * <param name="sentence">The sentence for which predicates will be determined automatically.</param>
         * <returns>If at least one word has been tagged, true; false otherwise.</returns>
         */
        public override bool AutoPredicate(AnnotatedSentence sentence)
        {
            var candidateList = sentence.PredicateFrameCandidates(_frameNet);
            foreach (var word in candidateList){
                word.SetArgument("PREDICATE$NONE$" + word.GetSemantic());
            }
            if (candidateList.Count > 0)
            {
                return true;
            }

            return false;
            
        }
    }
}