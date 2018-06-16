using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6F RID: 3183
	public class PlaceWorker_WindTurbine : PlaceWorker
	{
		// Token: 0x060045D3 RID: 17875 RVA: 0x0024C897 File Offset: 0x0024AC97
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			GenDraw.DrawFieldEdges(WindTurbineUtility.CalculateWindCells(center, rot, def.size).ToList<IntVec3>());
		}
	}
}
