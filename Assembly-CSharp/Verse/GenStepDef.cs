using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B3F RID: 2879
	public class GenStepDef : Def
	{
		// Token: 0x04002971 RID: 10609
		public SiteDefBase linkWithSite;

		// Token: 0x04002972 RID: 10610
		public float order;

		// Token: 0x04002973 RID: 10611
		public GenStep genStep;

		// Token: 0x06003F36 RID: 16182 RVA: 0x00214B64 File Offset: 0x00212F64
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}
	}
}
