using System;

namespace Verse
{
	// Token: 0x02000D29 RID: 3369
	public class Hediff_Implant : HediffWithComps
	{
		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06004A44 RID: 19012 RVA: 0x0026C240 File Offset: 0x0026A640
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x0026C256 File Offset: 0x0026A656
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (base.Part == null)
			{
				Log.Error("Part is null. It should be set before PostAdd for " + this.def + ".", false);
			}
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x0026C288 File Offset: 0x0026A688
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
