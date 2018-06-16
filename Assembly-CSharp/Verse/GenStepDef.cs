using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B40 RID: 2880
	public class GenStepDef : Def
	{
		// Token: 0x06003F32 RID: 16178 RVA: 0x002140BC File Offset: 0x002124BC
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}

		// Token: 0x0400296D RID: 10605
		public SiteDefBase linkWithSite;

		// Token: 0x0400296E RID: 10606
		public float order;

		// Token: 0x0400296F RID: 10607
		public GenStep genStep;
	}
}
