using System;

namespace Verse
{
	// Token: 0x02000BB9 RID: 3001
	public class WorldGenStepDef : Def
	{
		// Token: 0x04002C79 RID: 11385
		public float order;

		// Token: 0x04002C7A RID: 11386
		public WorldGenStep worldGenStep;

		// Token: 0x06004104 RID: 16644 RVA: 0x002259EE File Offset: 0x00223DEE
		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}
	}
}
