using System.IO;
using NUnit.Framework;
using PropBank;

namespace Test
{
    public class AnnotatedSentenceTest
    {
        AnnotatedSentence.AnnotatedSentence sentence0, sentence1, sentence2, sentence3, sentence4;
        AnnotatedSentence.AnnotatedSentence sentence5, sentence6, sentence7, sentence8, sentence9;

        [SetUp]
        public void Setup()
        {
            sentence0 = new AnnotatedSentence.AnnotatedSentence(new FileInfo("../../../sentences/0000.dev"));
            sentence1 = new AnnotatedSentence.AnnotatedSentence(new FileInfo("../../../sentences/0001.dev"));
            sentence2 = new AnnotatedSentence.AnnotatedSentence(new FileInfo("../../../sentences/0002.dev"));
            sentence3 = new AnnotatedSentence.AnnotatedSentence(new FileInfo("../../../sentences/0003.dev"));
            sentence4 = new AnnotatedSentence.AnnotatedSentence(new FileInfo("../../../sentences/0004.dev"));
            sentence5 = new AnnotatedSentence.AnnotatedSentence(new FileInfo("../../../sentences/0005.dev"));
            sentence6 = new AnnotatedSentence.AnnotatedSentence(new FileInfo("../../../sentences/0006.dev"));
            sentence7 = new AnnotatedSentence.AnnotatedSentence(new FileInfo("../../../sentences/0007.dev"));
            sentence8 = new AnnotatedSentence.AnnotatedSentence(new FileInfo("../../../sentences/0008.dev"));
            sentence9 = new AnnotatedSentence.AnnotatedSentence(new FileInfo("../../../sentences/0009.dev"));
        }

        [Test]
        public void TestGetShallowParseGroups()
        {
            Assert.AreEqual(4, sentence0.GetShallowParseGroups().Count);
            Assert.AreEqual(5, sentence1.GetShallowParseGroups().Count);
            Assert.AreEqual(3, sentence2.GetShallowParseGroups().Count);
            Assert.AreEqual(5, sentence3.GetShallowParseGroups().Count);
            Assert.AreEqual(5, sentence4.GetShallowParseGroups().Count);
            Assert.AreEqual(5, sentence5.GetShallowParseGroups().Count);
            Assert.AreEqual(6, sentence6.GetShallowParseGroups().Count);
            Assert.AreEqual(5, sentence7.GetShallowParseGroups().Count);
            Assert.AreEqual(5, sentence8.GetShallowParseGroups().Count);
            Assert.AreEqual(3, sentence9.GetShallowParseGroups().Count);
        }

        [Test]
        public void TestContainsPredicate()
        {
            Assert.True(sentence0.ContainsPredicate());
            Assert.True(sentence1.ContainsPredicate());
            Assert.False(sentence2.ContainsPredicate());
            Assert.True(sentence3.ContainsPredicate());
            Assert.True(sentence4.ContainsPredicate());
            Assert.False(sentence5.ContainsPredicate());
            Assert.False(sentence6.ContainsPredicate());
            Assert.True(sentence7.ContainsPredicate());
            Assert.True(sentence8.ContainsPredicate());
            Assert.True(sentence9.ContainsPredicate());
        }

        [Test]
        public void TestGetPredicate()
        {
            Assert.AreEqual("bulandırdı", sentence0.GetPredicate(0));
            Assert.AreEqual("yapacak", sentence1.GetPredicate(0));
            Assert.AreEqual("ediyorlar", sentence3.GetPredicate(0));
            Assert.AreEqual("yazmıştı", sentence4.GetPredicate(0));
            Assert.AreEqual("olunacaktı", sentence7.GetPredicate(0));
            Assert.AreEqual("gerekiyordu", sentence8.GetPredicate(0));
            Assert.AreEqual("ediyor", sentence9.GetPredicate(0));
        }

        [Test]
        public void TestToStems()
        {
            Assert.AreEqual("devasa ölçek yeni kanun kullan karmaşık ve çetrefil dil kavga bulan .", sentence0.ToStems());
            Assert.AreEqual("gelir art usul komite gel salı gün kanun tasarı hakkında bir duruşma yap .",
                sentence1.ToStems());
            Assert.AreEqual("reklam ve tanıtım iş yara yara gör üzere .", sentence2.ToStems());
            Assert.AreEqual("bu defa , daha da hız hareket et .", sentence3.ToStems());
            Assert.AreEqual("shearson lehman hutton ınc. dün öğle sonra kadar yeni tv reklam yaz .", sentence4.ToStems());
            Assert.AreEqual("bu kez , firma hazır .", sentence5.ToStems());
            Assert.AreEqual("`` diyalog sür kesinlikle temel önem haiz .", sentence6.ToStems());
            Assert.AreEqual("cuma gün bu üzerine düşün çok geç kal ol .", sentence7.ToStems());
            Assert.AreEqual("bu hakkında önceden düşün gerek . ''", sentence8.ToStems());
            Assert.AreEqual("isim göre çeşit göster birkaç kefaret fon reklam yap için devam et .", sentence9.ToStems());
        }

        [Test]
        public void TestPredicateCandidates()
        {
            FramesetList framesetList = new FramesetList();
            Assert.AreEqual(1, sentence0.PredicateCandidates(framesetList).Count);
            Assert.AreEqual(1, sentence1.PredicateCandidates(framesetList).Count);
            Assert.AreEqual(0, sentence2.PredicateCandidates(framesetList).Count);
            Assert.AreEqual(2, sentence3.PredicateCandidates(framesetList).Count);
            Assert.AreEqual(1, sentence4.PredicateCandidates(framesetList).Count);
            Assert.AreEqual(0, sentence5.PredicateCandidates(framesetList).Count);
            Assert.AreEqual(0, sentence6.PredicateCandidates(framesetList).Count);
            Assert.AreEqual(1, sentence7.PredicateCandidates(framesetList).Count);
            Assert.AreEqual(1, sentence8.PredicateCandidates(framesetList).Count);
            Assert.AreEqual(2, sentence9.PredicateCandidates(framesetList).Count);
        }
    }
}