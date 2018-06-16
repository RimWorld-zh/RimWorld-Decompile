using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C77 RID: 3191
	public class PlaceWorker_NeverAdjacentUnstandable : PlaceWorker
	{
		// Token: 0x060045E6 RID: 17894 RVA: 0x0024CDC0 File Offset: 0x0024B1C0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			GenDraw.DrawFieldEdges(GenAdj.OccupiedRect(center, rot, def.size).ExpandedBy(1).Cells.ToList<IntVec3>(), Color.white);
		}

		// Token: 0x060045E7 RID: 17895 RVA: 0x0024CDFC File Offset: 0x0024B1FC
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
