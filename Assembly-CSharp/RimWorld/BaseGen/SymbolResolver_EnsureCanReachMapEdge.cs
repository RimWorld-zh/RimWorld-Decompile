using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_EnsureCanReachMapEdge : SymbolResolver
	{
		private static HashSet<Room> visited = new HashSet<Room>();

		private static List<IntVec3> path = new List<IntVec3>();

		private static List<IntVec3> cellsInRandomOrder = new List<IntVec3>();

		public SymbolResolver_EnsureCanReachMapEdge()
		{
		}

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
								if (found)
								{
									return;
								}
								if (map.reachability.CanReachMapEdge(x, traverseParms))
								{
									found = true;
									foundDest = x;
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

		private bool CanTraverse(IntVec3 c, bool canPathThroughNonStandable)
		{
			Map map = BaseGen.globalSettings.map;
			Building edifice = c.GetEdifice(map);
			return this.IsWallOrRock(edifice) || ((canPathThroughNonStandable || c.Standable(map)) && !c.Impassable(map));
		}

		private bool IsWallOrRock(Building b)
		{
			return b != null && (b.def == ThingDefOf.Wall || b.def.building.isNaturalRock);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SymbolResolver_EnsureCanReachMapEdge()
		{
		}

		[CompilerGenerated]
		private sealed class <TryMakeAllCellsReachable>c__AnonStorey1
		{
			internal bool canPathThroughNonStandable;

			internal Map map;

			internal SymbolResolver_EnsureCanReachMapEdge $this;

			public <TryMakeAllCellsReachable>c__AnonStorey1()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <TryMakeAllCellsReachable>c__AnonStorey0
		{
			internal bool found;

			internal TraverseParms traverseParms;

			internal IntVec3 foundDest;

			internal SymbolResolver_EnsureCanReachMapEdge.<TryMakeAllCellsReachable>c__AnonStorey1 <>f__ref$1;

			public <TryMakeAllCellsReachable>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return !this.found && this.<>f__ref$1.$this.CanTraverse(x, this.<>f__ref$1.canPathThroughNonStandable);
			}

			internal void <>m__1(IntVec3 x)
			{
				if (this.found)
				{
					return;
				}
				if (this.<>f__ref$1.map.reachability.CanReachMapEdge(x, this.traverseParms))
				{
					this.found = true;
					this.foundDest = x;
				}
			}
		}
	}
}
