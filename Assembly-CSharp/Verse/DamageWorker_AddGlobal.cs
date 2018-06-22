using System;

namespace Verse
{
	// Token: 0x02000CF9 RID: 3321
	public class DamageWorker_AddGlobal : DamageWorker
	{
		// Token: 0x06004931 RID: 18737 RVA: 0x002679EC File Offset: 0x00265DEC
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
