using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B3C RID: 2876
	public class GenStepDef : Def
	{
		// Token: 0x06003F33 RID: 16179 RVA: 0x002147A8 File Offset: 0x00212BA8
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}

		// Token: 0x0400296A RID: 10602
		public SiteDefBase linkWithSite;

		// Token: 0x0400296B RID: 10603
		public float order;

		// Token: 0x0400296C RID: 10604
		public GenStep genStep;
	}
}
