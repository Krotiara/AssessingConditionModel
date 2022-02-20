using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.DataHandler
{
    public class DataPreprocessor
    {
        private readonly Regex ageYearRegex = new Regex(@"\d+г");
        private readonly Regex ageMonthRegex = new Regex(@"\d+мес");
        private readonly Regex dateRegex = new Regex(@"\d{1,2}.\d{1,2}.\d{4}");
        private readonly Regex idRegex = new Regex(@"\d+");


        public List<List<string>> PreProcessData(List<List<string>> data, ref Dictionary<string, int> headersColumnIndexes)
        {
            headersColumnIndexes = headersColumnIndexes.ToDictionary(p => p.Key.Trim().ToLower(), p => p.Value);
            var copyHeaders = headersColumnIndexes;
            data = data.Where(row => IsCorrectDataString(row, copyHeaders)).ToList();
            for (int i = 0; i < data.Count; i++)
            {
                List<string> row = data[i].Select(x=>x.Trim().ToLower()).ToList();
                AdjustRowDelimeters(ref row);
                AdjustGender(ref row, headersColumnIndexes);
                AdjustAge(ref row, headersColumnIndexes);
                data[i] = row;
            }
            return data;
        }


        private bool IsCorrectDataString(List<string> rawRow, Dictionary<string, int> headersColumnIndexes)
        {
            try
            {
                int idIndex = headersColumnIndexes.Where(pair => pair.Key.Equals("номер истории болезни")).First().Value;
                return idRegex.IsMatch(rawRow[idIndex]);
            }
            catch(IndexOutOfRangeException)
            {
                return false;
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


        private void AdjustAge(ref List<string> rawRow, Dictionary<string, int> headersColumnIndexes)
        {
            int ageIndex = headersColumnIndexes
                .Where(pair => pair.Key.Equals("возраст") || pair.Key.Equals("возраст ребенка")) //Лучше еще заменить возраст ребенка на возраст в начале
                .First()
                .Value;
            
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


        private void AdjustGender(ref List<string> rawRow, Dictionary<string, int> headersColumnIndexes)
        {
            try
            {
                int genderIndex = headersColumnIndexes.Where(pair => pair.Key.Equals("пол")).First().Value;
                if (rawRow[genderIndex].Equals("мужск"))
                    rawRow[genderIndex] = "м";
                if (rawRow[genderIndex].Equals("женск"))
                    rawRow[genderIndex] = "ж";
            }
            catch(System.ArgumentOutOfRangeException)
            {
                return; //TODO log - некорректная строка. Лучше засунуть раньше
            }
        }


       

    }
}
