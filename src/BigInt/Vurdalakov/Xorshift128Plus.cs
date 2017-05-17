namespace BigInt
{
    using System;
    using System.Numerics;

    public class Xorshift128Plus
    {
        private UInt64 _state0;
        private UInt64 _state1;
        private UInt64 _count = 0;

        public Xorshift128Plus()
        {
            Seed(0);
        }

        public void Seed(UInt64 seed)
        {
            if (0 == seed)
            {
                seed = (UInt64)DateTime.Now.Ticks;

                seed += _count;
                _count++;
            }

            this._state0 = seed;
            this._state1 = ~seed;

            var count = (Int32)(seed & 0x7F) + 15;
            for (var i = 0; i < count; i++)
            {
                this.Next();
            }
        }

        public UInt64 Next()
        {
            var x = this._state0;
            var y = this._state1;
            this._state0 = y;
            x ^= x << 23;
            this._state1 = x ^ y ^ (x >> 17) ^ (y >> 26);
            return this._state1 + y;
        }

        public BigInteger Next(Int32 bitCount)
        {
            BigInteger bigInteger = 0;

            for (var i = 0; i < bitCount / 64; i++)
            {
                bigInteger <<= 64;
                bigInteger |= Next();
            }

            var bitsLeft = bitCount % 64;
            if (bitsLeft > 0)
            {
                bigInteger <<= bitsLeft;
                var mask = ((UInt64)1 << bitsLeft) - 1;
                bigInteger |= (Next() & mask);
            }

            return bigInteger;
        }
    }
}
