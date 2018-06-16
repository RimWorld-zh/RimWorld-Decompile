using System;

namespace Verse
{
	// Token: 0x02000CFD RID: 3325
	public class DamageWorker_AddGlobal : DamageWorker
	{
		// Token: 0x06004922 RID: 18722 RVA: 0x002665FC File Offset: 0x002649FC
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				Hediff hediff = HediffMaker.MakeHediff(dinfo.Def.hediff, pawn, null);
				hediff.Severity = dinfo.Amount;
				pawn.health.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
			}
			return new DamageWorker.DamageResult();
		}
	}
}
