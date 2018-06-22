using System;

namespace Verse
{
	// Token: 0x02000FAD RID: 4013
	public abstract class RandomNumberGenerator
	{
		// Token: 0x0600612A RID: 24874
		public abstract int GetInt(uint iterations);

		// Token: 0x0600612B RID: 24875 RVA: 0x003105B0 File Offset: 0x0030E9B0
		public float GetFloat(uint iterations)
		{
			return (float)(((double)this.GetInt(iterations) - -2147483648.0) / 4294967295.0);
		}

		// Token: 0x04003F82 RID: 16258
		public uint seed = (uint)DateTime.Now.GetHashCode();
	}
}
