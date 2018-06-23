using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000DC7 RID: 3527
	public class Explosion : Thing
	{
		// Token: 0x04003474 RID: 13428
		public float radius;

		// Token: 0x04003475 RID: 13429
		public DamageDef damType;

		// Token: 0x04003476 RID: 13430
		public int damAmount;

		// Token: 0x04003477 RID: 13431
		public Thing instigator;

		// Token: 0x04003478 RID: 13432
		public ThingDef weapon;

		// Token: 0x04003479 RID: 13433
		public ThingDef projectile;

		// Token: 0x0400347A RID: 13434
		public Thing intendedTarget;

		// Token: 0x0400347B RID: 13435
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x0400347C RID: 13436
		public ThingDef preExplosionSpawnThingDef = null;

		// Token: 0x0400347D RID: 13437
		public float preExplosionSpawnChance = 0f;

		// Token: 0x0400347E RID: 13438
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x0400347F RID: 13439
		public ThingDef postExplosionSpawnThingDef = null;

		// Token: 0x04003480 RID: 13440
		public float postExplosionSpawnChance = 0f;

		// Token: 0x04003481 RID: 13441
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04003482 RID: 13442
		public float chanceToStartFire;

		// Token: 0x04003483 RID: 13443
		public bool damageFalloff;

		// Token: 0x04003484 RID: 13444
		private int startTick;

		// Token: 0x04003485 RID: 13445
		private List<IntVec3> cellsToAffect;

		// Token: 0x04003486 RID: 13446
		private List<Thing> damagedThings;

		// Token: 0x04003487 RID: 13447
		private HashSet<IntVec3> addedCellsAffectedOnlyByDamage;

		// Token: 0x04003488 RID: 13448
		private const float DamageFactorAtEdge = 0.2f;

		// Token: 0x04003489 RID: 13449
		private static HashSet<IntVec3> tmpCells = new HashSet<IntVec3>();

		// Token: 0x06004EF7 RID: 20215 RVA: 0x00293360 File Offset: 0x00291760
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.cellsToAffect = SimplePool<List<IntVec3>>.Get();
				this.cellsToAffect.Clear();
				this.damagedThings = SimplePool<List<Thing>>.Get();
				this.damagedThings.Clear();
				this.addedCellsAffectedOnlyByDamage = SimplePool<HashSet<IntVec3>>.Get();
				this.addedCellsAffectedOnlyByDamage.Clear();
			}
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x002933C0 File Offset: 0x002917C0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.cellsToAffect.Clear();
			SimplePool<List<IntVec3>>.Return(this.cellsToAffect);
			this.cellsToAffect = null;
			this.damagedThings.Clear();
			SimplePool<List<Thing>>.Return(this.damagedThings);
			this.damagedThings = null;
			this.addedCellsAffectedOnlyByDamage.Clear();
			SimplePool<HashSet<IntVec3>>.Return(this.addedCellsAffectedOnlyByDamage);
			this.addedCellsAffectedOnlyByDamage = null;
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x0029342C File Offset: 0x0029182C
		public virtual void StartExplosion(SoundDef explosionSound)
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartExplosion() on unspawned thing.", false);
			}
			else
			{
				this.startTick = Find.TickManager.TicksGame;
				this.cellsToAffect.Clear();
				this.damagedThings.Clear();
				this.addedCellsAffectedOnlyByDamage.Clear();
				this.cellsToAffect.AddRange(this.damType.Worker.ExplosionCellsToHit(this));
				if (this.applyDamageToExplosionCellsNeighbors)
				{
					this.AddCellsNeighbors(this.cellsToAffect);
				}
				this.damType.Worker.ExplosionStart(this, this.cellsToAffect);
				this.PlayExplosionSound(explosionSound);
				MoteMaker.MakeWaterSplash(base.Position.ToVector3Shifted(), base.Map, this.radius * 6f, 20f);
				this.cellsToAffect.Sort((IntVec3 a, IntVec3 b) => this.GetCellAffectTick(b).CompareTo(this.GetCellAffectTick(a)));
				RegionTraverser.BreadthFirstTraverse(base.Position, base.Map, (Region from, Region to) => true, delegate(Region x)
				{
					List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
					for (int i = list.Count - 1; i >= 0; i--)
					{
						((Pawn)list[i]).mindState.Notify_Explosion(this);
					}
					return false;
				}, 25, RegionType.Set_Passable);
			}
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x00293558 File Offset: 0x00291958
		public override void Tick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			int count = this.cellsToAffect.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				if (ticksGame < this.GetCellAffectTick(this.cellsToAffect[i]))
				{
					break;
				}
				try
				{
					this.AffectCell(this.cellsToAffect[i]);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Explosion could not affect cell ",
						this.cellsToAffect[i],
						": ",
						ex
					}), false);
				}
				this.cellsToAffect.RemoveAt(i);
			}
			if (!this.cellsToAffect.Any<IntVec3>())
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x00293644 File Offset: 0x00291A44
		public int GetDamageAmountAt(IntVec3 c)
		{
			int result;
			if (!this.damageFalloff)
			{
				result = this.damAmount;
			}
			else
			{
				float t = c.DistanceTo(base.Position) / this.radius;
				int a = GenMath.RoundRandom(Mathf.Lerp((float)this.damAmount, (float)this.damAmount * 0.2f, t));
				result = Mathf.Max(a, 1);
			}
			return result;
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x002936AC File Offset: 0x00291AAC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damType, "damType");
			Scribe_Values.Look<int>(ref this.damAmount, "damAmount", 0, false);
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weapon, "weapon");
			Scribe_Defs.Look<ThingDef>(ref this.projectile, "projectile");
			Scribe_References.Look<Thing>(ref this.intendedTarget, "intendedTarget", false);
			Scribe_Values.Look<bool>(ref this.applyDamageToExplosionCellsNeighbors, "applyDamageToExplosionCellsNeighbors", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.preExplosionSpawnThingDef, "preExplosionSpawnThingDef");
			Scribe_Values.Look<float>(ref this.preExplosionSpawnChance, "preExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.preExplosionSpawnThingCount, "preExplosionSpawnThingCount", 1, false);
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnThingDef, "postExplosionSpawnThingDef");
			Scribe_Values.Look<float>(ref this.postExplosionSpawnChance, "postExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.postExplosionSpawnThingCount, "postExplosionSpawnThingCount", 1, false);
			Scribe_Values.Look<float>(ref this.chanceToStartFire, "chanceToStartFire", 0f, false);
			Scribe_Values.Look<bool>(ref this.damageFalloff, "dealMoreDamageAtCenter", false, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Collections.Look<IntVec3>(ref this.cellsToAffect, "cellsToAffect", LookMode.Value, new object[0]);
			Scribe_Collections.Look<Thing>(ref this.damagedThings, "damagedThings", LookMode.Reference, new object[0]);
			Scribe_Collections.Look<IntVec3>(ref this.addedCellsAffectedOnlyByDamage, "addedCellsAffectedOnlyByDamage", LookMode.Value);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.damagedThings.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x0029386C File Offset: 0x00291C6C
		private int GetCellAffectTick(IntVec3 cell)
		{
			return this.startTick + (int)((cell - base.Position).LengthHorizontal * 1.5f);
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x002938A4 File Offset: 0x00291CA4
		private void AffectCell(IntVec3 c)
		{
			if (c.InBounds(base.Map))
			{
				bool flag = this.ShouldCellBeAffectedOnlyByDamage(c);
				if (!flag)
				{
					if (Rand.Chance(this.preExplosionSpawnChance) && c.Walkable(base.Map))
					{
						this.TrySpawnExplosionThing(this.preExplosionSpawnThingDef, c, this.preExplosionSpawnThingCount);
					}
				}
				this.damType.Worker.ExplosionAffectCell(this, c, this.damagedThings, !flag);
				if (!flag)
				{
					if (Rand.Chance(this.postExplosionSpawnChance) && c.Walkable(base.Map))
					{
						this.TrySpawnExplosionThing(this.postExplosionSpawnThingDef, c, this.postExplosionSpawnThingCount);
					}
				}
				float num = this.chanceToStartFire;
				if (this.damageFalloff)
				{
					num *= Mathf.Lerp(1f, 0.2f, c.DistanceTo(base.Position) / this.radius);
				}
				if (Rand.Chance(num))
				{
					FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f));
				}
			}
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x002939C4 File Offset: 0x00291DC4
		private void TrySpawnExplosionThing(ThingDef thingDef, IntVec3 c, int count)
		{
			if (thingDef != null)
			{
				if (thingDef.IsFilth)
				{
					FilthMaker.MakeFilth(c, base.Map, thingDef, count);
				}
				else
				{
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = count;
					GenSpawn.Spawn(thing, c, base.Map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x00293A1C File Offset: 0x00291E1C
		private void PlayExplosionSound(SoundDef explosionSound)
		{
			bool flag;
			if (Prefs.DevMode)
			{
				flag = (explosionSound != null);
			}
			else
			{
				flag = !explosionSound.NullOrUndefined();
			}
			if (flag)
			{
				explosionSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			else
			{
				this.damType.soundExplosion.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x00293A9C File Offset: 0x00291E9C
		private void AddCellsNeighbors(List<IntVec3> cells)
		{
			Explosion.tmpCells.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				Explosion.tmpCells.Add(cells[i]);
			}
			for (int j = 0; j < cells.Count; j++)
			{
				if (cells[j].Walkable(base.Map))
				{
					for (int k = 0; k < GenAdj.AdjacentCells.Length; k++)
					{
						IntVec3 intVec = cells[j] + GenAdj.AdjacentCells[k];
						if (intVec.InBounds(base.Map))
						{
							bool flag = Explosion.tmpCells.Add(intVec);
							if (flag)
							{
								this.addedCellsAffectedOnlyByDamage.Add(intVec);
							}
						}
					}
				}
			}
			cells.Clear();
			foreach (IntVec3 item in Explosion.tmpCells)
			{
				cells.Add(item);
			}
			Explosion.tmpCells.Clear();
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x00293BF0 File Offset: 0x00291FF0
		private bool ShouldCellBeAffectedOnlyByDamage(IntVec3 c)
		{
			return this.applyDamageToExplosionCellsNeighbors && this.addedCellsAffectedOnlyByDamage.Contains(c);
		}
	}
}
