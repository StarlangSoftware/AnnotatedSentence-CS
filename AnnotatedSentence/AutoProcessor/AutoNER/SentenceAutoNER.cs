namespace AnnotatedSentence.AutoProcessor.AutoNER
{
    /**
     * <summary>Abstract class to detect named entities in a sentence automatically. By implementing 5 abstract methods,
     * the class can detect (i) Person, (ii) Location, (iii) Organization, (iv) Money, (v) Time.
     * Each method tries to detect those named entities and if successful, sets the correct named entity for the word.
     * Anything that is denoted by a proper name, i. e., for instance, a person, a location, or an organization,
     * is considered to be a named entity. In addition, named entities also include things like dates, times,
     * or money. Here is a sample text with named entities marked:
     * [$_{ORG}$ Türk Hava Yolları] bu [$_{TIME}$ Pazartesi'den] itibaren [$_{LOC}$ İstanbul] [$_{LOC}$ Ankara]
     * güzergahı için indirimli satışlarını [$_{MONEY}$ 90 TL'den] başlatacağını açıkladı.
     * This sentence contains 5 named entities including 3 words labeled as ORGANIZATION, 2 words labeled as
     * LOCATION, 1 word labeled as TIME, and 1 word labeled as MONEY.</summary>
     */
    public abstract class SentenceAutoNER : NamedEntityRecognition.AutoNER
    {
        /**
         * <summary> The method should detect PERSON named entities. PERSON corresponds to people or
         * characters. Example: {\bf Atatürk} yurdu düşmanlardan kurtardı.</summary>
         * <param name="sentence">The sentence for which PERSON named entities checked.</param>
         */
        protected abstract void AutoDetectPerson(AnnotatedSentence sentence);

        /**
         * <summary> The method should detect LOCATION named entities. LOCATION corresponds to regions,
         * mountains, seas. Example: Ülkemizin başkenti {\bf Ankara'dır}.</summary>
         * <param name="sentence">The sentence for which LOCATION named entities checked.</param>
         */
        protected abstract void AutoDetectLocation(AnnotatedSentence sentence);

        /**
         * <summary> The method should detect ORGANIZATION named entities. ORGANIZATION corresponds to companies,
         * teams etc. Example:  {\bf IMKB} günü 60 puan yükselerek kapattı.</summary>
         * <param name="sentence">The sentence for which ORGANIZATION named entities checked.</param>
         */
        protected abstract void AutoDetectOrganization(AnnotatedSentence sentence);

        /**
         * <summary> The method should detect MONEY named entities. MONEY corresponds to monetarial
         * expressions. Example: Geçen gün {\bf 3000 TL} kazandık.</summary>
         * <param name="sentence">The sentence for which MONEY named entities checked.</param>
         */
        protected abstract void AutoDetectMoney(AnnotatedSentence sentence);

        /**
         * <summary> The method should detect TIME named entities. TIME corresponds to time
         * expressions. Example: {\bf Cuma günü} tatil yapacağım.</summary>
         * <param name="sentence">The sentence for which TIME named entities checked.</param>
         */
        protected abstract void AutoDetectTime(AnnotatedSentence sentence);

        /**
         * <summary> The main method to automatically detect named entities in a sentence. The algorithm
         * 1. Detects PERSON(s).
         * 2. Detects LOCATION(s).
         * 3. Detects ORGANIZATION(s).
         * 4. Detects MONEY.
         * 5. Detects TIME.
         * For not detected words, the algorithm sets the named entity "NONE".</summary>
         * <param name="sentence">The sentence for which named entities checked.</param>
         */
        public void AutoNER(AnnotatedSentence sentence)
        {
            AutoDetectPerson(sentence);
            AutoDetectLocation(sentence);
            AutoDetectOrganization(sentence);
            AutoDetectMoney(sentence);
            AutoDetectTime(sentence);
        }
    }
}