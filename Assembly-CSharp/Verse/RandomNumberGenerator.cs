using System;

namespace Verse
{
	// Token: 0x02000FAD RID: 4013
	public abstract class RandomNumberGenerator
	{
		// Token: 0x06006101 RID: 24833
		public abstract int GetInt(uint iterations);

		// Token: 0x06006102 RID: 24834 RVA: 0x0030E50C File Offset: 0x0030C90C
		public float GetFloat(uint iterations)
		{
			return (float)(((double)this.GetInt(iterations) - -2147483648.0) / 4294967295.0);
		}

		// Token: 0x04003F70 RID: 16240
		public uint seed = (uint)DateTime.Now.GetHashCode();
	}
}
