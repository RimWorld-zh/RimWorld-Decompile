using System;

namespace Verse
{
	// Token: 0x02000B40 RID: 2880
	public abstract class GenStep
	{
		// Token: 0x04002974 RID: 10612
		public GenStepDef def;

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06003F38 RID: 16184
		public abstract int SeedPart { get; }

		// Token: 0x06003F39 RID: 16185
		public abstract void Generate(Map map);
	}
}
