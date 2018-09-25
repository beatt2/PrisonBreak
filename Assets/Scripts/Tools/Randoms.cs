using System;

namespace Tools
{
    public static class Randoms 
    {
        public static int[] GiveRandoms(int amountOfRandoms, int lengtCount)
        {
            int[] randoms = new int[amountOfRandoms];
            // ReSharper disable once RedundantNameQualifier
            System.Random random = new Random();
            for (int i = 0; i < amountOfRandoms; i++)
            {
                randoms[i] = random.Next(0, lengtCount);
            }
            return randoms;
        }
    }
}
