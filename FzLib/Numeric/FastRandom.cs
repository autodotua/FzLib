using System;

namespace FzLib.Numeric
{
    public class FastRandom
    {
        public FastRandom(uint state)
        {
            CurrentUInt32State = state;
            CurrentUInt64State = state;
        }

        public FastRandom(ulong state)
        {
            CurrentUInt32State = (uint)(state % uint.MaxValue);
            CurrentUInt64State = state;
        }

        public FastRandom()
        {
            Random r = new Random();
            CurrentUInt32State = (uint)r.Next();
            CurrentUInt64State = (uint)r.Next();
        }

        public uint CurrentUInt32State { get; private set; }

        public ulong CurrentUInt64State { get; private set; }

        public uint GetUInt32()
        {
            /* Algorithm "xor" from p. 4 of Marsaglia, "Xorshift RNGs" */
            uint x = CurrentUInt32State;
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 5;
            CurrentUInt32State = x;
            return x;
        }

        public ulong GetUInt64()
        {
            ulong x = CurrentUInt64State;
            x ^= x << 13;
            x ^= x >> 7;
            x ^= x << 17;
            CurrentUInt64State = x;
            return x;
        }
    }

}