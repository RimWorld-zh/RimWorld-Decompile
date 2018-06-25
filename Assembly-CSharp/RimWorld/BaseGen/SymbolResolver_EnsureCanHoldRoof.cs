using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B0 RID: 944
	public class SymbolResolver_EnsureCanHoldRoof : SymbolResolver
	{
		// Token: 0x04000A19 RID: 2585
		private static HashSet<IntVec3> roofsAboutToCollapse = new HashSet<IntVec3>();

		// Token: 0x04000A1A RID: 2586
		private static List<IntVec3> edgeRoofs = new List<IntVec3>();

		// Token: 0x04000A1B RID: 2587
		private static HashSet<IntVec3> visited = new HashSet<IntVec3>();

		// Token: 0x0600105A RID: 4186 RVA: 0x0008A080 File Offset: 0x00088480
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

		// Token: 0x0600105B RID: 4187 RVA: 0x0008A0E8 File Offset: 0x000884E8
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

		// Token: 0x0600105C RID: 4188 RVA: 0x0008A234 File Offset: 0x00088634
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

		// Token: 0x0600105D RID: 4189 RVA: 0x0008A2E4 File Offset: 0x000886E4
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

		// Token: 0x0600105E RID: 4190 RVA: 0x0008A3D0 File Offset: 0x000887D0
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
	}
}
