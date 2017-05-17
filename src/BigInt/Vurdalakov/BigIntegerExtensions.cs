namespace BigInt
{
    using System;
    using System.Numerics;

    public static class BigIntegerExtensions
    {
        public static Int32 GetBitCount(this BigInteger bigInteger)
        {
            var bitCount = 0;

            while (bigInteger != 0)
            {
                bitCount++;
                bigInteger >>= 1;
            }

            return bitCount;
        }
    }
}
