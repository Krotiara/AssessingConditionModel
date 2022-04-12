using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.LungsModel
{
    public abstract class Lung
    {
        public bool[] SegmentsIsDamage { get; set; }

        public LungDamages LungDamage { get; set; }

        public Dictionary<LungFractions, int[]> FractionIndexes { get; set; } = new Dictionary<LungFractions, int[]>();

        abstract public void InitLung();


        public List<int> GetDamagedSegmentsIndexesBy(string segmentDescription)
        {
            segmentDescription = segmentDescription.Trim().ToLower();
            Regex fractionFormat = new Regex(@"нижняя|верхняя|средняя");
            Regex segmentFormat = new Regex(@"s\d{1,2}");

            List<int> resultIndexes = new List<int>();
            IEnumerable<string> parts = segmentDescription.Split(',').Select(x=>x.Trim());
            foreach(string part in parts)
            {
                try
                {
                    Match fractionFormatMatch = fractionFormat.Match(part);
                    Match segmentFormatMath = segmentFormat.Match(part);
                    if (fractionFormatMatch.Success)
                    {
                        //выудить среднее, нижнее или верхнее
                        LungFractions lungFraction = fractionFormatMatch.Value.GetValueFromName<LungFractions>();
                        resultIndexes.AddRange(FractionIndexes[lungFraction]);
                    }
                    else if (segmentFormatMath.Success)
                    {
                        //выудить номер, минус 1.
                        int segmentNumber = int.Parse(segmentFormatMath.Value.Replace("s", ""));
                        resultIndexes.Add(segmentNumber - 1);
                    }
                    else
                    {
                        //TODO add log
                    }
                }
                catch(Exception ex)
                {
                    //TODO лог
                    continue;
                }
            }
            return resultIndexes
                .Distinct()
                .OrderBy(x=>x)
                .ToList();
        }


        public LungDamages GetLungDamageBy(string lungDamageDescription)
        {
            try
            {
                lungDamageDescription = lungDamageDescription.Trim().ToLower();
                Regex lungDamageRegex = new Regex(@"кт\d|кт \d|кт-\d"); //TODO тест на кт-\d
                Match lungDamageMatch = lungDamageRegex.Match(lungDamageDescription);
                if (lungDamageMatch.Success)
                    return lungDamageMatch.Value
                        .Replace(" ","")
                        .Replace("-","")
                        .GetValueFromName<LungDamages>();
                else
                {
                    // TODO log
                    return LungDamages.No;
                }
            }
            catch(Exception ex)
            {
                //TODO log
                return LungDamages.No;
            }
        }


        //public void SetSegmentDamage(int segmentNumberFromOne)
        //{
        //    //TODO
        //}
    }
}
