using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class MeshUtility
	{
		private static List<int> offsets = new List<int>();

		private static List<bool> vertIsUsed = new List<bool>();

		public static void RemoveVertices(List<Vector3> verts, List<TriangleIndices> tris, Predicate<Vector3> predicate)
		{
			int num = 0;
			int count = tris.Count;
			while (num < count)
			{
				TriangleIndices triangleIndices = tris[num];
				if (predicate(verts[triangleIndices.v1]) || predicate(verts[triangleIndices.v2]) || predicate(verts[triangleIndices.v3]))
				{
					tris[num] = new TriangleIndices(-1, -1, -1);
				}
				num++;
			}
			tris.RemoveAll((Predicate<TriangleIndices>)((TriangleIndices x) => x.v1 == -1));
			MeshUtility.RemoveUnusedVertices(verts, tris);
		}

		public static void RemoveUnusedVertices(List<Vector3> verts, List<TriangleIndices> tris)
		{
			MeshUtility.vertIsUsed.Clear();
			int num = 0;
			int count = verts.Count;
			while (num < count)
			{
				MeshUtility.vertIsUsed.Add(false);
				num++;
			}
			int num2 = 0;
			int count2 = tris.Count;
			while (num2 < count2)
			{
				TriangleIndices triangleIndices = tris[num2];
				MeshUtility.vertIsUsed[triangleIndices.v1] = true;
				MeshUtility.vertIsUsed[triangleIndices.v2] = true;
				MeshUtility.vertIsUsed[triangleIndices.v3] = true;
				num2++;
			}
			int num3 = 0;
			MeshUtility.offsets.Clear();
			int num4 = 0;
			int count3 = verts.Count;
			while (num4 < count3)
			{
				if (!MeshUtility.vertIsUsed[num4])
				{
					num3++;
				}
				MeshUtility.offsets.Add(num3);
				num4++;
			}
			int num5 = 0;
			int count4 = tris.Count;
			while (num5 < count4)
			{
				TriangleIndices triangleIndices2 = tris[num5];
				tris[num5] = new TriangleIndices(triangleIndices2.v1 - MeshUtility.offsets[triangleIndices2.v1], triangleIndices2.v2 - MeshUtility.offsets[triangleIndices2.v2], triangleIndices2.v3 - MeshUtility.offsets[triangleIndices2.v3]);
				num5++;
			}
			verts.RemoveAll((Func<Vector3, int, bool>)((Vector3 elem, int index) => !MeshUtility.vertIsUsed[index]));
		}

		public static bool Visible(Vector3 point, float radius, Vector3 viewCenter, float viewAngle)
		{
			if (viewAngle >= 180.0)
			{
				return true;
			}
			return Vector3.Angle(viewCenter * radius, point) <= viewAngle;
		}

		public static Color32 MutateAlpha(this Color32 input, byte newAlpha)
		{
			input.a = newAlpha;
			return input;
		}
	}
}
