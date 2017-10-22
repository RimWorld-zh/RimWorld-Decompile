using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class MeshMakerCircles
	{
		public static Mesh MakePieMesh(int DegreesWide)
		{
			List<Vector2> list = new List<Vector2>();
			list.Add(new Vector2(0f, 0f));
			for (int num = 0; num < DegreesWide; num++)
			{
				float num2 = (float)((float)num / 180.0 * 3.1415927410125732);
				list.Add(new Vector2(0f, 0f)
				{
					x = (float)(0.550000011920929 * Math.Cos((double)num2)),
					y = (float)(0.550000011920929 * Math.Sin((double)num2))
				});
			}
			Vector3[] array = new Vector3[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				ref Vector3 val = ref array[i];
				Vector2 vector = list[i];
				float x = vector.x;
				Vector2 vector2 = list[i];
				val = new Vector3(x, 0f, vector2.y);
			}
			Triangulator triangulator = new Triangulator(list.ToArray());
			int[] triangles = triangulator.Triangulate();
			Mesh mesh = new Mesh();
			mesh.name = "MakePieMesh()";
			mesh.vertices = array;
			mesh.uv = new Vector2[list.Count];
			mesh.triangles = triangles;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}

		public static Mesh MakeCircleMesh(float radius)
		{
			List<Vector2> list = new List<Vector2>();
			list.Add(new Vector2(0f, 0f));
			for (int num = 0; num <= 360; num += 4)
			{
				float f = (float)((float)num / 180.0 * 3.1415927410125732);
				list.Add(new Vector2(radius * Mathf.Cos(f), radius * Mathf.Sin(f)));
			}
			Vector3[] array = new Vector3[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				ref Vector3 val = ref array[i];
				Vector2 vector = list[i];
				float x = vector.x;
				Vector2 vector2 = list[i];
				val = new Vector3(x, 0f, vector2.y);
			}
			int[] array2 = new int[(array.Length - 1) * 3];
			for (int j = 1; j < array.Length; j++)
			{
				int num2 = (j - 1) * 3;
				array2[num2] = 0;
				array2[num2 + 1] = (j + 1) % array.Length;
				array2[num2 + 2] = j;
			}
			Mesh mesh = new Mesh();
			mesh.name = "MakeCircleMesh()";
			mesh.vertices = array;
			mesh.triangles = array2;
			return mesh;
		}
	}
}
