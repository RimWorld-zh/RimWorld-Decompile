using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C79 RID: 3193
	[StaticConstructorOnStartup]
	public class PlaceWorker_FuelingPort : PlaceWorker
	{
		// Token: 0x04002FB5 RID: 12213
		private static readonly Material FuelingPortCellMaterial = MaterialPool.MatFrom("UI/Overlays/FuelingPort", ShaderDatabase.Transparent);

		// Token: 0x060045F9 RID: 17913 RVA: 0x0024E3F0 File Offset: 0x0024C7F0
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

		// Token: 0x060045FA RID: 17914 RVA: 0x0024E444 File Offset: 0x0024C844
		public static void DrawFuelingPortCell(IntVec3 center, Rot4 rot)
		{
			Vector3 position = FuelingPortUtility.GetFuelingPortCell(center, rot).ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, PlaceWorker_FuelingPort.FuelingPortCellMaterial, 0);
		}
	}
}
