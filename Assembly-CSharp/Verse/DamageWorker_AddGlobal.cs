namespace Verse
{
	public class DamageWorker_AddGlobal : DamageWorker
	{
		public override DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				Hediff hediff = HediffMaker.MakeHediff(dinfo.Def.hediff, pawn, null);
				hediff.Severity = (float)dinfo.Amount;
				pawn.health.AddHediff(hediff, null, dinfo);
			}
			return DamageResult.MakeNew();
		}
	}
}
