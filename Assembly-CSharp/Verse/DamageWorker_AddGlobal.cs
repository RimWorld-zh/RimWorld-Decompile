using System;

namespace Verse
{
	// Token: 0x02000CFC RID: 3324
	public class DamageWorker_AddGlobal : DamageWorker
	{
		// Token: 0x06004920 RID: 18720 RVA: 0x002665D4 File Offset: 0x002649D4
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
