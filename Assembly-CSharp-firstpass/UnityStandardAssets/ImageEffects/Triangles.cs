using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020001B2 RID: 434
	internal class Triangles
	{
		// Token: 0x04000896 RID: 2198
		private static Mesh[] meshes;

		// Token: 0x04000897 RID: 2199
		private static int currentTris = 0;

		// Token: 0x06000972 RID: 2418 RVA: 0x00019850 File Offset: 0x00017A50
		private static bool HasMeshes()
		{
			bool result;
			if (Triangles.meshes == null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < Triangles.meshes.Length; i++)
				{
					if (null == Triangles.meshes[i])
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x000198A8 File Offset: 0x00017AA8
		private static void Cleanup()
		{
			if (Triangles.meshes != null)
			{
				for (int i = 0; i < Triangles.meshes.Length; i++)
				{
					if (null != Triangles.meshes[i])
					{
						UnityEngine.Object.DestroyImmediate(Triangles.meshes[i]);
						Triangles.meshes[i] = null;
					}
				}
				Triangles.meshes = null;
			}
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00019910 File Offset: 0x00017B10
		private static Mesh[] GetMeshes(int totalWidth, int totalHeight)
		{
			Mesh[] result;
			if (Triangles.HasMeshes() && Triangles.currentTris == totalWidth * totalHeight)
			{
				result = Triangles.meshes;
			}
			else
			{
				int num = 21666;
				int num2 = totalWidth * totalHeight;
				Triangles.currentTris = num2;
				int num3 = Mathf.CeilToInt(1f * (float)num2 / (1f * (float)num));
				Triangles.meshes = new Mesh[num3];
				int num4 = 0;
				for (int i = 0; i < num2; i += num)
				{
					int triCount = Mathf.FloorToInt((float)Mathf.Clamp(num2 - i, 0, num));
					Triangles.meshes[num4] = Triangles.GetMesh(triCount, i, totalWidth, totalHeight);
					num4++;
				}
				result = Triangles.meshes;
			}
			return result;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x000199CC File Offset: 0x00017BCC
		private static Mesh GetMesh(int triCount, int triOffset, int totalWidth, int totalHeight)
		{
			Mesh mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave;
			Vector3[] array = new Vector3[triCount * 3];
			Vector2[] array2 = new Vector2[triCount * 3];
			Vector2[] array3 = new Vector2[triCount * 3];
			int[] array4 = new int[triCount * 3];
			for (int i = 0; i < triCount; i++)
			{
				int num = i * 3;
				int num2 = triOffset + i;
				float num3 = Mathf.Floor((float)(num2 % totalWidth)) / (float)totalWidth;
				float num4 = Mathf.Floor((float)(num2 / totalWidth)) / (float)totalHeight;
				Vector3 vector = new Vector3(num3 * 2f - 1f, num4 * 2f - 1f, 1f);
				array[num] = vector;
				array[num + 1] = vector;
				array[num + 2] = vector;
				array2[num] = new Vector2(0f, 0f);
				array2[num + 1] = new Vector2(1f, 0f);
				array2[num + 2] = new Vector2(0f, 1f);
				array3[num] = new Vector2(num3, num4);
				array3[num + 1] = new Vector2(num3, num4);
				array3[num + 2] = new Vector2(num3, num4);
				array4[num] = num;
				array4[num + 1] = num + 1;
				array4[num + 2] = num + 2;
			}
			mesh.vertices = array;
			mesh.triangles = array4;
			mesh.uv = array2;
			mesh.uv2 = array3;
			return mesh;
		}
	}
}
