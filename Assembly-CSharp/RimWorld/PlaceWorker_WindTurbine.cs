using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6E RID: 3182
	public class PlaceWorker_WindTurbine : PlaceWorker
	{
		// Token: 0x060045D1 RID: 17873 RVA: 0x0024C86F File Offset: 0x0024AC6F
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			GenDraw.DrawFieldEdges(WindTurbineUtility.CalculateWindCells(center, rot, def.size).ToList<IntVec3>());
		}
	}
}
