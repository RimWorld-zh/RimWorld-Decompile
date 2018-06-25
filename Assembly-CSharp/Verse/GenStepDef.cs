using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B3E RID: 2878
	public class GenStepDef : Def
	{
		// Token: 0x0400296A RID: 10602
		public SiteDefBase linkWithSite;

		// Token: 0x0400296B RID: 10603
		public float order;

		// Token: 0x0400296C RID: 10604
		public GenStep genStep;

		// Token: 0x06003F36 RID: 16182 RVA: 0x00214884 File Offset: 0x00212C84
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}
	}
}
