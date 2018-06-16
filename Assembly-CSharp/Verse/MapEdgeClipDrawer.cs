using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CBA RID: 3258
	[StaticConstructorOnStartup]
	public static class MapEdgeClipDrawer
	{
		// Token: 0x060047B7 RID: 18359 RVA: 0x0025BBCC File Offset: 0x00259FCC
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

		// Token: 0x040030A5 RID: 12453
		public static readonly Material ClipMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.1f, 0.1f, 0.1f), ShaderDatabase.MetaOverlay);

		// Token: 0x040030A6 RID: 12454
		private static readonly float ClipAltitude = AltitudeLayer.WorldClipper.AltitudeFor();

		// Token: 0x040030A7 RID: 12455
		private const float ClipWidth = 500f;
	}
}
