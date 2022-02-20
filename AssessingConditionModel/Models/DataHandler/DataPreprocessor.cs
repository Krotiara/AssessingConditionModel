using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.DataHandler
{
    public class DataPreprocessor
    {

        public void PreProcessData(ref List<List<string>> data, ref Dictionary<string, int> headersColumnIndexes)
        {
            headersColumnIndexes = headersColumnIndexes.ToDictionary(p => p.Key.Trim().ToLower(), p => p.Value);

            for (int i = 0; i < data.Count; i++)
            {
                List<string> row = data[i].Select(x=>x.Trim().ToLower()).ToList();
                AdjustRowDelimeters(ref row);
                AdjustGender(ref row, headersColumnIndexes);
               
                data[i] = row;
            }  
        }


        private void AdjustRowDelimeters(ref List<string> rawRow)
        {
            Regex dateRegex = new Regex(@"\d{1,2}.\d{1,2}.\d{4}"); 
            for(int i=0; i < rawRow.Count; i++)
            {
                if (rawRow[i].Any(c => char.IsDigit(c)) && !dateRegex.IsMatch(rawRow[i]))
                    rawRow[i] = rawRow[i].Replace('.', ','); //Для преобразования в double
            }
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
