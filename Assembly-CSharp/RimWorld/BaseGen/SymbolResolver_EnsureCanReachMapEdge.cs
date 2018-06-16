using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003AF RID: 943
	public class SymbolResolver_EnsureCanReachMapEdge : SymbolResolver
	{
		// Token: 0x0600105F RID: 4191 RVA: 0x0008A234 File Offset: 0x00088634
		public override void Resolve(ResolveParams rp)
		{
			SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Clear();
			CellRect.CellRectIterator iterator = rp.rect.GetIterator();
			while (!iterator.Done())
			{
				SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Add(iterator.Current);
				iterator.MoveNext();
			}
			SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Shuffle<IntVec3>();
			this.TryMakeAllCellsReachable(false, rp);
			this.TryMakeAllCellsReachable(true, rp);
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0008A2A0 File Offset: 0x000886A0
		private void TryMakeAllCellsReachable(bool canPathThroughNonStandable, ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_EnsureCanReachMapEdge.visited.Clear();
			for (int i = 0; i < SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Count; i++)
			{
				IntVec3 intVec = SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder[i];
				if (this.CanTraverse(intVec, canPathThroughNonStandable))
				{
					Room room = intVec.GetRoom(map, RegionType.Set_Passable);
					if (room != null && !SymbolResolver_EnsureCanReachMapEdge.visited.Contains(room))
					{
						SymbolResolver_EnsureCanReachMapEdge.visited.Add(room);
						TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
						if (!map.reachability.CanReachMapEdge(intVec, traverseParms))
						{
							bool found = false;
							IntVec3 foundDest = IntVec3.Invalid;
							map.floodFiller.FloodFill(intVec, (IntVec3 x) => !found && this.CanTraverse(x, canPathThroughNonStandable), delegate(IntVec3 x)
							{
								if (!found)
								{
									if (map.reachability.CanReachMapEdge(x, traverseParms))
									{
										found = true;
										foundDest = x;
									}
								}
							}, int.MaxValue, true, null);
							if (found)
							{
								this.ReconstructPathAndDestroyWalls(foundDest, room, rp);
							}
						}
					}
				}
			}
			SymbolResolver_EnsureCanReachMapEdge.visited.Clear();
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0008A3F8 File Offset: 0x000887F8
		private void ReconstructPathAndDestroyWalls(IntVec3 foundDest, Room room, ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			map.floodFiller.ReconstructLastFloodFillPath(foundDest, SymbolResolver_EnsureCanReachMapEdge.path);
			while (SymbolResolver_EnsureCanReachMapEdge.path.Count >= 2 && SymbolResolver_EnsureCanReachMapEdge.path[0].AdjacentToCardinal(room) && SymbolResolver_EnsureCanReachMapEdge.path[1].AdjacentToCardinal(room))
			{
				SymbolResolver_EnsureCanReachMapEdge.path.RemoveAt(0);
			}
			IntVec3 intVec = IntVec3.Invalid;
			ThingDef thingDef = null;
			IntVec3 intVec2 = IntVec3.Invalid;
			ThingDef thingDef2 = null;
			for (int i = 0; i < SymbolResolver_EnsureCanReachMapEdge.path.Count; i++)
			{
				Building edifice = SymbolResolver_EnsureCanReachMapEdge.path[i].GetEdifice(map);
				if (this.IsWallOrRock(edifice))
				{
					if (!intVec.IsValid)
					{
						intVec = SymbolResolver_EnsureCanReachMapEdge.path[i];
						thingDef = edifice.Stuff;
					}
					intVec2 = SymbolResolver_EnsureCanReachMapEdge.path[i];
					thingDef2 = edifice.Stuff;
					edifice.Destroy(DestroyMode.Vanish);
				}
			}
			if (intVec.IsValid)
			{
				ThingDef thingDef3;
				if ((thingDef3 = thingDef) == null)
				{
					thingDef3 = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
				}
				ThingDef stuff = thingDef3;
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Door, stuff);
				thing.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing, intVec, map, WipeMode.Vanish);
			}
			if (intVec2.IsValid && intVec2 != intVec && !intVec2.AdjacentToCardinal(intVec))
			{
				ThingDef thingDef4;
				if ((thingDef4 = thingDef2) == null)
				{
					thingDef4 = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
				}
				ThingDef stuff2 = thingDef4;
				Thing thing2 = ThingMaker.MakeThing(ThingDefOf.Door, stuff2);
				thing2.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing2, intVec2, map, WipeMode.Vanish);
			}
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0008A5E4 File Offset: 0x000889E4
		private bool CanTraverse(IntVec3 c, bool canPathThroughNonStandable)
		{
			Map map = BaseGen.globalSettings.map;
			Building edifice = c.GetEdifice(map);
			return this.IsWallOrRock(edifice) || ((canPathThroughNonStandable || c.Standable(map)) && !c.Impassable(map));
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0008A64C File Offset: 0x00088A4C
		private bool IsWallOrRock(Building b)
		{
			return b != null && (b.def == ThingDefOf.Wall || b.def.building.isNaturalRock);
		}

		// Token: 0x04000A1C RID: 2588
		private static HashSet<Room> visited = new HashSet<Room>();

		// Token: 0x04000A1D RID: 2589
		private static List<IntVec3> path = new List<IntVec3>();

		// Token: 0x04000A1E RID: 2590
		private static List<IntVec3> cellsInRandomOrder = new List<IntVec3>();
	}
}
