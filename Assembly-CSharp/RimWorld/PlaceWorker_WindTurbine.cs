using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6B RID: 3179
	public class PlaceWorker_WindTurbine : PlaceWorker
	{
		// Token: 0x060045DA RID: 17882 RVA: 0x0024DC3F File Offset: 0x0024C03F
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			GenDraw.DrawFieldEdges(WindTurbineUtility.CalculateWindCells(center, rot, def.size).ToList<IntVec3>());
		}
	}
}
