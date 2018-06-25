using System;

namespace Verse
{
	// Token: 0x02000D2B RID: 3371
	public class Hediff_Implant : HediffWithComps
	{
		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06004A48 RID: 19016 RVA: 0x0026C36C File Offset: 0x0026A76C
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x0026C382 File Offset: 0x0026A782
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (base.Part == null)
			{
				Log.Error("Part is null. It should be set before PostAdd for " + this.def + ".", false);
			}
		}

		// Token: 0x06004A4A RID: 19018 RVA: 0x0026C3B4 File Offset: 0x0026A7B4
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_Implant has null part after loading.", false);
				this.pawn.health.hediffSet.hediffs.Remove(this);
			}
		}
	}
}
