using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C76 RID: 3190
	public class PlaceWorker_NeverAdjacentUnstandable : PlaceWorker
	{
		// Token: 0x060045F0 RID: 17904 RVA: 0x0024E524 File Offset: 0x0024C924
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			GenDraw.DrawFieldEdges(GenAdj.OccupiedRect(center, rot, def.size).ExpandedBy(1).Cells.ToList<IntVec3>(), Color.white);
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x0024E560 File Offset: 0x0024C960
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			CellRect.CellRectIterator iterator = GenAdj.OccupiedRect(center, rot, def.Size).ExpandedBy(1).GetIterator();
			while (!iterator.Done())
			{
				IntVec3 c = iterator.Current;
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != thingToIgnore && list[i].def.passability != Traversability.Standable)
					{
						return "MustPlaceAdjacentStandable".Translate();
					}
				}
				iterator.MoveNext();
			}
			return true;
		}
	}
}
