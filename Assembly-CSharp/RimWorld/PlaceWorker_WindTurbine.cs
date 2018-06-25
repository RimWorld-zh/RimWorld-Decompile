using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6E RID: 3182
	public class PlaceWorker_WindTurbine : PlaceWorker
	{
		// Token: 0x060045DD RID: 17885 RVA: 0x0024DFFB File Offset: 0x0024C3FB
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			GenDraw.DrawFieldEdges(WindTurbineUtility.CalculateWindCells(center, rot, def.size).ToList<IntVec3>());
		}
	}
}
