using System;

namespace Verse
{
	// Token: 0x02000BBB RID: 3003
	public abstract class WorldGenStep
	{
		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06004101 RID: 16641
		public abstract int SeedPart { get; }

		// Token: 0x06004102 RID: 16642
		public abstract void GenerateFresh(string seed);

		// Token: 0x06004103 RID: 16643 RVA: 0x000F257D File Offset: 0x000F097D
		public virtual void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFresh(seed);
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x000F2587 File Offset: 0x000F0987
		public virtual void GenerateFromScribe(string seed)
		{
		}

		// Token: 0x04002C6F RID: 11375
		public WorldGenStepDef def;
	}
}
