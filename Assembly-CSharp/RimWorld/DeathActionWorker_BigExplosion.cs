using Verse;

namespace RimWorld
{
	public class DeathActionWorker_BigExplosion : DeathActionWorker
	{
		public override void PawnDied(Corpse corpse)
		{
			float radius = (float)((corpse.InnerPawn.ageTracker.CurLifeStageIndex != 0) ? ((corpse.InnerPawn.ageTracker.CurLifeStageIndex != 1) ? 4.9000000953674316 : 2.9000000953674316) : 1.8999999761581421);
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, DamageDefOf.Flame, corpse.InnerPawn, null, null, null, null, 0f, 1, false, null, 0f, 1);
		}
	}
}
