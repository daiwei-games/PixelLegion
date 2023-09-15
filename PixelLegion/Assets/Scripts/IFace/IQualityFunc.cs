using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.IFace
{
    public interface IQualityFunc
    {
        /// <summary>
        /// 素質初始化
        /// </summary>
        /// 
        /// <returns>返回初始化之後的素質</returns>
        public int QualityInitialization(float quality);

        /// <summary>
        /// 素質範圍計算
        /// </summary>
        /// <param name="quality">素質</param>
        /// <returns>返回計算之後的素質</returns>
        public int QualityRangeCalculation(float quality);
    }
}
