using System;

namespace Verse
{
	// Token: 0x02000B3D RID: 2877
	public abstract class GenStep
	{
		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06003F35 RID: 16181
		public abstract int SeedPart { get; }

		// Token: 0x06003F36 RID: 16182
		public abstract void Generate(Map map);

		// Token: 0x0400296D RID: 10605
		public GenStepDef def;
	}
}
