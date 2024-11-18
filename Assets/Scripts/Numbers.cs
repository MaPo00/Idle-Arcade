using UnityEngine;

namespace Utils.Extensions
{
    public static class Numbers
    {
        public static string KorMFormat(this int num)
        {
            return num switch
            {
                >= 100000000 => RoundWithComma(num / 1000000f, 10).ToString("0.#M"),
                >= 1000000 => RoundWithComma(num / 1000000f, 10).ToString("0.##M"),
                >= 100000 => RoundWithComma(num / 1000f, 10).ToString("0.#k"),
                >= 10000 => RoundWithComma(num / 1000f, 10).ToString("0.##k"),
                _ => num.ToString("#,0")
            };
        }

        private static float RoundWithComma(float number, int count) => Mathf.Floor(number * count) / count;
    }
}