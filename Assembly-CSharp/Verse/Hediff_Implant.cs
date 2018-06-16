using System;

namespace Verse
{
	// Token: 0x02000D2D RID: 3373
	public class Hediff_Implant : HediffWithComps
	{
		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06004A34 RID: 18996 RVA: 0x0026ADDC File Offset: 0x002691DC
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004A35 RID: 18997 RVA: 0x0026ADF2 File Offset: 0x002691F2
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (base.Part == null)
			{
				Log.Error("Part is null. It should be set before PostAdd for " + this.def + ".", false);
			}
		}
	}
}
