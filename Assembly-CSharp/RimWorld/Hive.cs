using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	public class Hive : ThingWithComps
	{
		public bool active = true;

		public int nextPawnSpawnTick = -1;

		private List<Pawn> spawnedPawns = new List<Pawn>();

		public bool caveColony = false;

		public bool canSpawnPawns = true;

		public const int PawnSpawnRadius = 2;

		public const float MaxSpawnedPawnsPoints = 500f;

		public const float InitialPawnsPoints = 200f;

		private static readonly FloatRange PawnSpawnIntervalDays = new FloatRange(0.85f, 1.1f);

		public static List<PawnKindDef> spawnablePawnKinds = new List<PawnKindDef>();

		public static readonly string MemoAttackedByEnemy = "HiveAttacked";

		public static readonly string MemoDestroyed = "HiveDestroyed";

		public static readonly string MemoBurnedBadly = "HiveBurnedBadly";

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		[CompilerGenerated]
		private static RegionEntryPredicate <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache2;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache3;

		public Hive()
		{
		}

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

		public static void ResetStaticData()
		{
			Hive.spawnablePawnKinds.Clear();
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megascarab);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Spelopede);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megaspider);
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction == null)
			{
				this.SetFaction(Faction.OfInsects, null);
			}
			if (!respawningAfterLoad && this.active)
			{
				this.SpawnInitialPawns();
			}
		}

		private void SpawnInitialPawns()
		{
			this.SpawnPawnsUntilPoints(200f);
			this.CalculateNextPawnSpawnTick();
		}

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
					if (Find.TickManager.TicksGame >= this.nextPawnSpawnTick)
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

		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (dinfo.Def.externalViolence && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
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

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
			Scribe_Values.Look<int>(ref this.nextPawnSpawnTick, "nextPawnSpawnTick", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.spawnedPawns, "spawnedPawns", LookMode.Reference, new object[0]);
			Scribe_Values.Look<bool>(ref this.caveColony, "caveColony", false, false);
			Scribe_Values.Look<bool>(ref this.canSpawnPawns, "canSpawnPawns", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.spawnedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		private void Activate()
		{
			this.active = true;
			this.SpawnInitialPawns();
			this.CalculateNextPawnSpawnTick();
			CompSpawnerHives comp = base.GetComp<CompSpawnerHives>();
			if (comp != null)
			{
				comp.CalculateNextHiveSpawnTick();
			}
		}

		private void CalculateNextPawnSpawnTick()
		{
			float num = GenMath.LerpDouble(0f, 5f, 1f, 0.5f, (float)this.spawnedPawns.Count);
			this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)(Hive.PawnSpawnIntervalDays.RandomInRange * 60000f / (num * Find.Storyteller.difficulty.enemyReproductionRateFactor));
		}

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

		private Lord CreateNewLord()
		{
			return LordMaker.MakeNewLord(base.Faction, new LordJob_DefendAndExpandHive(!this.caveColony), base.Map, null);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Hive()
		{
		}

		[CompilerGenerated]
		private static bool <get_Lord>m__0(Pawn x)
		{
			Lord lord = x.GetLord();
			return lord != null && lord.LordJob is LordJob_DefendAndExpandHive;
		}

		[CompilerGenerated]
		private static bool <get_Lord>m__1(Region from, Region to)
		{
			return true;
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__2(Pawn x)
		{
			return x == null;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy0()
		{
			return base.GetGizmos();
		}

		[CompilerGenerated]
		private static bool <PreventPlayerSellingThingsNearby>m__3(Pawn p)
		{
			return !p.Downed;
		}

		[CompilerGenerated]
		private sealed class <>c__AnonStorey1
		{
			internal Predicate<Pawn> hasDefendHiveLord;

			internal Pawn foundPawn;

			internal Hive $this;

			public <>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsOfDef(ThingDefOf.Hive);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != this.$this)
					{
						if (list[i].Faction == this.$this.Faction)
						{
							this.foundPawn = ((Hive)list[i]).spawnedPawns.Find(this.hasDefendHiveLord);
							if (this.foundPawn != null)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class <TrySpawnPawn>c__AnonStorey2
		{
			internal float curPoints;

			public <TrySpawnPawn>c__AnonStorey2()
			{
			}

			internal bool <>m__0(PawnKindDef x)
			{
				return this.curPoints + x.combatPower <= 500f;
			}
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal Command_Action <com>__2;

			internal Hive $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<GetGizmos>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_126;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						g = enumerator.Current;
						this.$current = g;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (!Prefs.DevMode)
				{
					goto IL_126;
				}
				Command_Action com = new Command_Action();
				com.defaultLabel = "DEBUG: Spawn pawn";
				com.icon = TexCommand.ReleaseAnimals;
				com.action = delegate()
				{
					Pawn pawn;
					base.TrySpawnPawn(out pawn);
				};
				this.$current = com;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_126:
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Hive.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new Hive.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			internal void <>m__0()
			{
				Pawn pawn;
				base.TrySpawnPawn(out pawn);
			}
		}
	}
}
