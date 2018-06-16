using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C7B RID: 3195
	[StaticConstructorOnStartup]
	public class PlaceWorker_FuelingPort : PlaceWorker
	{
		// Token: 0x060045EF RID: 17903 RVA: 0x0024CF6C File Offset: 0x0024B36C
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

		// Token: 0x060045F0 RID: 17904 RVA: 0x0024CFC0 File Offset: 0x0024B3C0
		public static void DrawFuelingPortCell(IntVec3 center, Rot4 rot)
		{
			Vector3 position = FuelingPortUtility.GetFuelingPortCell(center, rot).ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, PlaceWorker_FuelingPort.FuelingPortCellMaterial, 0);
		}

		// Token: 0x04002FAD RID: 12205
		private static readonly Material FuelingPortCellMaterial = MaterialPool.MatFrom("UI/Overlays/FuelingPort", ShaderDatabase.Transparent);
	}
}
