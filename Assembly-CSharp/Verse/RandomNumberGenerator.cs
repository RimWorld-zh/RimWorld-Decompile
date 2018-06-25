using System;

namespace Verse
{
	// Token: 0x02000FB1 RID: 4017
	public abstract class RandomNumberGenerator
	{
		// Token: 0x04003F85 RID: 16261
		public uint seed = (uint)DateTime.Now.GetHashCode();

		// Token: 0x06006134 RID: 24884
		public abstract int GetInt(uint iterations);

		// Token: 0x06006135 RID: 24885 RVA: 0x00310C30 File Offset: 0x0030F030
		public float GetFloat(uint iterations)
		{
			return (float)(((double)this.GetInt(iterations) - -2147483648.0) / 4294967295.0);
		}
	}
}
