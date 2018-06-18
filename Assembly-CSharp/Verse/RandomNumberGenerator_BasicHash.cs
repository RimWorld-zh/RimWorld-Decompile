using System;

namespace Verse
{
	// Token: 0x02000FAB RID: 4011
	public class RandomNumberGenerator_BasicHash : RandomNumberGenerator
	{
		// Token: 0x060060D6 RID: 24790 RVA: 0x0030E548 File Offset: 0x0030C948
		public override int GetInt(uint iterations)
		{
			return (int)this.GetHash((int)iterations);
		}

		// Token: 0x060060D7 RID: 24791 RVA: 0x0030E564 File Offset: 0x0030C964
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

		// Token: 0x060060D8 RID: 24792 RVA: 0x0030E5CC File Offset: 0x0030C9CC
		private static uint Rotate(uint value, int count)
		{
			return value << count | value >> 32 - count;
		}

		// Token: 0x04003F67 RID: 16231
		private const uint Prime1 = 2654435761u;

		// Token: 0x04003F68 RID: 16232
		private const uint Prime2 = 2246822519u;

		// Token: 0x04003F69 RID: 16233
		private const uint Prime3 = 3266489917u;

		// Token: 0x04003F6A RID: 16234
		private const uint Prime4 = 668265263u;

		// Token: 0x04003F6B RID: 16235
		private const uint Prime5 = 374761393u;
	}
}
