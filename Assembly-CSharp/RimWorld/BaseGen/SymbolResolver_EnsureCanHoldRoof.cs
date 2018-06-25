using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_EnsureCanHoldRoof : SymbolResolver
	{
		private static HashSet<IntVec3> roofsAboutToCollapse = new HashSet<IntVec3>();

		private static List<IntVec3> edgeRoofs = new List<IntVec3>();

		private static HashSet<IntVec3> visited = new HashSet<IntVec3>();

		[CompilerGenerated]
		private static Action<IntVec3> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<IntVec3> <>f__am$cache1;

		public SymbolResolver_EnsureCanHoldRoof()
		{
		}

		public override void Resolve(ResolveParams rp)
		{
			ThingDef wallStuff = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
			do
			{
				this.CalculateRoofsAboutToCollapse(rp.rect);
				this.CalculateEdgeRoofs(rp.rect);
			}
			while (this.TrySpawnPillar(rp.faction, wallStuff));
		}

		private void CalculateRoofsAboutToCollapse(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Clear();
			SymbolResolver_EnsureCanHoldRoof.visited.Clear();
			CellRect.CellRectIterator iterator = rect.GetIterator();
			while (!iterator.Done())
			{
				IntVec3 intVec = iterator.Current;
				if (intVec.Roofed(map))
				{
					if (!RoofCollapseCellsFinder.ConnectsToRoofHolder(intVec, map, SymbolResolver_EnsureCanHoldRoof.visited))
					{
						map.floodFiller.FloodFill(intVec, (IntVec3 x) => x.Roofed(map), delegate(IntVec3 x)
						{
							SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Add(x);
						}, int.MaxValue, false, null);
					}
				}
				iterator.MoveNext();
			}
			CellRect.CellRectIterator iterator2 = rect.GetIterator();
			while (!iterator2.Done())
			{
				IntVec3 intVec2 = iterator2.Current;
				if (intVec2.Roofed(map))
				{
					if (!SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Contains(intVec2))
					{
						if (!RoofCollapseUtility.WithinRangeOfRoofHolder(intVec2, map, false))
						{
							SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Add(intVec2);
						}
					}
				}
				iterator2.MoveNext();
			}
		}

		private void CalculateEdgeRoofs(CellRect rect)
		{
			SymbolResolver_EnsureCanHoldRoof.edgeRoofs.Clear();
			foreach (IntVec3 intVec in SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse)
			{
				for (int i = 0; i < 4; i++)
				{
					IntVec3 item = intVec + GenAdj.CardinalDirections[i];
					if (!SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Contains(item))
					{
						SymbolResolver_EnsureCanHoldRoof.edgeRoofs.Add(intVec);
						break;
					}
				}
			}
		}

		private bool TrySpawnPillar(Faction faction, ThingDef wallStuff)
		{
			bool result;
			if (!SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Any<IntVec3>())
			{
				result = false;
			}
			else
			{
				Map map = BaseGen.globalSettings.map;
				IntVec3 bestCell = IntVec3.Invalid;
				float bestScore = 0f;
				FloodFiller floodFiller = map.floodFiller;
				IntVec3 invalid = IntVec3.Invalid;
				Predicate<IntVec3> passCheck = (IntVec3 x) => SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Contains(x);
				Action<IntVec3> processor = delegate(IntVec3 x)
				{
					float pillarSpawnScore = this.GetPillarSpawnScore(x);
					if (pillarSpawnScore > 0f && (!bestCell.IsValid || pillarSpawnScore >= bestScore))
					{
						bestCell = x;
						bestScore = pillarSpawnScore;
					}
				};
				List<IntVec3> extraRoots = SymbolResolver_EnsureCanHoldRoof.edgeRoofs;
				floodFiller.FloodFill(invalid, passCheck, processor, int.MaxValue, false, extraRoots);
				if (bestCell.IsValid)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Wall, wallStuff);
					thing.SetFaction(faction, null);
					GenSpawn.Spawn(thing, bestCell, map, WipeMode.Vanish);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		private float GetPillarSpawnScore(IntVec3 c)
		{
			Map map = BaseGen.globalSettings.map;
			float result;
			if (c.Impassable(map) || c.GetFirstBuilding(map) != null || c.GetFirstItem(map) != null || c.GetFirstPawn(map) != null)
			{
				result = 0f;
			}
			else
			{
				bool flag = true;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c2 = c + GenAdj.AdjacentCells[i];
					if (!c2.InBounds(map) || !c2.Walkable(map))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					result = 2f;
				}
				else
				{
					result = 1f;
				}
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SymbolResolver_EnsureCanHoldRoof()
		{
		}

		[CompilerGenerated]
		private static void <CalculateRoofsAboutToCollapse>m__0(IntVec3 x)
		{
			SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Add(x);
		}

		[CompilerGenerated]
		private static bool <TrySpawnPillar>m__1(IntVec3 x)
		{
			return SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Contains(x);
		}

		[CompilerGenerated]
		private sealed class <CalculateRoofsAboutToCollapse>c__AnonStorey0
		{
			internal Map map;

			public <CalculateRoofsAboutToCollapse>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Roofed(this.map);
			}
		}

		[CompilerGenerated]
		private sealed class <TrySpawnPillar>c__AnonStorey1
		{
			internal IntVec3 bestCell;

			internal float bestScore;

			internal SymbolResolver_EnsureCanHoldRoof $this;

			public <TrySpawnPillar>c__AnonStorey1()
			{
			}

			internal void <>m__0(IntVec3 x)
			{
				float pillarSpawnScore = this.$this.GetPillarSpawnScore(x);
				if (pillarSpawnScore > 0f && (!this.bestCell.IsValid || pillarSpawnScore >= this.bestScore))
				{
					this.bestCell = x;
					this.bestScore = pillarSpawnScore;
				}
			}
		}
	}
}
