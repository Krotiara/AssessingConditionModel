using PatientDataHandler.API.Entities;
using PatientDataHandler.API.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.UnitTests.Service
{
    public class ExcelDataProviderTests
    {

        [Fact]
        public void ParseCorrectBytesMustContainsNotNullEntities()
        {
            ExcelDataProvider dataProvider = new ExcelDataProvider();
            IList<Influence> influences = dataProvider.ParseData(GetTestDataBytes("correctData.xlsx"));

            Assert.True(influences.Count() > 0);
            
            foreach(Influence influence in influences)
            {
                Assert.True(influence.DynamicParameters != null);
                Assert.True(influence.StartParameters != null);
                Assert.True(influence.InfluenceType != Interfaces.InfluenceTypes.None);
                Assert.True(influence.MedicineName != null);
                Assert.True(influence.StartTimestamp != default(DateTime));
                Assert.True(influence.EndTimestamp != default(DateTime));
                Assert.True(influence.PatientId != -1);
                Assert.True(influence.Patient != null);
            }    
        }


        [Fact]
        public void ParseBytesWithEmptyFieldsMustThrow()
        {
            ExcelDataProvider dataProvider = new ExcelDataProvider();
            Assert.Throws<ParseInfluenceDataException>(() 
                => dataProvider.ParseData(GetTestDataBytes("dataWithEmptyFields.xlsx")));
        }


        [Fact]
        public void ParseNotExcelFileBytesMustThrow()
        {
            byte[] notExcelBytes = Encoding.ASCII.GetBytes("not excel");
            ExcelDataProvider dataProvider = new ExcelDataProvider();
            Assert.Throws<ParseInfluenceDataException>(() => dataProvider.ParseData(notExcelBytes));
        }


        [Fact]
        public void ParseDataWithoutPatientIdMustThrow()
        {
            byte[] dataWithoutIds = GetTestDataBytes("dataWithoutIds.xlsx");
            ExcelDataProvider dataProvider = new ExcelDataProvider();
            Assert.Throws<ParseInfluenceDataException>(() => dataProvider.ParseData(dataWithoutIds));
        }


        private byte[] GetTestDataBytes(string fileNameWithExtension)
        {
            string projectDirectory = Directory
                .GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string path = Path.Combine(projectDirectory, $"TestFiles/{fileNameWithExtension}");
            byte[] bytes = File.ReadAllBytes(path);
            return bytes;
        }
    }
}
