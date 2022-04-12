using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.LungsModel
{
    public class LungsModel
    {

        public LungsModel()
        {
            LeftLung = new LeftLung();
            RightLung = new RightLung();
        }

        public LeftLung LeftLung { get; set; }

        public RightLung RightLung { get; set; }


        public void SetData(bool isRightHandDamage, bool isLeftHandDamage,
            string rightLungDamageDescription, string leftLungDamageDescription, string damageVolumeDescription)
        {
            if (isRightHandDamage)
            {
                List<int> damageIndexes = RightLung.GetDamagedSegmentsIndexesBy(rightLungDamageDescription);
                foreach (int index in damageIndexes)
                    RightLung.SegmentsIsDamage[index] = true;

                LungDamages lungDamage = RightLung.GetLungDamageBy(damageVolumeDescription);
                RightLung.LungDamage = lungDamage;
            }

            if(isLeftHandDamage)
            {
                List<int> damageIndexes = LeftLung.GetDamagedSegmentsIndexesBy(rightLungDamageDescription);
                foreach (int index in damageIndexes)
                    LeftLung.SegmentsIsDamage[index] = true;

                LungDamages lungDamage = LeftLung.GetLungDamageBy(damageVolumeDescription);
                LeftLung.LungDamage = lungDamage;
            }
        }
    }


    public class LeftLung: Lung
    {
      
        public LeftLung()
        {
            InitLung();
        }

        public override void InitLung()
        {
            SegmentsIsDamage = new bool[10];
            LungDamage = LungDamages.No;
            FractionIndexes[LungFractions.Upper] = new int[] { 0, 1, 2, 3, 4 };
            FractionIndexes[LungFractions.Lower] = new int[] { 5, 6, 7, 8, 9 };
        }

    }

    public class RightLung: Lung
    {
        public RightLung()
        {
            InitLung();
        }

        public override void InitLung()
        {
            SegmentsIsDamage = new bool[10];
            LungDamage = LungDamages.No;
            FractionIndexes[LungFractions.Upper] = new int[] { 0, 1, 2};
            FractionIndexes[LungFractions.Middle] = new int[] { 3, 4 };
            FractionIndexes[LungFractions.Lower] = new int[] { 5, 6, 7, 8, 9 };
        }
    }


    public enum LungFractions
    {
        [Display(Name = "Верхняя доля")]
        Upper,
        [Display(Name = "Средняя доля")]
        Middle,
        [Display(Name = "Нижняя доля")]
        Lower
    }


    public enum LungDamages
    {
        [Display(Name = "Нет повреждений")]
        No,
        [Display(Name = "КТ1")]
        CT1,
        [Display(Name = "КТ2")]
        CT2,
        [Display(Name = "КТ3")]
        CT3,
        [Display(Name = "КТ4")]
        CT4
    }
}
