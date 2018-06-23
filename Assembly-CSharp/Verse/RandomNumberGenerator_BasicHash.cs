using System;

namespace Verse
{
	// Token: 0x02000FAB RID: 4011
	public class RandomNumberGenerator_BasicHash : RandomNumberGenerator
	{
		// Token: 0x04003F79 RID: 16249
		private const uint Prime1 = 2654435761u;

		// Token: 0x04003F7A RID: 16250
		private const uint Prime2 = 2246822519u;

		// Token: 0x04003F7B RID: 16251
		private const uint Prime3 = 3266489917u;

		// Token: 0x04003F7C RID: 16252
		private const uint Prime4 = 668265263u;

		// Token: 0x04003F7D RID: 16253
		private const uint Prime5 = 374761393u;

		// Token: 0x060060FF RID: 24831 RVA: 0x003105EC File Offset: 0x0030E9EC
		public override int GetInt(uint iterations)
		{
			return (int)this.GetHash((int)iterations);
		}

		// Token: 0x06006100 RID: 24832 RVA: 0x00310608 File Offset: 0x0030EA08
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

		// Token: 0x06006101 RID: 24833 RVA: 0x00310670 File Offset: 0x0030EA70
		private static uint Rotate(uint value, int count)
		{
			return value << count | value >> 32 - count;
		}
	}
}
