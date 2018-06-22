using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C77 RID: 3191
	[StaticConstructorOnStartup]
	public class PlaceWorker_FuelingPort : PlaceWorker
	{
		// Token: 0x060045F6 RID: 17910 RVA: 0x0024E314 File Offset: 0x0024C714
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			if (def.building != null && def.building.hasFuelingPort)
			{
				if (FuelingPortUtility.GetFuelingPortCell(center, rot).Standable(currentMap))
				{
					PlaceWorker_FuelingPort.DrawFuelingPortCell(center, rot);
				}
			}
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x0024E368 File Offset: 0x0024C768
		public static void DrawFuelingPortCell(IntVec3 center, Rot4 rot)
		{
			Vector3 position = FuelingPortUtility.GetFuelingPortCell(center, rot).ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, PlaceWorker_FuelingPort.FuelingPortCellMaterial, 0);
		}

		// Token: 0x04002FB5 RID: 12213
		private static readonly Material FuelingPortCellMaterial = MaterialPool.MatFrom("UI/Overlays/FuelingPort", ShaderDatabase.Transparent);
	}
}
