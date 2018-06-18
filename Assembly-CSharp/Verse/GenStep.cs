using System;

namespace Verse
{
	// Token: 0x02000B41 RID: 2881
	public abstract class GenStep
	{
		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06003F36 RID: 16182
		public abstract int SeedPart { get; }

		// Token: 0x06003F37 RID: 16183
		public abstract void Generate(Map map);

		// Token: 0x04002970 RID: 10608
		public GenStepDef def;
	}
}
