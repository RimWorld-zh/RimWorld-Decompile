using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000990 RID: 2448
	public static class MeshUtility
	{
		// Token: 0x06003704 RID: 14084 RVA: 0x001D60B8 File Offset: 0x001D44B8
		public static void RemoveVertices(List<Vector3> verts, List<TriangleIndices> tris, Predicate<Vector3> predicate)
		{
			int i = 0;
			int count = tris.Count;
			while (i < count)
			{
				TriangleIndices triangleIndices = tris[i];
				if (predicate(verts[triangleIndices.v1]) || predicate(verts[triangleIndices.v2]) || predicate(verts[triangleIndices.v3]))
				{
					tris[i] = new TriangleIndices(-1, -1, -1);
				}
				i++;
			}
			tris.RemoveAll((TriangleIndices x) => x.v1 == -1);
			MeshUtility.RemoveUnusedVertices(verts, tris);
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x001D616C File Offset: 0x001D456C
		public static void RemoveUnusedVertices(List<Vector3> verts, List<TriangleIndices> tris)
		{
			MeshUtility.vertIsUsed.Clear();
			int i = 0;
			int count = verts.Count;
			while (i < count)
			{
				MeshUtility.vertIsUsed.Add(false);
				i++;
			}
			int j = 0;
			int count2 = tris.Count;
			while (j < count2)
			{
				TriangleIndices triangleIndices = tris[j];
				MeshUtility.vertIsUsed[triangleIndices.v1] = true;
				MeshUtility.vertIsUsed[triangleIndices.v2] = true;
				MeshUtility.vertIsUsed[triangleIndices.v3] = true;
				j++;
			}
			int num = 0;
			MeshUtility.offsets.Clear();
			int k = 0;
			int count3 = verts.Count;
			while (k < count3)
			{
				if (!MeshUtility.vertIsUsed[k])
				{
					num++;
				}
				MeshUtility.offsets.Add(num);
				k++;
			}
			int l = 0;
			int count4 = tris.Count;
			while (l < count4)
			{
				TriangleIndices triangleIndices2 = tris[l];
				tris[l] = new TriangleIndices(triangleIndices2.v1 - MeshUtility.offsets[triangleIndices2.v1], triangleIndices2.v2 - MeshUtility.offsets[triangleIndices2.v2], triangleIndices2.v3 - MeshUtility.offsets[triangleIndices2.v3]);
				l++;
			}
			verts.RemoveAll((Vector3 elem, int index) => !MeshUtility.vertIsUsed[index]);
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x001D62FC File Offset: 0x001D46FC
		public static bool Visible(Vector3 point, float radius, Vector3 viewCenter, float viewAngle)
		{
			return viewAngle >= 180f || Vector3.Angle(viewCenter * radius, point) <= viewAngle;
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x001D6338 File Offset: 0x001D4738
		public static bool VisibleForWorldgen(Vector3 point, float radius, Vector3 viewCenter, float viewAngle)
		{
			bool result;
			if (viewAngle >= 180f)
			{
				result = true;
			}
			else
			{
				float num = Vector3.Angle(viewCenter * radius, point) + -1E-05f;
				if (Mathf.Abs(num - viewAngle) < 1E-06f)
				{
					Log.Warning(string.Format("Angle difference {0} is within epsilon; recommend adjusting visibility tweak", num - viewAngle), false);
				}
				result = (num <= viewAngle);
			}
			return result;
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x001D63A4 File Offset: 0x001D47A4
		public static Color32 MutateAlpha(this Color32 input, byte newAlpha)
		{
			input.a = newAlpha;
			return input;
		}

		// Token: 0x0400237E RID: 9086
		private static List<int> offsets = new List<int>();

		// Token: 0x0400237F RID: 9087
		private static List<bool> vertIsUsed = new List<bool>();
	}
}
