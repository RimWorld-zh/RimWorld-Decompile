using System;

namespace Verse
{
	// Token: 0x02000BB8 RID: 3000
	public class WorldGenStepDef : Def
	{
		// Token: 0x04002C72 RID: 11378
		public float order;

		// Token: 0x04002C73 RID: 11379
		public WorldGenStep worldGenStep;

		// Token: 0x06004104 RID: 16644 RVA: 0x0022570E File Offset: 0x00223B0E
		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}
	}
}
