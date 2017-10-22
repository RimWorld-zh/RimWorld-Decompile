using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_EnsureCanHoldRoof : SymbolResolver
	{
		private static HashSet<IntVec3> roofsAboutToCollapse = new HashSet<IntVec3>();

		private static List<IntVec3> edgeRoofs = new List<IntVec3>();

		private static HashSet<IntVec3> visited = new HashSet<IntVec3>();

		public override void Resolve(ResolveParams rp)
		{
			ThingDef wallStuff = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
			while (true)
			{
				this.CalculateRoofsAboutToCollapse(rp.rect);
				this.CalculateEdgeRoofs(rp.rect);
				if (!this.TrySpawnPillar(rp.faction, wallStuff))
					break;
			}
		}

		private void CalculateRoofsAboutToCollapse(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Clear();
			SymbolResolver_EnsureCanHoldRoof.visited.Clear();
			CellRect.CellRectIterator iterator = rect.GetIterator();
			while (!iterator.Done())
			{
				IntVec3 current = iterator.Current;
				if (current.Roofed(map) && !RoofCollapseCellsFinder.ConnectsToRoofHolder(current, map, SymbolResolver_EnsureCanHoldRoof.visited))
				{
					map.floodFiller.FloodFill(current, (Predicate<IntVec3>)((IntVec3 x) => x.Roofed(map)), (Action<IntVec3>)delegate(IntVec3 x)
					{
						SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Add(x);
					}, 2147483647, false, null);
				}
				iterator.MoveNext();
			}
			CellRect.CellRectIterator iterator2 = rect.GetIterator();
			while (!iterator2.Done())
			{
				IntVec3 current2 = iterator2.Current;
				if (current2.Roofed(map) && !SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Contains(current2) && !RoofCollapseUtility.WithinRangeOfRoofHolder(current2, map))
				{
					SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Add(current2);
				}
				iterator2.MoveNext();
			}
		}

		private void CalculateEdgeRoofs(CellRect rect)
		{
			SymbolResolver_EnsureCanHoldRoof.edgeRoofs.Clear();
			foreach (IntVec3 item2 in SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse)
			{
				for (int i = 0; i < 4; i++)
				{
					IntVec3 item = item2 + GenAdj.CardinalDirections[i];
					if (!SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Contains(item))
					{
						SymbolResolver_EnsureCanHoldRoof.edgeRoofs.Add(item2);
						break;
					}
				}
			}
		}

		private bool TrySpawnPillar(Faction faction, ThingDef wallStuff)
		{
			bool result;
			if (!SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Any())
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
				Predicate<IntVec3> passCheck = (Predicate<IntVec3>)((IntVec3 x) => SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Contains(x));
				Action<IntVec3> processor = (Action<IntVec3>)delegate(IntVec3 x)
				{
					float pillarSpawnScore = this.GetPillarSpawnScore(x);
					if (pillarSpawnScore > 0.0)
					{
						if (bestCell.IsValid && !(pillarSpawnScore >= bestScore))
							return;
						bestCell = x;
						bestScore = pillarSpawnScore;
					}
				};
				List<IntVec3> extraRoots = SymbolResolver_EnsureCanHoldRoof.edgeRoofs;
				floodFiller.FloodFill(invalid, passCheck, processor, 2147483647, false, extraRoots);
				if (bestCell.IsValid)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Wall, wallStuff);
					thing.SetFaction(faction, null);
					GenSpawn.Spawn(thing, bestCell, map);
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
				int num = 0;
				while (num < 8)
				{
					IntVec3 c2 = c + GenAdj.AdjacentCells[num];
					if (c2.InBounds(map) && c2.Walkable(map))
					{
						num++;
						continue;
					}
					flag = false;
					break;
				}
				result = (float)((!flag) ? 1.0 : 2.0);
			}
			return result;
		}
	}
}
