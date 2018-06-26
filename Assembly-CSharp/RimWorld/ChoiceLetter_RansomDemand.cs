using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class ChoiceLetter_RansomDemand : ChoiceLetter
	{
		public Map map;

		public Faction faction;

		public Pawn kidnapped;

		public int fee;

		public ChoiceLetter_RansomDemand()
		{
		}

		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (base.ArchivedOnly)
				{
					yield return base.Option_Dismiss;
					yield break;
				}
				DiaOption accept = new DiaOption("RansomDemand_Accept".Translate());
				accept.action = delegate()
				{
					this.faction.kidnapped.RemoveKidnappedPawn(this.kidnapped);
					Find.WorldPawns.RemovePawn(this.kidnapped);
					IntVec3 intVec;
					if (this.faction.def.techLevel < TechLevel.Industrial)
					{
						if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map) && this.map.reachability.CanReachColony(c), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec) && !CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec))
						{
							Log.Warning("Could not find any edge cell.", false);
							intVec = DropCellFinder.TradeDropSpot(this.map);
						}
						GenSpawn.Spawn(this.kidnapped, intVec, this.map, WipeMode.Vanish);
					}
					else
					{
						intVec = DropCellFinder.TradeDropSpot(this.map);
						TradeUtility.SpawnDropPod(intVec, this.map, this.kidnapped);
					}
					CameraJumper.TryJump(intVec, this.map);
					TradeUtility.LaunchSilver(this.map, this.fee);
					Find.LetterStack.RemoveLetter(this);
				};
				accept.resolveTree = true;
				if (!TradeUtility.ColonyHasEnoughSilver(this.map, this.fee))
				{
					accept.Disable("NeedSilverLaunchable".Translate(new object[]
					{
						this.fee.ToString()
					}));
				}
				yield return accept;
				yield return base.Option_Reject;
				yield return base.Option_Postpone;
				yield break;
			}
		}

		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && Find.Maps.Contains(this.map) && this.faction.kidnapped.KidnappedPawnsListForReading.Contains(this.kidnapped);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.kidnapped, "kidnapped", false);
			Scribe_Values.Look<int>(ref this.fee, "fee", 0, false);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<DiaOption>, IEnumerator, IDisposable, IEnumerator<DiaOption>
		{
			internal DiaOption <accept>__1;

			internal ChoiceLetter_RansomDemand $this;

			internal DiaOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (base.ArchivedOnly)
					{
						this.$current = base.Option_Dismiss;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					accept = new DiaOption("RansomDemand_Accept".Translate());
					accept.action = delegate()
					{
						this.faction.kidnapped.RemoveKidnappedPawn(this.kidnapped);
						Find.WorldPawns.RemovePawn(this.kidnapped);
						IntVec3 intVec;
						if (this.faction.def.techLevel < TechLevel.Industrial)
						{
							if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map) && this.map.reachability.CanReachColony(c), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec) && !CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec))
							{
								Log.Warning("Could not find any edge cell.", false);
								intVec = DropCellFinder.TradeDropSpot(this.map);
							}
							GenSpawn.Spawn(this.kidnapped, intVec, this.map, WipeMode.Vanish);
						}
						else
						{
							intVec = DropCellFinder.TradeDropSpot(this.map);
							TradeUtility.SpawnDropPod(intVec, this.map, this.kidnapped);
						}
						CameraJumper.TryJump(intVec, this.map);
						TradeUtility.LaunchSilver(this.map, this.fee);
						Find.LetterStack.RemoveLetter(this);
					};
					accept.resolveTree = true;
					if (!TradeUtility.ColonyHasEnoughSilver(this.map, this.fee))
					{
						accept.Disable("NeedSilverLaunchable".Translate(new object[]
						{
							this.fee.ToString()
						}));
					}
					this.$current = accept;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = base.Option_Reject;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = base.Option_Postpone;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			DiaOption IEnumerator<DiaOption>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.DiaOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<DiaOption> IEnumerable<DiaOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ChoiceLetter_RansomDemand.<>c__Iterator0 <>c__Iterator = new ChoiceLetter_RansomDemand.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			internal void <>m__0()
			{
				this.faction.kidnapped.RemoveKidnappedPawn(this.kidnapped);
				Find.WorldPawns.RemovePawn(this.kidnapped);
				IntVec3 intVec;
				if (this.faction.def.techLevel < TechLevel.Industrial)
				{
					if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map) && this.map.reachability.CanReachColony(c), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec) && !CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec))
					{
						Log.Warning("Could not find any edge cell.", false);
						intVec = DropCellFinder.TradeDropSpot(this.map);
					}
					GenSpawn.Spawn(this.kidnapped, intVec, this.map, WipeMode.Vanish);
				}
				else
				{
					intVec = DropCellFinder.TradeDropSpot(this.map);
					TradeUtility.SpawnDropPod(intVec, this.map, this.kidnapped);
				}
				CameraJumper.TryJump(intVec, this.map);
				TradeUtility.LaunchSilver(this.map, this.fee);
				Find.LetterStack.RemoveLetter(this);
			}

			internal bool <>m__1(IntVec3 c)
			{
				return c.Standable(this.map) && this.map.reachability.CanReachColony(c);
			}

			internal bool <>m__2(IntVec3 c)
			{
				return c.Standable(this.map);
			}
		}
	}
}
