using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class Printer_Plane
	{
		private static Color32[] defaultColors = new Color32[4]
		{
			new Color32(255, 255, 255, 255),
			new Color32(255, 255, 255, 255),
			new Color32(255, 255, 255, 255),
			new Color32(255, 255, 255, 255)
		};

		private static Vector2[] defaultUvs = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		private static Vector2[] defaultUvsFlipped = new Vector2[4]
		{
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f),
			new Vector2(0f, 0f)
		};

		public static void PrintPlane(SectionLayer layer, Vector3 center, Vector2 size, Material mat, float rot = 0f, bool flipUv = false, Vector2[] uvs = null, Color32[] colors = null, float topVerticesAltitudeBias = 0.01f)
		{
			if (colors == null)
			{
				colors = Printer_Plane.defaultColors;
			}
			if (uvs == null)
			{
				uvs = (flipUv ? Printer_Plane.defaultUvsFlipped : Printer_Plane.defaultUvs);
			}
			LayerSubMesh subMesh = layer.GetSubMesh(mat);
			int count = subMesh.verts.Count;
			subMesh.verts.Add(new Vector3((float)(-0.5 * size.x), 0f, (float)(-0.5 * size.y)));
			subMesh.verts.Add(new Vector3((float)(-0.5 * size.x), topVerticesAltitudeBias, (float)(0.5 * size.y)));
			subMesh.verts.Add(new Vector3((float)(0.5 * size.x), topVerticesAltitudeBias, (float)(0.5 * size.y)));
			subMesh.verts.Add(new Vector3((float)(0.5 * size.x), 0f, (float)(-0.5 * size.y)));
			if (rot != 0.0)
			{
				float num = (float)(rot * 0.01745329238474369);
				num = (float)(num * -1.0);
				for (int i = 0; i < 4; i++)
				{
					Vector3 vector = subMesh.verts[count + i];
					float x = vector.x;
					Vector3 vector2 = subMesh.verts[count + i];
					float z = vector2.z;
					float num2 = Mathf.Cos(num);
					float num3 = Mathf.Sin(num);
					float num4 = x * num2 - z * num3;
					float z2 = x * num3 + z * num2;
					List<Vector3> verts = subMesh.verts;
					int index = count + i;
					float x2 = num4;
					Vector3 vector3 = subMesh.verts[count + i];
					verts[index] = new Vector3(x2, vector3.y, z2);
				}
			}
			for (int j = 0; j < 4; j++)
			{
				List<Vector3> verts2;
				int index2;
				(verts2 = subMesh.verts)[index2 = count + j] = verts2[index2] + center;
				subMesh.uvs.Add(uvs[j]);
				subMesh.colors.Add(colors[j]);
			}
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 1);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count + 3);
		}
	}
}
