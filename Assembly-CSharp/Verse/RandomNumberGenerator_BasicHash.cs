using System;

namespace Verse
{
	// Token: 0x02000FB0 RID: 4016
	public class RandomNumberGenerator_BasicHash : RandomNumberGenerator
	{
		// Token: 0x04003F84 RID: 16260
		private const uint Prime1 = 2654435761u;

		// Token: 0x04003F85 RID: 16261
		private const uint Prime2 = 2246822519u;

		// Token: 0x04003F86 RID: 16262
		private const uint Prime3 = 3266489917u;

		// Token: 0x04003F87 RID: 16263
		private const uint Prime4 = 668265263u;

		// Token: 0x04003F88 RID: 16264
		private const uint Prime5 = 374761393u;

		// Token: 0x06006109 RID: 24841 RVA: 0x00310EB0 File Offset: 0x0030F2B0
		public override int GetInt(uint iterations)
		{
			return (int)this.GetHash((int)iterations);
		}

		// Token: 0x0600610A RID: 24842 RVA: 0x00310ECC File Offset: 0x0030F2CC
		private uint GetHash(int buffer)
		{
			uint num = this.seed + 374761393u;
			num += 4u;
			num += (uint)(buffer * -1028477379);
			num = RandomNumberGenerator_BasicHash.Rotate(num, 17) * 668265263u;
			num ^= num >> 15;
			num *= 2246822519u;
			num ^= num >> 13;
			num *= 3266489917u;
			return num ^ num >> 16;
		}

		// Token: 0x0600610B RID: 24843 RVA: 0x00310F34 File Offset: 0x0030F334
		private static uint Rotate(uint value, int count)
		{
			return value << count | value >> 32 - count;
		}
	}
}
