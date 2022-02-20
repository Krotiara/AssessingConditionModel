using AssessingConditionModel.Models;
using AssessingConditionModel.Models.DataHandler;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace AssessingConditionModelTests
{
    public class DataReaderTests
    {
        private readonly string testExcelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData\\multiLineTestFile.xlsx");
        private DataParser dataParser = new DataParser();

        public DataReaderTests()
        {

        }


        [Fact]
        public void GetExcelDataNotEmptyTest()
        {
            var datas = dataParser.GetExcelData(testExcelFilePath, new List<int> { 1 });
            Assert.NotEmpty(datas.Item1);
            Assert.NotEmpty(datas.Item2);
        }


        [Fact]
        public void ReadMultiLineCellIsCorrect()
        {
            var datas = dataParser.GetExcelData(testExcelFilePath, new List<int> { 1 });
            List<List<string>> data = datas.Item1;
            Dictionary<string, int> headersIndexes = datas.Item2;

            string expectedCellValue = "1г  11 мес (1,9)";

            Assert.Equal(expectedCellValue, data[0][0]);
        }

    }
}
