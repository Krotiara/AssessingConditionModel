using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PatientDataHandler.API.Entities
{
    public class DataPreprocessor
    {
        private readonly Regex ageYearRegex = new Regex(@"\d+г");
        private readonly Regex ageMonthRegex = new Regex(@"\d+мес");
        private readonly Regex dateRegex = new Regex(@"\d{1,2}.\d{1,2}.\d{4}");
        private readonly Regex idRegex = new Regex(@"\d+");


        /// <summary>
        /// Нулевая строка - заголовки
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IList<IList<string>> PreProcessData(IList<IList<string>> data)
        {
            int genderIndex = data[0]
                .Select(x=> x.Trim().ToLower())
                .Select((Value, Index) => new { Value, Index })
                .Single(p => p.Value == "пол" || p.Value == "gender").Index;

            int ageIndex = data[0]
                .Select(x => x.Trim().ToLower())
                .Select((Value, Index) => new { Value, Index })
                .Single(p => p.Value == "возраст" || p.Value == "age").Index;

            for (int i = 1; i < data.Count; i++)
            {
                List<string> row = data[i].Select(x=>x.Trim().ToLower()).ToList();
                //FillEmptyCells(ref row);
                AdjustRowDelimeters(ref row);
                AdjustGender(ref row, genderIndex);
                AdjustAge(ref row, ageIndex);
                data[i] = row;
            }
            return data;
        }


      

        private void FillEmptyCells(ref List<string> rawRow)
        {
            for(int i=0; i < rawRow.Count; i++)
            {
                if (rawRow[i].Equals(""))
                    rawRow[i] = "0";
            }
        }


        private void AdjustRowDelimeters(ref List<string> rawRow)
        {
           
            for(int i=0; i < rawRow.Count; i++)
            {
                if (rawRow[i].Any(c => char.IsDigit(c)) && !dateRegex.IsMatch(rawRow[i]))
                    rawRow[i] = rawRow[i].Replace('.', ','); //Для преобразования в double
            }
        }


        private void AdjustAge(ref List<string> rawRow, int ageIndex)
        {    
            string rawAge = rawRow[ageIndex].Replace(" ", ""); //Во входных данных могут встретиться пробелы, поэтому убираем их.
            if (rawAge.Equals("")) return; //TODO понадежнее обработку
           
            double age;
            bool isParseCorrect = double.TryParse(rawAge, out age);
            if (!isParseCorrect)
            {
                string yearString = ageYearRegex.Match(rawAge).Value;
                string monthString = ageMonthRegex.Match(rawAge).Value;
                double rAge = double.Parse(Regex.Match(yearString, @"\d").Value);
                double rMonth = double.Parse(Regex.Match(monthString, @"\d").Value);
                rawRow[ageIndex] = $"{rAge},{rMonth}";
            }
            else
                rawRow[ageIndex] = age.ToString();
        }


        private void AdjustGender(ref List<string> rawRow, int genderColumnIndex)
        {
            try
            {
                
                if (rawRow[genderColumnIndex].Equals("мужск"))
                    rawRow[genderColumnIndex] = "м";
                if (rawRow[genderColumnIndex].Equals("женск"))
                    rawRow[genderColumnIndex] = "ж";
            }
            catch(System.ArgumentOutOfRangeException)
            {
                return; //TODO log - некорректная строка. Лучше засунуть раньше
            }
        }


       

    }
}
