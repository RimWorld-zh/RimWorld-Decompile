using System;

namespace Verse
{
	// Token: 0x02000BBA RID: 3002
	public abstract class WorldGenStep
	{
		// Token: 0x04002C7B RID: 11387
		public WorldGenStepDef def;

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06004106 RID: 16646
		public abstract int SeedPart { get; }

		// Token: 0x06004107 RID: 16647
		public abstract void GenerateFresh(string seed);

		// Token: 0x06004108 RID: 16648 RVA: 0x000F2989 File Offset: 0x000F0D89
		public virtual void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFresh(seed);
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x000F2993 File Offset: 0x000F0D93
		public virtual void GenerateFromScribe(string seed)
		{
		}
	}
}
