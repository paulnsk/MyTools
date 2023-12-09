using System;
using System.Collections.Generic;
using System.Text;

namespace MyTools
{
    public static class MathUtils
    {

        private static readonly Random Rnd = new() ;

        /// <summary>
        /// Returns true with specified probability (e.g. if probability is 0.8 then true is returned 4 of 5 times)
        /// </summary>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static bool WithProbability(double probability)
        {
            if (probability <= 0 || probability > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(probability), "Probability must be between 0 and 1");
            }

            return Rnd.NextDouble() <= probability;
        }
    }
}
