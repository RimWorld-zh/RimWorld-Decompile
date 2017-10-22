using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_NeverAdjacentUnstandable : PlaceWorker
	{
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot)
		{
			CellRect cellRect = GenAdj.OccupiedRect(center, rot, def.size);
			cellRect = cellRect.ExpandedBy(1);
			GenDraw.DrawFieldEdges(cellRect.Cells.ToList(), Color.white);
		}

		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			CellRect cellRect = GenAdj.OccupiedRect(center, rot, def.Size);
			cellRect = cellRect.ExpandedBy(1);
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			AcceptanceReport result;
			while (true)
			{
				if (!iterator.Done())
				{
					IntVec3 current = iterator.Current;
					List<Thing> list = map.thingGrid.ThingsListAt(current);
					for (int i = 0; i < list.Count; i++)
					{
						if (((list[i] != thingToIgnore) ? list[i].def.passability : Traversability.Standable) != 0)
							goto IL_006b;
					}
					iterator.MoveNext();
					continue;
				}
				result = true;
				break;
				IL_006b:
				result = "MustPlaceAdjacentStandable".Translate();
				break;
			}
			return result;
		}
	}
}
