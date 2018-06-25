using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB9 RID: 3257
	[StaticConstructorOnStartup]
	public static class MapEdgeClipDrawer
	{
		// Token: 0x040030B5 RID: 12469
		public static readonly Material ClipMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.1f, 0.1f, 0.1f), ShaderDatabase.MetaOverlay);

		// Token: 0x040030B6 RID: 12470
		private static readonly float ClipAltitude = AltitudeLayer.WorldClipper.AltitudeFor();

		// Token: 0x040030B7 RID: 12471
		private const float ClipWidth = 500f;

		// Token: 0x060047C1 RID: 18369 RVA: 0x0025D350 File Offset: 0x0025B750
		public static void DrawClippers(Map map)
		{
			IntVec3 size = map.Size;
			Vector3 s = new Vector3(500f, 1f, (float)size.z);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(new Vector3(-250f, MapEdgeClipDrawer.ClipAltitude, (float)size.z / 2f), Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, MapEdgeClipDrawer.ClipMat, 0);
			matrix = default(Matrix4x4);
			matrix.SetTRS(new Vector3((float)size.x + 250f, MapEdgeClipDrawer.ClipAltitude, (float)size.z / 2f), Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, MapEdgeClipDrawer.ClipMat, 0);
			s = new Vector3(1000f, 1f, 500f);
			matrix = default(Matrix4x4);
			matrix.SetTRS(new Vector3((float)(size.x / 2), MapEdgeClipDrawer.ClipAltitude, (float)size.z + 250f), Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, MapEdgeClipDrawer.ClipMat, 0);
			matrix = default(Matrix4x4);
			matrix.SetTRS(new Vector3((float)(size.x / 2), MapEdgeClipDrawer.ClipAltitude, -250f), Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, MapEdgeClipDrawer.ClipMat, 0);
		}
	}
}
