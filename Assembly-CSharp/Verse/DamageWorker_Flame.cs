using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CF4 RID: 3316
	public class DamageWorker_Flame : DamageWorker_AddInjury
	{
		// Token: 0x06004924 RID: 18724 RVA: 0x002672C0 File Offset: 0x002656C0
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			Pawn pawn = victim as Pawn;
			if (pawn != null && pawn.Faction == Faction.OfPlayer)
			{
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			Map map = victim.Map;
			DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
			if (!damageResult.deflected && !dinfo.InstantPermanentInjury)
			{
				victim.TryAttachFire(Rand.Range(0.15f, 0.25f));
			}
			if (victim.Destroyed && map != null && pawn == null)
			{
				foreach (IntVec3 c in victim.OccupiedRect())
				{
					FilthMaker.MakeFilth(c, map, ThingDefOf.Filth_Ash, 1);
				}
				Plant plant = victim as Plant;
				if (plant != null && victim.def.plant.IsTree && plant.LifeStage != PlantLifeStage.Sowing && victim.def != ThingDefOf.BurnedTree)
				{
					DeadPlant deadPlant = (DeadPlant)GenSpawn.Spawn(ThingDefOf.BurnedTree, victim.Position, map, WipeMode.Vanish);
					deadPlant.Growth = plant.Growth;
				}
			}
			return damageResult;
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x00267420 File Offset: 0x00265820
		public override void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, bool canThrowMotes)
		{
			base.ExplosionAffectCell(explosion, c, damagedThings, canThrowMotes);
			if (this.def == DamageDefOf.Flame && Rand.Chance(FireUtility.ChanceToStartFireIn(c, explosion.Map)))
			{
				FireUtility.TryStartFireIn(c, explosion.Map, Rand.Range(0.2f, 0.6f));
			}
		}
	}
}
