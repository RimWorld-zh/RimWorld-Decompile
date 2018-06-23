using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098C RID: 2444
	public static class MeshUtility
	{
		// Token: 0x0400237C RID: 9084
		private static List<int> offsets = new List<int>();

		// Token: 0x0400237D RID: 9085
		private static List<bool> vertIsUsed = new List<bool>();

		// Token: 0x060036FD RID: 14077 RVA: 0x001D62B4 File Offset: 0x001D46B4
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

		// Token: 0x060036FE RID: 14078 RVA: 0x001D6368 File Offset: 0x001D4768
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

		// Token: 0x060036FF RID: 14079 RVA: 0x001D64F8 File Offset: 0x001D48F8
		public static bool Visible(Vector3 point, float radius, Vector3 viewCenter, float viewAngle)
		{
			return viewAngle >= 180f || Vector3.Angle(viewCenter * radius, point) <= viewAngle;
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x001D6534 File Offset: 0x001D4934
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

		// Token: 0x06003701 RID: 14081 RVA: 0x001D65A0 File Offset: 0x001D49A0
		public static Color32 MutateAlpha(this Color32 input, byte newAlpha)
		{
			input.a = newAlpha;
			return input;
		}
	}
}
