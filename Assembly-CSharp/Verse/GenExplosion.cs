using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000F3B RID: 3899
	public static class GenExplosion
	{
		// Token: 0x06005E0C RID: 24076 RVA: 0x002FD68C File Offset: 0x002FBA8C
		public static void DoExplosion(IntVec3 center, Map map, float radius, DamageDef damType, Thing instigator, int damAmount = -1, SoundDef explosionSound = null, ThingDef weapon = null, ThingDef projectile = null, Thing intendedTarget = null, ThingDef postExplosionSpawnThingDef = null, float postExplosionSpawnChance = 0f, int postExplosionSpawnThingCount = 1, bool applyDamageToExplosionCellsNeighbors = false, ThingDef preExplosionSpawnThingDef = null, float preExplosionSpawnChance = 0f, int preExplosionSpawnThingCount = 1, float chanceToStartFire = 0f, bool damageFalloff = false)
		{
			if (map == null)
			{
				Log.Warning("Tried to do explosion in a null map.", false);
			}
			else
			{
				if (damAmount < 0)
				{
					damAmount = damType.defaultDamage;
				}
				if (damAmount < 0)
				{
					Log.ErrorOnce("Attempted to trigger an explosion without defined damage", 91094882, false);
					damAmount = 0;
				}
				Explosion explosion = (Explosion)GenSpawn.Spawn(ThingDefOf.Explosion, center, map, WipeMode.Vanish);
				explosion.radius = radius;
				explosion.damType = damType;
				explosion.instigator = instigator;
				explosion.damAmount = damAmount;
				explosion.weapon = weapon;
				explosion.projectile = projectile;
				explosion.intendedTarget = intendedTarget;
				explosion.preExplosionSpawnThingDef = preExplosionSpawnThingDef;
				explosion.preExplosionSpawnChance = preExplosionSpawnChance;
				explosion.preExplosionSpawnThingCount = preExplosionSpawnThingCount;
				explosion.postExplosionSpawnThingDef = postExplosionSpawnThingDef;
				explosion.postExplosionSpawnChance = postExplosionSpawnChance;
				explosion.postExplosionSpawnThingCount = postExplosionSpawnThingCount;
				explosion.applyDamageToExplosionCellsNeighbors = applyDamageToExplosionCellsNeighbors;
				explosion.chanceToStartFire = chanceToStartFire;
				explosion.damageFalloff = damageFalloff;
				explosion.StartExplosion(explosionSound);
			}
		}

		// Token: 0x06005E0D RID: 24077 RVA: 0x002FD777 File Offset: 0x002FBB77
		public static void RenderPredictedAreaOfEffect(IntVec3 loc, float radius)
		{
			GenDraw.DrawFieldEdges(DamageDefOf.Bomb.Worker.ExplosionCellsToHit(loc, Find.CurrentMap, radius).ToList<IntVec3>());
		}

		// Token: 0x06005E0E RID: 24078 RVA: 0x002FD79C File Offset: 0x002FBB9C
		public static void NotifyNearbyPawnsOfDangerousExplosive(Thing exploder, DamageDef damage, Faction onlyFaction = null)
		{
			if (damage.externalViolence)
			{
				Room room = exploder.GetRoom(RegionType.Set_Passable);
				for (int i = 0; i < GenExplosion.PawnNotifyCellCount; i++)
				{
					IntVec3 c = exploder.Position + GenRadial.RadialPattern[i];
					if (c.InBounds(exploder.Map))
					{
						List<Thing> thingList = c.GetThingList(exploder.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							Pawn pawn = thingList[j] as Pawn;
							if (pawn != null && pawn.RaceProps.intelligence >= Intelligence.Humanlike && (onlyFaction == null || pawn.Faction == onlyFaction))
							{
								Room room2 = pawn.GetRoom(RegionType.Set_Passable);
								if (room2 == null || room2.CellCount == 1 || (room2 == room && GenSight.LineOfSight(exploder.Position, pawn.Position, exploder.Map, true, null, 0, 0)))
								{
									pawn.mindState.Notify_DangerousExploderAboutToExplode(exploder);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04003DFE RID: 15870
		private static readonly int PawnNotifyCellCount = GenRadial.NumCellsInRadius(4.5f);
	}
}
