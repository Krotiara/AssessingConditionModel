using AssessingConditionModel.Models;
using AssessingConditionModel.Models.DataHandler;
using AssessingConditionModel.Models.LungsModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AssessingConditionModelTests
{
    public class LungModelTests
    {
        
        private LungsModel lungModel = new LungsModel();

        public LungModelTests()
        {

        }


        [Fact]
        public void GetMultiLungDamageInSegmentFormatTest()
        {
            string damageString = "S1, S9";
            List<int> assertIndexes = new List<int> { 0, 8 };

            List<int> indexes = lungModel.LeftLung.GetDamagedSegmentsIndexesBy(damageString);

            Assert.Equal(assertIndexes, indexes);
        }


        [Fact]
        public void GetMultiRightLungDamageInFractionFormatTest()
        {
            string damageString = "средн€€ дол€, верхн€€ дол€";

            List<int> assertIndexes = new List<int>();
            assertIndexes.AddRange(lungModel.RightLung.FractionIndexes[LungFractions.Middle]);
            assertIndexes.AddRange(lungModel.RightLung.FractionIndexes[LungFractions.Upper]);
            

            List<int> indexes = lungModel.RightLung.GetDamagedSegmentsIndexesBy(damageString);

            Assert.Equal(assertIndexes.OrderBy(x => x), indexes);
        }


        [Fact]
        public void GetLungDamageInSegmentFormatTest()
        {
            string damageString = "S5";

            List<int> assertIndexes = new List<int> { 4 };

            List<int> indexes = lungModel.LeftLung.GetDamagedSegmentsIndexesBy(damageString);

            Assert.Equal(assertIndexes, indexes);
        }


        [Fact]
        public void GetRightLungDamageInFractionFormatTest()
        {
            string damageString = "средн€€ дол€";
            List<int> assertIndexes = lungModel.RightLung.FractionIndexes[LungFractions.Middle].ToList();

            List<int> indexes = lungModel.RightLung.GetDamagedSegmentsIndexesBy(damageString);

            Assert.Equal(assertIndexes, indexes);
        }


        [Fact]
        public void GetMiddleFractionOnLeftLungMustReturnEmptyTest()
        {
            string damageString = "средн€€ дол€";
            List<int> assertIndexes = new List<int>();

            List<int> indexes = lungModel.LeftLung.GetDamagedSegmentsIndexesBy(damageString);

            Assert.Equal(assertIndexes, indexes);
        }


        [Fact]
        public void GetLungDamageInComboFormatTest()
        {
            string damageString = "S1, S2, нижн€€ дол€";

            List<int> assertIndexes = new List<int> { 0, 1 };
            assertIndexes.AddRange(lungModel.RightLung.FractionIndexes[LungFractions.Lower]);

            List<int> indexes = lungModel.RightLung.GetDamagedSegmentsIndexesBy(damageString);

            Assert.Equal(assertIndexes, indexes);
        }


        [Fact]
        public void GetLungDamageIndexesMustBeDistinctTest()
        {
            string damageString = "S1, S1, S1";
            List<int> assertIndexes = new List<int> { 0 };

            List<int> indexes = lungModel.RightLung.GetDamagedSegmentsIndexesBy(damageString);

            Assert.Equal(assertIndexes, indexes);
        }


        [Fact]
        public void GetLungDamageIndexesMustBeOrderedTest()
        {
            string damageString = "S4, S9, S6";

            List<int> assertIndexes = new List<int> { 3, 5, 8 };

            List<int> indexes = lungModel.RightLung.GetDamagedSegmentsIndexesBy(damageString);

            Assert.Equal(assertIndexes, indexes);
        }


        [Fact]
        public void GetLungDamageIndexesNustBeUpperIndependentTest()
        {
            string damageString = "S1, s2, Ќижн€€ дол€";
            List<int> assertIndexes = new List<int> { 0, 1 };
            assertIndexes.AddRange(lungModel.RightLung.FractionIndexes[LungFractions.Lower]);

            List<int> indexes = lungModel.RightLung.GetDamagedSegmentsIndexesBy(damageString);

            Assert.Equal(assertIndexes, indexes);
        }


        [Fact]
        public void LungDamageIndexMustBeMinusOneTest()
        {
            string damageString = "S1";
            List<int> assertIndexes = new List<int> { 0 };

            List<int> indexes = lungModel.RightLung.GetDamagedSegmentsIndexesBy(damageString);

            Assert.Equal(assertIndexes, indexes);
        }
    }
}
