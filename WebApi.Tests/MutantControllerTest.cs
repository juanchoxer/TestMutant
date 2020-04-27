using WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace WebApi.Tests
{
    [TestClass]
    public class MutantControllerTest
    {
        [TestMethod]
        public void IsMutantTrueTest()
        {
            MutantController controller = new MutantController();

            string[] dna = new string[6] { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG" };
            ReceiverDNA receiver = new ReceiverDNA() { dna = dna };

            var result = controller.IsMutantAsync(receiver);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateDNAAndBuildGridTrueTest()
        {
            TesterDNA testerDNA = new TesterDNA();

            string[] dna = new string[6] { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG" };

            bool result = testerDNA.ValidateDNAAndBuildGrid(dna);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ValidateDNAAndBuildGridTFalseTest()
        {
            TesterDNA testerDNA = new TesterDNA();

            string[] dna = new string[6] { "ATGCGA", "CAGTGC", "TTATTT", "AGACGG", "GCGTCZ", "TCACTG" };

            bool result = testerDNA.ValidateDNAAndBuildGrid(dna);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void FindMatchesTrueTest()
        {
            TesterDNA testerDNA = new TesterDNA();

            string[] dna = new string[6] { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG" };

            testerDNA.ValidateDNAAndBuildGrid(dna);

            bool result = testerDNA.FindMatches();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void FindMatchesFalseTest()
        {
            TesterDNA testerDNA = new TesterDNA();

            string[] dna = new string[6] { "ATGCGA", "CAGTGC", "TTATTT", "AGACGG", "GCGTCA", "TCACTG" };

            testerDNA.ValidateDNAAndBuildGrid(dna);

            bool result = testerDNA.FindMatches();

            Assert.AreEqual(false, result);
        }
    }
}
