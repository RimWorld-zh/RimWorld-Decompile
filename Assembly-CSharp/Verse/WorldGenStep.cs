using System;

namespace Verse
{
	// Token: 0x02000BBB RID: 3003
	public abstract class WorldGenStep
	{
		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060040FF RID: 16639
		public abstract int SeedPart { get; }

		// Token: 0x06004100 RID: 16640
		public abstract void GenerateFresh(string seed);

		// Token: 0x06004101 RID: 16641 RVA: 0x000F2505 File Offset: 0x000F0905
		public virtual void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFresh(seed);
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x000F250F File Offset: 0x000F090F
		public virtual void GenerateFromScribe(string seed)
		{
		}

		// Token: 0x04002C6F RID: 11375
		public WorldGenStepDef def;
	}
}
