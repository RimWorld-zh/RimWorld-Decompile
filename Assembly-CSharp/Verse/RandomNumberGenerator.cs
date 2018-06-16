using System;

namespace Verse
{
	// Token: 0x02000FAE RID: 4014
	public abstract class RandomNumberGenerator
	{
		// Token: 0x06006103 RID: 24835
		public abstract int GetInt(uint iterations);

		// Token: 0x06006104 RID: 24836 RVA: 0x0030E430 File Offset: 0x0030C830
		public float GetFloat(uint iterations)
		{
			return (float)(((double)this.GetInt(iterations) - -2147483648.0) / 4294967295.0);
		}

		// Token: 0x04003F71 RID: 16241
		public uint seed = (uint)DateTime.Now.GetHashCode();
	}
}
