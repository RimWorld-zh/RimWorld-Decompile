using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class Hive : ThingWithComps
	{
		public bool active = true;

		public int nextPawnSpawnTick = -1;

		private List<Pawn> spawnedPawns = new List<Pawn>();

		private int ticksToSpawnInitialPawns = -1;

		public bool canSpawnPawns = true;

		private const int InitialPawnSpawnDelay = 420;

		private const int PawnSpawnRadius = 4;

		public const float MaxSpawnedPawnsPoints = 500f;

		public const float InitialPawnsPoints = 200f;

		private static readonly FloatRange PawnSpawnIntervalDays = new FloatRange(0.85f, 1.1f);

		public static readonly string MemoAttackedByEnemy = "HiveAttacked";

		public static readonly string MemoDestroyed = "HiveDestroyed";

		public static readonly string MemoBurnedBadly = "HiveBurnedBadly";

		private Lord Lord
		{
			get
			{
				Predicate<Pawn> hasDefendHiveLord = (Predicate<Pawn>)delegate(Pawn x)
				{
					Lord lord = x.GetLord();
					return lord != null && lord.LordJob is LordJob_DefendAndExpandHive;
				};
				Pawn foundPawn = this.spawnedPawns.Find(hasDefendHiveLord);
				Lord result;
				if (base.Spawned)
				{
					if (foundPawn == null)
					{
						RegionTraverser.BreadthFirstTraverse(this.GetRegion(RegionType.Set_Passable), (RegionEntryPredicate)((Region from, Region to) => true), (RegionProcessor)delegate(Region r)
						{
							List<Thing> list = r.ListerThings.ThingsOfDef(ThingDefOf.Hive);
							int num = 0;
							bool result2;
							while (true)
							{
								if (num < list.Count)
								{
									if (list[num] != this && list[num].Faction == base.Faction)
									{
										foundPawn = ((Hive)list[num]).spawnedPawns.Find(hasDefendHiveLord);
										if (foundPawn != null)
										{
											result2 = true;
											break;
										}
									}
									num++;
									continue;
								}
								result2 = false;
								break;
							}
							return result2;
						}, 20, RegionType.Set_Passable);
					}
					if (foundPawn != null)
					{
						result = foundPawn.GetLord();
						goto IL_00bd;
					}
				}
				result = null;
				goto IL_00bd;
				IL_00bd:
				return result;
			}
		}

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

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction == null)
			{
				this.SetFaction(Faction.OfInsects, null);
			}
			if (!respawningAfterLoad)
			{
				this.ticksToSpawnInitialPawns = 420;
			}
		}

		private void SpawnInitialPawnsNow()
		{
			this.ticksToSpawnInitialPawns = -1;
			this.SpawnPawnsUntilPoints(200f);
		}

		public void SpawnPawnsUntilPoints(float points)
		{
			Pawn pawn = default(Pawn);
			while (this.SpawnedPawnsPoints < points && this.TrySpawnPawn(out pawn))
			{
			}
			this.CalculateNextPawnSpawnTick();
		}

		public override void TickRare()
		{
			base.TickRare();
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
						this.ticksToSpawnInitialPawns -= 250;
						if (this.ticksToSpawnInitialPawns <= 0)
						{
							this.SpawnInitialPawnsNow();
						}
					}
					if (Find.TickManager.TicksGame >= this.nextPawnSpawnTick)
					{
						Pawn pawn = default(Pawn);
						if (this.SpawnedPawnsPoints < 500.0 && this.TrySpawnPawn(out pawn) && pawn.caller != null)
						{
							pawn.caller.DoCall();
						}
						this.CalculateNextPawnSpawnTick();
					}
				}
			}
		}

		public override void DeSpawn()
		{
			Map map = base.Map;
			base.DeSpawn();
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				lords[i].ReceiveMemo(Hive.MemoDestroyed);
			}
		}

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
			if (dinfo.Def == DamageDefOf.Flame && (float)this.HitPoints < (float)base.MaxHitPoints * 0.30000001192092896)
			{
				Lord lord2 = this.Lord;
				if (lord2 != null)
				{
					lord2.ReceiveMemo(Hive.MemoBurnedBadly);
				}
			}
			base.PostApplyDamage(dinfo, totalDamageDealt);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
			Scribe_Values.Look<int>(ref this.nextPawnSpawnTick, "nextPawnSpawnTick", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.spawnedPawns, "spawnedPawns", LookMode.Reference, new object[0]);
			Scribe_Values.Look<int>(ref this.ticksToSpawnInitialPawns, "ticksToSpawnInitialPawns", 0, false);
			Scribe_Values.Look<bool>(ref this.canSpawnPawns, "canSpawnPawns", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.spawnedPawns.RemoveAll((Predicate<Pawn>)((Pawn x) => x == null));
			}
		}

		private void Activate()
		{
			this.active = true;
			this.nextPawnSpawnTick = Find.TickManager.TicksGame + Rand.Range(200, 400);
			CompSpawnerHives comp = base.GetComp<CompSpawnerHives>();
			if (comp != null)
			{
				comp.CalculateNextHiveSpawnTick();
			}
		}

		private void CalculateNextPawnSpawnTick()
		{
			float num = GenMath.LerpDouble(0f, 5f, 1f, 0.5f, (float)this.spawnedPawns.Count);
			this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)(Hive.PawnSpawnIntervalDays.RandomInRange * 60000.0 / (num * Find.Storyteller.difficulty.enemyReproductionRateFactor));
		}

		private void FilterOutUnspawnedPawns()
		{
			for (int num = this.spawnedPawns.Count - 1; num >= 0; num--)
			{
				if (!this.spawnedPawns[num].Spawned)
				{
					this.spawnedPawns.RemoveAt(num);
				}
			}
		}

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
				List<PawnKindDef> list = new List<PawnKindDef>();
				list.Add(PawnKindDefOf.Megascarab);
				list.Add(PawnKindDefOf.Spelopede);
				list.Add(PawnKindDefOf.Megaspider);
				float curPoints = this.SpawnedPawnsPoints;
				IEnumerable<PawnKindDef> source = from x in list
				where curPoints + x.combatPower <= 500.0
				select x;
				PawnKindDef kindDef = default(PawnKindDef);
				if (!source.TryRandomElement<PawnKindDef>(out kindDef))
				{
					pawn = null;
					result = false;
				}
				else
				{
					pawn = PawnGenerator.GeneratePawn(kindDef, base.Faction);
					GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(base.Position, base.Map, 4, null), base.Map);
					this.spawnedPawns.Add(pawn);
					Lord lord = this.Lord;
					if (lord == null)
					{
						lord = this.CreateNewLord();
					}
					lord.AddPawn(pawn);
					result = true;
				}
			}
			return result;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g = enumerator.Current;
					yield return g;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!Prefs.DevMode)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "DEBUG: Spawn pawn",
				icon = TexCommand.ReleaseAnimals,
				action = (Action)delegate
				{
					Pawn pawn = default(Pawn);
					((_003CGetGizmos_003Ec__Iterator0)/*Error near IL_00f4: stateMachine*/)._0024this.TrySpawnPawn(out pawn);
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_012f:
			/*Error near IL_0130: Unexpected return in MoveNext()*/;
		}

		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			bool result;
			if (this.spawnedPawns.Count > 0 && this.spawnedPawns.Any((Predicate<Pawn>)((Pawn p) => !p.Downed)))
			{
				reason = base.def.label;
				result = true;
			}
			else
			{
				reason = (string)null;
				result = false;
			}
			return result;
		}

		private Lord CreateNewLord()
		{
			return LordMaker.MakeNewLord(base.Faction, new LordJob_DefendAndExpandHive(), base.Map, null);
		}
	}
}
