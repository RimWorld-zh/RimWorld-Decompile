using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	public static class LightningBoltMeshMaker
	{
		private static List<Vector2> verts2D;

		private static Vector2 lightningTop;

		private const float LightningHeight = 200f;

		private const float LightningRootXVar = 50f;

		private const float VertexInterval = 0.25f;

		private const float MeshWidth = 2f;

		private const float UVIntervalY = 0.04f;

		private const float PerturbAmp = 12f;

		private const float PerturbFreq = 0.007f;

		public static Mesh NewBoltMesh()
		{
			LightningBoltMeshMaker.lightningTop = new Vector2(Rand.Range(-50f, 50f), 200f);
			LightningBoltMeshMaker.MakeVerticesBase();
			LightningBoltMeshMaker.PeturbVerticesRandomly();
			LightningBoltMeshMaker.DoubleVertices();
			return LightningBoltMeshMaker.MeshFromVerts();
		}

		private static void MakeVerticesBase()
		{
			int num = (int)Math.Ceiling((Vector2.zero - LightningBoltMeshMaker.lightningTop).magnitude / 0.25);
			Vector2 b = LightningBoltMeshMaker.lightningTop / (float)num;
			LightningBoltMeshMaker.verts2D = new List<Vector2>();
			Vector2 vector = Vector2.zero;
			for (int num2 = 0; num2 < num; num2++)
			{
				LightningBoltMeshMaker.verts2D.Add(vector);
				vector += b;
			}
		}

		private static void PeturbVerticesRandomly()
		{
			Perlin perlin = new Perlin(0.0070000002160668373, 2.0, 0.5, 6, Rand.Range(0, 2147483647), QualityMode.High);
			List<Vector2> list = LightningBoltMeshMaker.verts2D.ListFullCopy();
			LightningBoltMeshMaker.verts2D.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				float d = (float)(12.0 * (float)perlin.GetValue((double)i, 0.0, 0.0));
				Vector2 item = list[i] + d * Vector2.right;
				LightningBoltMeshMaker.verts2D.Add(item);
			}
		}

		private static void DoubleVertices()
		{
			List<Vector2> list = LightningBoltMeshMaker.verts2D.ListFullCopy();
			Vector3 vector = default(Vector3);
			Vector2 a = default(Vector2);
			LightningBoltMeshMaker.verts2D.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				if (i <= list.Count - 2)
				{
					vector = Quaternion.AngleAxis(90f, Vector3.up) * (list[i] - list[i + 1]);
					a = new Vector2(vector.y, vector.z);
					a.Normalize();
				}
				Vector2 item = list[i] - 1f * a;
				Vector2 item2 = list[i] + 1f * a;
				LightningBoltMeshMaker.verts2D.Add(item);
				LightningBoltMeshMaker.verts2D.Add(item2);
			}
		}

		private static Mesh MeshFromVerts()
		{
			Vector3[] array = new Vector3[LightningBoltMeshMaker.verts2D.Count];
			for (int i = 0; i < array.Length; i++)
			{
				ref Vector3 val = ref array[i];
				Vector2 vector = LightningBoltMeshMaker.verts2D[i];
				float x = vector.x;
				Vector2 vector2 = LightningBoltMeshMaker.verts2D[i];
				val = new Vector3(x, 0f, vector2.y);
			}
			float num = 0f;
			Vector2[] array2 = new Vector2[LightningBoltMeshMaker.verts2D.Count];
			for (int num2 = 0; num2 < LightningBoltMeshMaker.verts2D.Count; num2 += 2)
			{
				array2[num2] = new Vector2(0f, num);
				array2[num2 + 1] = new Vector2(1f, num);
				num = (float)(num + 0.039999999105930328);
			}
			int[] array3 = new int[LightningBoltMeshMaker.verts2D.Count * 3];
			for (int num3 = 0; num3 < LightningBoltMeshMaker.verts2D.Count - 2; num3 += 2)
			{
				int num4 = num3 * 3;
				array3[num4] = num3;
				array3[num4 + 1] = num3 + 1;
				array3[num4 + 2] = num3 + 2;
				array3[num4 + 3] = num3 + 2;
				array3[num4 + 4] = num3 + 1;
				array3[num4 + 5] = num3 + 3;
			}
			Mesh mesh = new Mesh();
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.triangles = array3;
			mesh.name = "MeshFromVerts()";
			return mesh;
		}
	}
}
