using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000DCA RID: 3530
	public class Explosion : Thing
	{
		// Token: 0x06004EE2 RID: 20194 RVA: 0x00291D84 File Offset: 0x00290184
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

		// Token: 0x06004EE3 RID: 20195 RVA: 0x00291DE4 File Offset: 0x002901E4
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

		// Token: 0x06004EE4 RID: 20196 RVA: 0x00291E50 File Offset: 0x00290250
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

		// Token: 0x06004EE5 RID: 20197 RVA: 0x00291F7C File Offset: 0x0029037C
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

		// Token: 0x06004EE6 RID: 20198 RVA: 0x00292068 File Offset: 0x00290468
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

		// Token: 0x06004EE7 RID: 20199 RVA: 0x002920D0 File Offset: 0x002904D0
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

		// Token: 0x06004EE8 RID: 20200 RVA: 0x00292290 File Offset: 0x00290690
		private int GetCellAffectTick(IntVec3 cell)
		{
			return this.startTick + (int)((cell - base.Position).LengthHorizontal * 1.5f);
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x002922C8 File Offset: 0x002906C8
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

		// Token: 0x06004EEA RID: 20202 RVA: 0x002923E8 File Offset: 0x002907E8
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

		// Token: 0x06004EEB RID: 20203 RVA: 0x00292440 File Offset: 0x00290840
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

		// Token: 0x06004EEC RID: 20204 RVA: 0x002924C0 File Offset: 0x002908C0
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

		// Token: 0x06004EED RID: 20205 RVA: 0x00292614 File Offset: 0x00290A14
		private bool ShouldCellBeAffectedOnlyByDamage(IntVec3 c)
		{
			return this.applyDamageToExplosionCellsNeighbors && this.addedCellsAffectedOnlyByDamage.Contains(c);
		}

		// Token: 0x04003469 RID: 13417
		public float radius;

		// Token: 0x0400346A RID: 13418
		public DamageDef damType;

		// Token: 0x0400346B RID: 13419
		public int damAmount;

		// Token: 0x0400346C RID: 13420
		public Thing instigator;

		// Token: 0x0400346D RID: 13421
		public ThingDef weapon;

		// Token: 0x0400346E RID: 13422
		public ThingDef projectile;

		// Token: 0x0400346F RID: 13423
		public Thing intendedTarget;

		// Token: 0x04003470 RID: 13424
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04003471 RID: 13425
		public ThingDef preExplosionSpawnThingDef = null;

		// Token: 0x04003472 RID: 13426
		public float preExplosionSpawnChance = 0f;

		// Token: 0x04003473 RID: 13427
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04003474 RID: 13428
		public ThingDef postExplosionSpawnThingDef = null;

		// Token: 0x04003475 RID: 13429
		public float postExplosionSpawnChance = 0f;

		// Token: 0x04003476 RID: 13430
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04003477 RID: 13431
		public float chanceToStartFire;

		// Token: 0x04003478 RID: 13432
		public bool damageFalloff;

		// Token: 0x04003479 RID: 13433
		private int startTick;

		// Token: 0x0400347A RID: 13434
		private List<IntVec3> cellsToAffect;

		// Token: 0x0400347B RID: 13435
		private List<Thing> damagedThings;

		// Token: 0x0400347C RID: 13436
		private HashSet<IntVec3> addedCellsAffectedOnlyByDamage;

		// Token: 0x0400347D RID: 13437
		private const float DamageFactorAtEdge = 0.2f;

		// Token: 0x0400347E RID: 13438
		private static HashSet<IntVec3> tmpCells = new HashSet<IntVec3>();
	}
}
