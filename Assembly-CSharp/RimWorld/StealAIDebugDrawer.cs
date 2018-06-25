using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000DB RID: 219
	public static class StealAIDebugDrawer
	{
		// Token: 0x040002B0 RID: 688
		private static List<Thing> tmpToSteal = new List<Thing>();

		// Token: 0x040002B1 RID: 689
		private static BoolGrid debugDrawGrid;

		// Token: 0x040002B2 RID: 690
		private static Lord debugDrawLord = null;

		// Token: 0x060004D0 RID: 1232 RVA: 0x00035CF0 File Offset: 0x000340F0
		public static void DebugDraw()
		{
			if (!DebugViewSettings.drawStealDebug)
			{
				StealAIDebugDrawer.debugDrawLord = null;
			}
			else
			{
				Lord lord = StealAIDebugDrawer.debugDrawLord;
				StealAIDebugDrawer.debugDrawLord = StealAIDebugDrawer.FindHostileLord();
				if (StealAIDebugDrawer.debugDrawLord != null)
				{
					StealAIDebugDrawer.CheckInitDebugDrawGrid();
					float num = StealAIUtility.StartStealingMarketValueThreshold(StealAIDebugDrawer.debugDrawLord);
					if (lord != StealAIDebugDrawer.debugDrawLord)
					{
						foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
						{
							StealAIDebugDrawer.debugDrawGrid[intVec] = (StealAIDebugDrawer.TotalMarketValueAround(intVec, Find.CurrentMap, StealAIDebugDrawer.debugDrawLord.ownedPawns.Count) > num);
						}
					}
					foreach (IntVec3 c in Find.CurrentMap.AllCells)
					{
						if (StealAIDebugDrawer.debugDrawGrid[c])
						{
							CellRenderer.RenderCell(c, 0.5f);
						}
					}
					StealAIDebugDrawer.tmpToSteal.Clear();
					for (int i = 0; i < StealAIDebugDrawer.debugDrawLord.ownedPawns.Count; i++)
					{
						Pawn pawn = StealAIDebugDrawer.debugDrawLord.ownedPawns[i];
						Thing thing;
						if (StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 7f, out thing, pawn, StealAIDebugDrawer.tmpToSteal))
						{
							GenDraw.DrawLineBetween(pawn.TrueCenter(), thing.TrueCenter());
							StealAIDebugDrawer.tmpToSteal.Add(thing);
						}
					}
					StealAIDebugDrawer.tmpToSteal.Clear();
				}
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00035EC4 File Offset: 0x000342C4
		public static void Notify_ThingChanged(Thing thing)
		{
			if (StealAIDebugDrawer.debugDrawLord != null)
			{
				StealAIDebugDrawer.CheckInitDebugDrawGrid();
				if (thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Item || thing.def.passability == Traversability.Impassable)
				{
					if (thing.def.passability == Traversability.Impassable)
					{
						StealAIDebugDrawer.debugDrawLord = null;
					}
					else
					{
						int num = GenRadial.NumCellsInRadius(8f);
						float num2 = StealAIUtility.StartStealingMarketValueThreshold(StealAIDebugDrawer.debugDrawLord);
						for (int i = 0; i < num; i++)
						{
							IntVec3 intVec = thing.Position + GenRadial.RadialPattern[i];
							if (intVec.InBounds(thing.Map))
							{
								StealAIDebugDrawer.debugDrawGrid[intVec] = (StealAIDebugDrawer.TotalMarketValueAround(intVec, Find.CurrentMap, StealAIDebugDrawer.debugDrawLord.ownedPawns.Count) > num2);
							}
						}
					}
				}
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00035FC4 File Offset: 0x000343C4
		private static float TotalMarketValueAround(IntVec3 center, Map map, int pawnsCount)
		{
			float result;
			if (center.Impassable(map))
			{
				result = 0f;
			}
			else
			{
				float num = 0f;
				StealAIDebugDrawer.tmpToSteal.Clear();
				for (int i = 0; i < pawnsCount; i++)
				{
					IntVec3 intVec = center + GenRadial.RadialPattern[i];
					if (!intVec.InBounds(map) || intVec.Impassable(map) || !GenSight.LineOfSight(center, intVec, map, false, null, 0, 0))
					{
						intVec = center;
					}
					Thing thing;
					if (StealAIUtility.TryFindBestItemToSteal(intVec, map, 7f, out thing, null, StealAIDebugDrawer.tmpToSteal))
					{
						num += StealAIUtility.GetValue(thing);
						StealAIDebugDrawer.tmpToSteal.Add(thing);
					}
				}
				StealAIDebugDrawer.tmpToSteal.Clear();
				result = num;
			}
			return result;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00036098 File Offset: 0x00034498
		private static Lord FindHostileLord()
		{
			Lord lord = null;
			List<Lord> lords = Find.CurrentMap.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				if (lords[i].faction != null && lords[i].faction.HostileTo(Faction.OfPlayer))
				{
					if (lord == null || lords[i].ownedPawns.Count > lord.ownedPawns.Count)
					{
						lord = lords[i];
					}
				}
			}
			return lord;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0003613C File Offset: 0x0003453C
		private static void CheckInitDebugDrawGrid()
		{
			if (StealAIDebugDrawer.debugDrawGrid == null)
			{
				StealAIDebugDrawer.debugDrawGrid = new BoolGrid(Find.CurrentMap);
			}
			else if (!StealAIDebugDrawer.debugDrawGrid.MapSizeMatches(Find.CurrentMap))
			{
				StealAIDebugDrawer.debugDrawGrid.ClearAndResizeTo(Find.CurrentMap);
			}
		}
	}
}
