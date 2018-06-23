using System;

namespace Verse
{
	// Token: 0x02000BB7 RID: 2999
	public abstract class WorldGenStep
	{
		// Token: 0x04002C74 RID: 11380
		public WorldGenStepDef def;

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06004103 RID: 16643
		public abstract int SeedPart { get; }

		// Token: 0x06004104 RID: 16644
		public abstract void GenerateFresh(string seed);

		// Token: 0x06004105 RID: 16645 RVA: 0x000F25D1 File Offset: 0x000F09D1
		public virtual void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFresh(seed);
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x000F25DB File Offset: 0x000F09DB
		public virtual void GenerateFromScribe(string seed)
		{
		}
	}
}
