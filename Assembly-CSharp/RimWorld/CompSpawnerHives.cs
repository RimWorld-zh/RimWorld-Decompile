using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompSpawnerHives : ThingComp
	{
		private int nextHiveSpawnTick = -1;

		public bool canSpawnHives = true;

		public const int MaxHivesPerMap = 30;

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache0;

		public CompSpawnerHives()
		{
		}

		private CompProperties_SpawnerHives Props
		{
			get
			{
				return (CompProperties_SpawnerHives)this.props;
			}
		}

		private bool CanSpawnChildHive
		{
			get
			{
				return this.canSpawnHives && HivesUtility.TotalSpawnedHivesCount(this.parent.Map) < 30;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.CalculateNextHiveSpawnTick();
			}
		}

		public override void CompTick()
		{
			base.CompTick();
			Hive hive = this.parent as Hive;
			if (hive == null || hive.active)
			{
				if (Find.TickManager.TicksGame >= this.nextHiveSpawnTick)
				{
					Hive t;
					if (this.TrySpawnChildHive(false, out t))
					{
						Messages.Message("MessageHiveReproduced".Translate(), t, MessageTypeDefOf.NegativeEvent, true);
					}
					else
					{
						this.CalculateNextHiveSpawnTick();
					}
				}
			}
		}

		public override string CompInspectStringExtra()
		{
			string result;
			if (!this.canSpawnHives)
			{
				result = "DormantHiveNotReproducing".Translate();
			}
			else if (this.CanSpawnChildHive)
			{
				result = "HiveReproducesIn".Translate() + ": " + (this.nextHiveSpawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod();
			}
			else
			{
				result = null;
			}
			return result;
		}

		public void CalculateNextHiveSpawnTick()
		{
			Room room = this.parent.GetRoom(RegionType.Set_Passable);
			int num = 0;
			int num2 = GenRadial.NumCellsInRadius(9f);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map))
				{
					if (intVec.GetRoom(this.parent.Map, RegionType.Set_Passable) == room)
					{
						if (intVec.GetThingList(this.parent.Map).Any((Thing t) => t is Hive))
						{
							num++;
						}
					}
				}
			}
			float num3 = GenMath.LerpDouble(0f, 7f, 1f, 0.35f, (float)Mathf.Clamp(num, 0, 7));
			this.nextHiveSpawnTick = Find.TickManager.TicksGame + (int)(this.Props.HiveSpawnIntervalDays.RandomInRange * 60000f / (num3 * Find.Storyteller.difficulty.enemyReproductionRateFactor));
		}

		public bool TrySpawnChildHive(bool ignoreRoofedRequirement, out Hive newHive)
		{
			bool result;
			if (!this.CanSpawnChildHive)
			{
				newHive = null;
				result = false;
			}
			else
			{
				IntVec3 loc = CompSpawnerHives.FindChildHiveLocation(this.parent.Position, this.parent.Map, this.parent.def, this.Props, ignoreRoofedRequirement);
				if (!loc.IsValid)
				{
					newHive = null;
					result = false;
				}
				else
				{
					newHive = (Hive)GenSpawn.Spawn(this.parent.def, loc, this.parent.Map, WipeMode.FullRefund);
					if (newHive.Faction != this.parent.Faction)
					{
						newHive.SetFaction(this.parent.Faction, null);
					}
					Hive hive = this.parent as Hive;
					if (hive != null)
					{
						newHive.active = hive.active;
					}
					this.CalculateNextHiveSpawnTick();
					result = true;
				}
			}
			return result;
		}

		public static IntVec3 FindChildHiveLocation(IntVec3 pos, Map map, ThingDef parentDef, CompProperties_SpawnerHives props, bool ignoreRoofedRequirement)
		{
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < 2; i++)
			{
				float minDist = props.HiveSpawnPreferredMinDist;
				if (i == 1)
				{
					minDist = 0f;
				}
				if (CellFinder.TryFindRandomReachableCellNear(pos, map, props.HiveSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), null, out intVec, 999999))
				{
					intVec = CellFinder.FindNoWipeSpawnLocNear(intVec, map, parentDef, Rot4.North, 2, (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement));
					break;
				}
			}
			return intVec;
		}

		private static bool CanSpawnHiveAt(IntVec3 c, Map map, IntVec3 parentPos, ThingDef parentDef, float minDist, bool ignoreRoofedRequirement)
		{
			bool result;
			if ((!ignoreRoofedRequirement && !c.Roofed(map)) || (!c.Walkable(map) || (minDist != 0f && (float)c.DistanceToSquared(parentPos) < minDist * minDist)) || c.GetFirstThing(map, ThingDefOf.InsectJelly) != null || c.GetFirstThing(map, ThingDefOf.GlowPod) != null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
					if (c2.InBounds(map))
					{
						List<Thing> thingList = c2.GetThingList(map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j] is Hive || thingList[j] is TunnelHiveSpawner)
							{
								return false;
							}
						}
					}
				}
				List<Thing> thingList2 = c.GetThingList(map);
				for (int k = 0; k < thingList2.Count; k++)
				{
					Thing thing = thingList2[k];
					bool flag = thing.def.category == ThingCategory.Building && thing.def.passability == Traversability.Impassable;
					if (flag && GenSpawn.SpawningWipes(parentDef, thing.def))
					{
						return true;
					}
				}
				result = true;
			}
			return result;
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Reproduce",
					icon = TexCommand.GatherSpotActive,
					action = delegate()
					{
						Hive hive;
						this.TrySpawnChildHive(false, out hive);
					}
				};
			}
			yield break;
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextHiveSpawnTick, "nextHiveSpawnTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canSpawnHives, "canSpawnHives", true, false);
		}

		[CompilerGenerated]
		private static bool <CalculateNextHiveSpawnTick>m__0(Thing t)
		{
			return t is Hive;
		}

		[CompilerGenerated]
		private sealed class <FindChildHiveLocation>c__AnonStorey1
		{
			internal Map map;

			internal IntVec3 pos;

			internal ThingDef parentDef;

			internal bool ignoreRoofedRequirement;

			public <FindChildHiveLocation>c__AnonStorey1()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <FindChildHiveLocation>c__AnonStorey2
		{
			internal float minDist;

			internal CompSpawnerHives.<FindChildHiveLocation>c__AnonStorey1 <>f__ref$1;

			public <FindChildHiveLocation>c__AnonStorey2()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return CompSpawnerHives.CanSpawnHiveAt(c, this.<>f__ref$1.map, this.<>f__ref$1.pos, this.<>f__ref$1.parentDef, this.minDist, this.<>f__ref$1.ignoreRoofedRequirement);
			}

			internal bool <>m__1(IntVec3 c)
			{
				return CompSpawnerHives.CanSpawnHiveAt(c, this.<>f__ref$1.map, this.<>f__ref$1.pos, this.<>f__ref$1.parentDef, this.minDist, this.<>f__ref$1.ignoreRoofedRequirement);
			}
		}

		[CompilerGenerated]
		private sealed class <CompGetGizmosExtra>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Command_Action <com>__1;

			internal CompSpawnerHives $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <CompGetGizmosExtra>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (Prefs.DevMode)
					{
						Command_Action com = new Command_Action();
						com.defaultLabel = "Dev: Reproduce";
						com.icon = TexCommand.GatherSpotActive;
						com.action = delegate()
						{
							Hive hive;
							base.TrySpawnChildHive(false, out hive);
						};
						this.$current = com;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
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
				this.$disposing = true;
				this.$PC = -1;
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
				CompSpawnerHives.<CompGetGizmosExtra>c__Iterator0 <CompGetGizmosExtra>c__Iterator = new CompSpawnerHives.<CompGetGizmosExtra>c__Iterator0();
				<CompGetGizmosExtra>c__Iterator.$this = this;
				return <CompGetGizmosExtra>c__Iterator;
			}

			internal void <>m__0()
			{
				Hive hive;
				base.TrySpawnChildHive(false, out hive);
			}
		}
	}
}
