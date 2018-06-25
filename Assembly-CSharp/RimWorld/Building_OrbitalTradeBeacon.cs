using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Building_OrbitalTradeBeacon : Building
	{
		private const float TradeRadius = 7.9f;

		private static List<IntVec3> tradeableCells = new List<IntVec3>();

		[CompilerGenerated]
		private static RegionEntryPredicate <>f__am$cache0;

		public Building_OrbitalTradeBeacon()
		{
		}

		public IEnumerable<IntVec3> TradeableCells
		{
			get
			{
				return Building_OrbitalTradeBeacon.TradeableCellsAround(base.Position, base.Map);
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>() != null)
			{
				yield return new Command_Action
				{
					action = new Action(this.MakeMatchingStockpile),
					hotKey = KeyBindingDefOf.Misc1,
					defaultDesc = "CommandMakeBeaconStockpileDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true),
					defaultLabel = "CommandMakeBeaconStockpileLabel".Translate()
				};
			}
			yield break;
		}

		private void MakeMatchingStockpile()
		{
			Designator des = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>();
			des.DesignateMultiCell(from c in this.TradeableCells
			where des.CanDesignateCell(c).Accepted
			select c);
		}

		public static List<IntVec3> TradeableCellsAround(IntVec3 pos, Map map)
		{
			Building_OrbitalTradeBeacon.tradeableCells.Clear();
			List<IntVec3> result;
			if (!pos.InBounds(map))
			{
				result = Building_OrbitalTradeBeacon.tradeableCells;
			}
			else
			{
				Region region = pos.GetRegion(map, RegionType.Set_Passable);
				if (region == null)
				{
					result = Building_OrbitalTradeBeacon.tradeableCells;
				}
				else
				{
					RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.portal == null, delegate(Region r)
					{
						foreach (IntVec3 item in r.Cells)
						{
							if (item.InHorDistOf(pos, 7.9f))
							{
								Building_OrbitalTradeBeacon.tradeableCells.Add(item);
							}
						}
						return false;
					}, 13, RegionType.Set_Passable);
					result = Building_OrbitalTradeBeacon.tradeableCells;
				}
			}
			return result;
		}

		public static IEnumerable<Building_OrbitalTradeBeacon> AllPowered(Map map)
		{
			foreach (Building_OrbitalTradeBeacon b in map.listerBuildings.AllBuildingsColonistOfClass<Building_OrbitalTradeBeacon>())
			{
				CompPowerTrader power = b.GetComp<CompPowerTrader>();
				if (power == null || power.PowerOn)
				{
					yield return b;
				}
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Building_OrbitalTradeBeacon()
		{
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy0()
		{
			return base.GetGizmos();
		}

		[CompilerGenerated]
		private static bool <TradeableCellsAround>m__0(Region from, Region r)
		{
			return r.portal == null;
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__1;

			internal Command_Action <mz>__2;

			internal Building_OrbitalTradeBeacon $this;

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
					goto IL_15B;
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
				if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>() == null)
				{
					goto IL_15B;
				}
				Command_Action mz = new Command_Action();
				mz.action = new Action(base.MakeMatchingStockpile);
				mz.hotKey = KeyBindingDefOf.Misc1;
				mz.defaultDesc = "CommandMakeBeaconStockpileDesc".Translate();
				mz.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
				mz.defaultLabel = "CommandMakeBeaconStockpileLabel".Translate();
				this.$current = mz;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_15B:
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
				Building_OrbitalTradeBeacon.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new Building_OrbitalTradeBeacon.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <MakeMatchingStockpile>c__AnonStorey2
		{
			internal Designator des;

			public <MakeMatchingStockpile>c__AnonStorey2()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return this.des.CanDesignateCell(c).Accepted;
			}
		}

		[CompilerGenerated]
		private sealed class <TradeableCellsAround>c__AnonStorey3
		{
			internal IntVec3 pos;

			public <TradeableCellsAround>c__AnonStorey3()
			{
			}

			internal bool <>m__0(Region r)
			{
				foreach (IntVec3 item in r.Cells)
				{
					if (item.InHorDistOf(this.pos, 7.9f))
					{
						Building_OrbitalTradeBeacon.tradeableCells.Add(item);
					}
				}
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class <AllPowered>c__Iterator1 : IEnumerable, IEnumerable<Building_OrbitalTradeBeacon>, IEnumerator, IDisposable, IEnumerator<Building_OrbitalTradeBeacon>
		{
			internal Map map;

			internal IEnumerator<Building_OrbitalTradeBeacon> $locvar0;

			internal Building_OrbitalTradeBeacon <b>__1;

			internal CompPowerTrader <power>__2;

			internal Building_OrbitalTradeBeacon $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllPowered>c__Iterator1()
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
					enumerator = map.listerBuildings.AllBuildingsColonistOfClass<Building_OrbitalTradeBeacon>().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						b = enumerator.Current;
						power = b.GetComp<CompPowerTrader>();
						if (power == null || power.PowerOn)
						{
							this.$current = b;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
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
				this.$PC = -1;
				return false;
			}

			Building_OrbitalTradeBeacon IEnumerator<Building_OrbitalTradeBeacon>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Building_OrbitalTradeBeacon>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Building_OrbitalTradeBeacon> IEnumerable<Building_OrbitalTradeBeacon>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Building_OrbitalTradeBeacon.<AllPowered>c__Iterator1 <AllPowered>c__Iterator = new Building_OrbitalTradeBeacon.<AllPowered>c__Iterator1();
				<AllPowered>c__Iterator.map = map;
				return <AllPowered>c__Iterator;
			}
		}
	}
}
