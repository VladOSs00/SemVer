using NUnit.Framework;
using SemVer;

namespace Testing34
{
    class UnionIntersectEqualsTest
    {

        [Test]
        public void EqualsTest()
        {
            var rangeA = new Range(">=1.2.3 < 1.3.0 || =1.3.2");
            var rangeB = new Range("=1.3.2 || >=1.2.3 <1.3.0");
            Assert.IsTrue(rangeA.Equals(rangeB));

            rangeA = new Range(">=1.2.3 < 1.3.0 || =1.3.2");
            rangeB = new Range("=1.3.1 || >=1.2.3 <1.3.0");
            Assert.IsFalse(rangeA.Equals(rangeB));

            rangeA = new Range(">1.0.0 <2.0.0");
            rangeB = new Range(">1.0.0 <3.0.0");
            Assert.IsFalse(rangeA.Equals(rangeB));

            rangeA = new Range(">1.2.3");
            rangeB = new Range(">=1.2.3");
            Assert.IsFalse(rangeA.Equals(rangeB));

            rangeA = new Range(">1.2.3");
            rangeB = new Range(">1.2.0");
            Assert.IsFalse(rangeA.Equals(rangeB));
        }


        [Test]
        public void UnionTest()
        {
            var rangeA = new Range("<2.0.0 || >3.1.4");
            var rangeContains = new Range(">=1.0.0  || <=1.9.9");
            var rangeContains2 = new Range(">=3.1.5  || <=9");
            var rangeNotContain = new Range(">2.0.0 < 3.1.4");
            var versionNotContain = new Version("2.5.5");

            Assert.IsTrue(rangeA.Contains(rangeContains));
            Assert.IsTrue(rangeA.Contains(rangeContains2));
            Assert.IsFalse(rangeA.Contains(rangeNotContain));
            Assert.IsFalse(rangeA.Contains(versionNotContain));
        }

        [Test]
        public void IntersectTest()
        {
            var noSimilarityRange = new Range(">=9");

            var rangeA = new Range(">=1.2.3");
            var rangeB = new Range(">=1.2.0 < 1.2.6");
            var expected = new Range(">=1.2.3 < 1.2.6");
            var rangeIntersect = rangeA.Intersect(rangeB);
            Assert.AreEqual(expected, rangeIntersect);
            Assert.AreNotEqual(expected, noSimilarityRange);

            rangeA = new Range(">=1.2.3");
            rangeB = new Range("=1.2.7");
            expected = new Range("=1.2.7");
            rangeIntersect = rangeA.Intersect(rangeB);
            Assert.AreEqual(expected, rangeIntersect);
            Assert.AreNotEqual(expected, noSimilarityRange);

            rangeA = new Range("<=1.2.3");
            rangeB = new Range(">=1.2.4");
            expected = new Range("<0.0.0");
            rangeIntersect = rangeA.Intersect(rangeB);
            Assert.AreEqual(expected, rangeIntersect);
            Assert.AreNotEqual(expected, noSimilarityRange);

            rangeA = new Range("<3.0.0");
            rangeB = new Range(">=3.0.0");
            expected = new Range("<0.0.0");
            rangeIntersect = rangeA.Intersect(rangeB);
            Assert.AreEqual(expected, rangeIntersect);
            Assert.AreNotEqual(expected, noSimilarityRange);

            rangeA = new Range("<3.0.0");
            rangeB = new Range(">=3.0.0");
            expected = new Range("<0.0.0");
            rangeIntersect = rangeA.Intersect(rangeB);
            Assert.AreEqual(expected, rangeIntersect);
            Assert.AreNotEqual(expected, noSimilarityRange);

            var ex = Assert.Throws<System.ArgumentException>(() => new Range("pepega"));
            Assert.That(ex.Message, Is.EqualTo("Invalid version string: \"pepega\""));
        }
    }
}
