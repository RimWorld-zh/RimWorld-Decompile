using System;

namespace Verse
{
	// Token: 0x02000D2C RID: 3372
	public class Hediff_Implant : HediffWithComps
	{
		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06004A32 RID: 18994 RVA: 0x0026ADB4 File Offset: 0x002691B4
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004A33 RID: 18995 RVA: 0x0026ADCA File Offset: 0x002691CA
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (base.Part == null)
			{
				Log.Error("Part is null. It should be set before PostAdd for " + this.def + ".", false);
			}
		}
	}
}
