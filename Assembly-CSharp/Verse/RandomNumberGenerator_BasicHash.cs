using System;

namespace Verse
{
	// Token: 0x02000FAF RID: 4015
	public class RandomNumberGenerator_BasicHash : RandomNumberGenerator
	{
		// Token: 0x04003F7C RID: 16252
		private const uint Prime1 = 2654435761u;

		// Token: 0x04003F7D RID: 16253
		private const uint Prime2 = 2246822519u;

		// Token: 0x04003F7E RID: 16254
		private const uint Prime3 = 3266489917u;

		// Token: 0x04003F7F RID: 16255
		private const uint Prime4 = 668265263u;

		// Token: 0x04003F80 RID: 16256
		private const uint Prime5 = 374761393u;

		// Token: 0x06006109 RID: 24841 RVA: 0x00310C6C File Offset: 0x0030F06C
		public override int GetInt(uint iterations)
		{
			return (int)this.GetHash((int)iterations);
		}

		// Token: 0x0600610A RID: 24842 RVA: 0x00310C88 File Offset: 0x0030F088
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

		// Token: 0x0600610B RID: 24843 RVA: 0x00310CF0 File Offset: 0x0030F0F0
		private static uint Rotate(uint value, int count)
		{
			return value << count | value >> 32 - count;
		}
	}
}
