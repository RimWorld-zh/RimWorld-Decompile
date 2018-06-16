using System;

namespace Verse
{
	// Token: 0x02000BBA RID: 3002
	public class WorldGenStepDef : Def
	{
		// Token: 0x060040FD RID: 16637 RVA: 0x00224E8A File Offset: 0x0022328A
		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}

		// Token: 0x04002C6D RID: 11373
		public float order;

		// Token: 0x04002C6E RID: 11374
		public WorldGenStep worldGenStep;
	}
}
