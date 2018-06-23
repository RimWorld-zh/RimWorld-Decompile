using System;

namespace Verse
{
	// Token: 0x02000BB6 RID: 2998
	public class WorldGenStepDef : Def
	{
		// Token: 0x04002C72 RID: 11378
		public float order;

		// Token: 0x04002C73 RID: 11379
		public WorldGenStep worldGenStep;

		// Token: 0x06004101 RID: 16641 RVA: 0x00225632 File Offset: 0x00223A32
		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}
	}
}
