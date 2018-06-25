using System;

namespace Verse
{
	// Token: 0x02000B3F RID: 2879
	public abstract class GenStep
	{
		// Token: 0x0400296D RID: 10605
		public GenStepDef def;

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06003F38 RID: 16184
		public abstract int SeedPart { get; }

		// Token: 0x06003F39 RID: 16185
		public abstract void Generate(Map map);
	}
}
