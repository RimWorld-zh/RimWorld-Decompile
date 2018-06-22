using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D73 RID: 3443
	internal static class MeshMakerPlanes
	{
		// Token: 0x06004D3B RID: 19771 RVA: 0x00283DA0 File Offset: 0x002821A0
		public static Mesh NewPlaneMesh(float size)
		{
			return MeshMakerPlanes.NewPlaneMesh(size, false);
		}

		// Token: 0x06004D3C RID: 19772 RVA: 0x00283DBC File Offset: 0x002821BC
		public static Mesh NewPlaneMesh(float size, bool flipped)
		{
			return MeshMakerPlanes.NewPlaneMesh(size, flipped, false);
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x00283DDC File Offset: 0x002821DC
		public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift)
		{
			return MeshMakerPlanes.NewPlaneMesh(new Vector2(size, size), flipped, backLift, false);
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x00283E00 File Offset: 0x00282200
		public static Mesh NewPlaneMesh(float size, bool flipped, bool backLift, bool twist)
		{
			return MeshMakerPlanes.NewPlaneMesh(new Vector2(size, size), flipped, backLift, twist);
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x00283E24 File Offset: 0x00282224
		public static Mesh NewPlaneMesh(Vector2 size, bool flipped, bool backLift, bool twist)
		{
			Vector3[] array = new Vector3[4];
			Vector2[] array2 = new Vector2[4];
			int[] array3 = new int[6];
			array[0] = new Vector3(-0.5f * size.x, 0f, -0.5f * size.y);
			array[1] = new Vector3(-0.5f * size.x, 0f, 0.5f * size.y);
			array[2] = new Vector3(0.5f * size.x, 0f, 0.5f * size.y);
			array[3] = new Vector3(0.5f * size.x, 0f, -0.5f * size.y);
			if (backLift)
			{
				array[1].y = 0.00234375f;
				array[2].y = 0.00234375f;
				array[3].y = 0.000937500037f;
			}
			if (twist)
			{
				array[0].y = 0.001171875f;
				array[1].y = 0.0005859375f;
				array[2].y = 0f;
				array[3].y = 0.0005859375f;
			}
			if (!flipped)
			{
				array2[0] = new Vector2(0f, 0f);
				array2[1] = new Vector2(0f, 1f);
				array2[2] = new Vector2(1f, 1f);
				array2[3] = new Vector2(1f, 0f);
			}
			else
			{
				array2[0] = new Vector2(1f, 0f);
				array2[1] = new Vector2(1f, 1f);
				array2[2] = new Vector2(0f, 1f);
				array2[3] = new Vector2(0f, 0f);
			}
			array3[0] = 0;
			array3[1] = 1;
			array3[2] = 2;
			array3[3] = 0;
			array3[4] = 2;
			array3[5] = 3;
			Mesh mesh = new Mesh();
			mesh.name = "NewPlaneMesh()";
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.SetTriangles(array3, 0);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x002840CC File Offset: 0x002824CC
		public static Mesh NewWholeMapPlane()
		{
			Mesh mesh = MeshMakerPlanes.NewPlaneMesh(2000f, false, false);
			Vector2[] array = new Vector2[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = mesh.uv[i] * 200f;
			}
			mesh.uv = array;
			return mesh;
		}

		// Token: 0x0400337E RID: 13182
		private const float BackLiftAmount = 0.00234375f;

		// Token: 0x0400337F RID: 13183
		private const float TwistAmount = 0.001171875f;
	}
}
