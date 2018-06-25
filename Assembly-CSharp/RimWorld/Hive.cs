using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006C7 RID: 1735
	public class Hive : ThingWithComps
	{
		// Token: 0x040014E2 RID: 5346
		public bool active = true;

		// Token: 0x040014E3 RID: 5347
		public int nextPawnSpawnTick = -1;

		// Token: 0x040014E4 RID: 5348
		private List<Pawn> spawnedPawns = new List<Pawn>();

		// Token: 0x040014E5 RID: 5349
		private int ticksToSpawnInitialPawns = -1;

		// Token: 0x040014E6 RID: 5350
		public bool caveColony = false;

		// Token: 0x040014E7 RID: 5351
		public bool canSpawnPawns = true;

		// Token: 0x040014E8 RID: 5352
		private const int InitialPawnSpawnDelay = 0;

		// Token: 0x040014E9 RID: 5353
		public const int PawnSpawnRadius = 2;

		// Token: 0x040014EA RID: 5354
		public const float MaxSpawnedPawnsPoints = 500f;

		// Token: 0x040014EB RID: 5355
		public const float InitialPawnsPoints = 200f;

		// Token: 0x040014EC RID: 5356
		private static readonly FloatRange PawnSpawnIntervalDays = new FloatRange(0.85f, 1.1f);

		// Token: 0x040014ED RID: 5357
		public static List<PawnKindDef> spawnablePawnKinds = new List<PawnKindDef>();

		// Token: 0x040014EE RID: 5358
		public static readonly string MemoAttackedByEnemy = "HiveAttacked";

		// Token: 0x040014EF RID: 5359
		public static readonly string MemoDestroyed = "HiveDestroyed";

		// Token: 0x040014F0 RID: 5360
		public static readonly string MemoBurnedBadly = "HiveBurnedBadly";

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06002588 RID: 9608 RVA: 0x00141B4C File Offset: 0x0013FF4C
		private Lord Lord
		{
			get
			{
				Predicate<Pawn> hasDefendHiveLord = delegate(Pawn x)
				{
					Lord lord = x.GetLord();
					return lord != null && lord.LordJob is LordJob_DefendAndExpandHive;
				};
				Pawn foundPawn = this.spawnedPawns.Find(hasDefendHiveLord);
				if (base.Spawned)
				{
					if (foundPawn == null)
					{
						RegionTraverser.BreadthFirstTraverse(this.GetRegion(RegionType.Set_Passable), (Region from, Region to) => true, delegate(Region r)
						{
							List<Thing> list = r.ListerThings.ThingsOfDef(ThingDefOf.Hive);
							for (int i = 0; i < list.Count; i++)
							{
								if (list[i] != this)
								{
									if (list[i].Faction == this.Faction)
									{
										foundPawn = ((Hive)list[i]).spawnedPawns.Find(hasDefendHiveLord);
										if (foundPawn != null)
										{
											return true;
										}
									}
								}
							}
							return false;
						}, 20, RegionType.Set_Passable);
					}
					if (foundPawn != null)
					{
						return foundPawn.GetLord();
					}
				}
				return null;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06002589 RID: 9609 RVA: 0x00141C18 File Offset: 0x00140018
		private float SpawnedPawnsPoints
		{
			get
			{
				this.FilterOutUnspawnedPawns();
				float num = 0f;
				for (int i = 0; i < this.spawnedPawns.Count; i++)
				{
					num += this.spawnedPawns[i].kindDef.combatPower;
				}
				return num;
			}
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x00141C71 File Offset: 0x00140071
		public static void ResetStaticData()
		{
			Hive.spawnablePawnKinds.Clear();
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megascarab);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Spelopede);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megaspider);
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x00141CAB File Offset: 0x001400AB
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction == null)
			{
				this.SetFaction(Faction.OfInsects, null);
			}
			if (!respawningAfterLoad)
			{
				this.ticksToSpawnInitialPawns = 0;
			}
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x00141CDA File Offset: 0x001400DA
		private void SpawnInitialPawnsNow()
		{
			this.ticksToSpawnInitialPawns = -1;
			this.SpawnPawnsUntilPoints(200f);
			this.CalculateNextPawnSpawnTick();
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x00141CF8 File Offset: 0x001400F8
		public void SpawnPawnsUntilPoints(float points)
		{
			int num = 0;
			while (this.SpawnedPawnsPoints < points)
			{
				num++;
				if (num > 1000)
				{
					Log.Error("Too many iterations.", false);
					break;
				}
				Pawn pawn;
				if (!this.TrySpawnPawn(out pawn))
				{
					break;
				}
			}
			this.CalculateNextPawnSpawnTick();
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x00141D54 File Offset: 0x00140154
		public override void Tick()
		{
			base.Tick();
			if (base.Spawned)
			{
				this.FilterOutUnspawnedPawns();
				if (!this.active && !base.Position.Fogged(base.Map))
				{
					this.Activate();
				}
				if (this.active)
				{
					if (this.ticksToSpawnInitialPawns > 0)
					{
						this.ticksToSpawnInitialPawns--;
						if (this.ticksToSpawnInitialPawns <= 0)
						{
							this.SpawnInitialPawnsNow();
						}
					}
					else if (Find.TickManager.TicksGame >= this.nextPawnSpawnTick)
					{
						if (this.SpawnedPawnsPoints < 500f)
						{
							Pawn pawn;
							bool flag = this.TrySpawnPawn(out pawn);
							if (flag && pawn.caller != null)
							{
								pawn.caller.DoCall();
							}
						}
						this.CalculateNextPawnSpawnTick();
					}
				}
			}
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x00141E38 File Offset: 0x00140238
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				lords[i].ReceiveMemo(Hive.MemoDestroyed);
			}
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x00141E8C File Offset: 0x0014028C
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (dinfo.Def.externalViolence && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
				if (this.ticksToSpawnInitialPawns > 0)
				{
					this.SpawnInitialPawnsNow();
				}
				Lord lord = this.Lord;
				if (lord != null)
				{
					lord.ReceiveMemo(Hive.MemoAttackedByEnemy);
				}
			}
			if (dinfo.Def == DamageDefOf.Flame && (float)this.HitPoints < (float)base.MaxHitPoints * 0.3f)
			{
				Lord lord2 = this.Lord;
				if (lord2 != null)
				{
					lord2.ReceiveMemo(Hive.MemoBurnedBadly);
				}
			}
			base.PostApplyDamage(dinfo, totalDamageDealt);
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x00141F40 File Offset: 0x00140340
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
			Scribe_Values.Look<int>(ref this.nextPawnSpawnTick, "nextPawnSpawnTick", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.spawnedPawns, "spawnedPawns", LookMode.Reference, new object[0]);
			Scribe_Values.Look<int>(ref this.ticksToSpawnInitialPawns, "ticksToSpawnInitialPawns", 0, false);
			Scribe_Values.Look<bool>(ref this.caveColony, "caveColony", false, false);
			Scribe_Values.Look<bool>(ref this.canSpawnPawns, "canSpawnPawns", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.spawnedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x00141FFC File Offset: 0x001403FC
		private void Activate()
		{
			this.active = true;
			this.CalculateNextPawnSpawnTick();
			CompSpawnerHives comp = base.GetComp<CompSpawnerHives>();
			if (comp != null)
			{
				comp.CalculateNextHiveSpawnTick();
			}
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x0014202C File Offset: 0x0014042C
		private void CalculateNextPawnSpawnTick()
		{
			float num = GenMath.LerpDouble(0f, 5f, 1f, 0.5f, (float)this.spawnedPawns.Count);
			this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)(Hive.PawnSpawnIntervalDays.RandomInRange * 60000f / (num * Find.Storyteller.difficulty.enemyReproductionRateFactor));
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x00142098 File Offset: 0x00140498
		private void FilterOutUnspawnedPawns()
		{
			for (int i = this.spawnedPawns.Count - 1; i >= 0; i--)
			{
				if (!this.spawnedPawns[i].Spawned)
				{
					this.spawnedPawns.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x001420E8 File Offset: 0x001404E8
		private bool TrySpawnPawn(out Pawn pawn)
		{
			bool result;
			if (!this.canSpawnPawns)
			{
				pawn = null;
				result = false;
			}
			else
			{
				float curPoints = this.SpawnedPawnsPoints;
				IEnumerable<PawnKindDef> source = from x in Hive.spawnablePawnKinds
				where curPoints + x.combatPower <= 500f
				select x;
				PawnKindDef kindDef;
				if (!source.TryRandomElement(out kindDef))
				{
					pawn = null;
					result = false;
				}
				else
				{
					pawn = PawnGenerator.GeneratePawn(kindDef, base.Faction);
					GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(base.Position, base.Map, 2, null), base.Map, WipeMode.Vanish);
					this.spawnedPawns.Add(pawn);
					Lord lord = this.Lord;
					if (lord == null)
					{
						lord = this.CreateNewLord();
					}
					lord.AddPawn(pawn);
					SoundDefOf.Hive_Spawn.PlayOneShot(this);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x001421C4 File Offset: 0x001405C4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Spawn pawn",
					icon = TexCommand.ReleaseAnimals,
					action = delegate()
					{
						Pawn pawn;
						this.TrySpawnPawn(out pawn);
					}
				};
			}
			yield break;
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x001421F0 File Offset: 0x001405F0
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (this.spawnedPawns.Count > 0)
			{
				if (this.spawnedPawns.Any((Pawn p) => !p.Downed))
				{
					reason = this.def.label;
					return true;
				}
			}
			reason = null;
			return false;
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x0014225C File Offset: 0x0014065C
		private Lord CreateNewLord()
		{
			return LordMaker.MakeNewLord(base.Faction, new LordJob_DefendAndExpandHive(!this.caveColony), base.Map, null);
		}
	}
}
