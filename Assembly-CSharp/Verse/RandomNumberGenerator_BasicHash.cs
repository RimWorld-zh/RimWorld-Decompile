namespace Verse
{
	public class RandomNumberGenerator_BasicHash : RandomNumberGenerator
	{
		private const uint Prime1 = 2654435761u;

		private const uint Prime2 = 2246822519u;

		private const uint Prime3 = 3266489917u;

		private const uint Prime4 = 668265263u;

		private const uint Prime5 = 374761393u;

		public override int GetInt(uint iterations)
		{
			return (int)this.GetHash((int)iterations);
		}

		private uint GetHash(int buffer)
		{
			uint num = base.seed + 374761393;
			num += 4;
			num = (uint)((int)num + buffer * -1028477379);
			num = RandomNumberGenerator_BasicHash.Rotate(num, 17) * 668265263;
			num ^= num >> 15;
			num = (uint)((int)num * -2048144777);
			num ^= num >> 13;
			num = (uint)((int)num * -1028477379);
			return num ^ num >> 16;
		}

		private static uint Rotate(uint value, int count)
		{
			return value << count | value >> 32 - count;
		}
	}
}
