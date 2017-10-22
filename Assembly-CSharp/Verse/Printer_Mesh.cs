using UnityEngine;

namespace Verse
{
	public static class Printer_Mesh
	{
		public static void PrintMesh(SectionLayer layer, Vector3 center, Mesh mesh, Material mat)
		{
			LayerSubMesh subMesh = layer.GetSubMesh(mat);
			int count = subMesh.verts.Count;
			int vertexCount = mesh.vertexCount;
			Vector3[] vertices = mesh.vertices;
			Color32[] colors = mesh.colors32;
			Vector2[] uv = mesh.uv;
			for (int num = 0; num < vertexCount; num++)
			{
				subMesh.verts.Add(vertices[num] + center);
				if (colors.Length > num)
				{
					subMesh.colors.Add(colors[num]);
				}
				else
				{
					subMesh.colors.Add(new Color32((byte)255, (byte)255, (byte)255, (byte)255));
				}
				if (uv.Length > num)
				{
					subMesh.uvs.Add(uv[num]);
				}
				else
				{
					subMesh.uvs.Add(Vector2.zero);
				}
			}
			int[] triangles = mesh.triangles;
			for (int i = 0; i < triangles.Length; i++)
			{
				int num2 = triangles[i];
				subMesh.tris.Add(count + num2);
			}
		}
	}
}
