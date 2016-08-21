using System;

namespace Xb
{
    /// <summary>
    /// Numeric functions
    /// 数値関数群
    /// </summary>
    /// <remarks></remarks>
    public class Num
    {
        /// <summary>
        /// Fraction Round Type
        /// 端数処理区分
        /// </summary>
        /// <remarks></remarks>
        public enum RoundType
        {
            /// <summary>
            /// Truncate
            /// 切り捨て
            /// </summary>
            /// <remarks></remarks>
            Floor,

            /// <summary>
            /// Half Round Up
            /// 四捨五入
            /// </summary>
            /// <remarks></remarks>
            HalfUp,

            /// <summary>
            /// Round Up
            /// 切り上げ
            /// </summary>
            /// <remarks></remarks>
            Cell
        }


        /// <summary>
        /// Get Rounded Number
        /// 指定桁数で端数処理する。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="roundType"></param>
        /// <param name="decimalPoint"></param>
        /// <returns></returns>
        public static decimal Round(decimal value, 
                                    RoundType roundType = RoundType.HalfUp, 
                                    int decimalPoint = 0)
        {
            var isReverseRound = (value < 0);

            //get calc-digit
            var digit = Convert.ToDecimal(Math.Pow(10, (decimalPoint)));

            //minus value to plus.
            if (isReverseRound)
                value = Math.Abs(value);

            var result = default(decimal);
            switch (roundType)
            {
                case RoundType.Floor:
                    //Truncate
                    result = Math.Sign(value) 
                             * Math.Floor(Math.Abs(value) * digit) 
                             / digit;
                    break;
                case RoundType.HalfUp:
                    //Half Round Up
                    result = Convert.ToDecimal(Math.Sign(value) 
                                               * Math.Floor((Math.Abs(value) * digit) 
                                                            + (decimal)0.5)
                                               / digit);
                    break;
                case RoundType.Cell:
                    //Round Up
                    result = Convert.ToDecimal(Math.Sign(value) 
                                               * Math.Floor((Math.Abs(value) * digit) 
                                                            + (decimal)0.999999999999999999999999999) 
                                               / digit);
                    break;
                default:
                    throw new ArgumentException("unknown RoundType: " + roundType.ToString());
            }

            //value to minus if passing minus
            if (isReverseRound)
                result = result * -1;

            return result;
        }

        /// <summary>
        /// Validate Number-String
        /// 文字列が数値にキャスト可能か否かを検証する。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsNumeric(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            double tmp;
            return double.TryParse(value, System.Globalization.NumberStyles.Any, null, out tmp);
        }
    }
}
