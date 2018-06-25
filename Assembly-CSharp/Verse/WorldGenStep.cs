using System;

namespace Verse
{
	// Token: 0x02000BB9 RID: 3001
	public abstract class WorldGenStep
	{
		// Token: 0x04002C74 RID: 11380
		public WorldGenStepDef def;

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06004106 RID: 16646
		public abstract int SeedPart { get; }

		// Token: 0x06004107 RID: 16647
		public abstract void GenerateFresh(string seed);

		// Token: 0x06004108 RID: 16648 RVA: 0x000F2721 File Offset: 0x000F0B21
		public virtual void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFresh(seed);
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x000F272B File Offset: 0x000F0B2B
		public virtual void GenerateFromScribe(string seed)
		{
		}
	}
}
