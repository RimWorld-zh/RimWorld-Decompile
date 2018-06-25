using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D7B RID: 3451
	[StaticConstructorOnStartup]
	[HasDebugOutput]
	public static class MeshPool
	{
		// Token: 0x0400338E RID: 13198
		private const int MaxGridMeshSize = 15;

		// Token: 0x0400338F RID: 13199
		private const float HumanlikeBodyWidth = 1.5f;

		// Token: 0x04003390 RID: 13200
		private const float HumanlikeHeadAverageWidth = 1.5f;

		// Token: 0x04003391 RID: 13201
		private const float HumanlikeHeadNarrowWidth = 1.3f;

		// Token: 0x04003392 RID: 13202
		public static readonly GraphicMeshSet humanlikeBodySet = new GraphicMeshSet(1.5f);

		// Token: 0x04003393 RID: 13203
		public static readonly GraphicMeshSet humanlikeHeadSet = new GraphicMeshSet(1.5f);

		// Token: 0x04003394 RID: 13204
		public static readonly GraphicMeshSet humanlikeHairSetAverage = new GraphicMeshSet(1.5f);

		// Token: 0x04003395 RID: 13205
		public static readonly GraphicMeshSet humanlikeHairSetNarrow = new GraphicMeshSet(1.3f, 1.5f);

		// Token: 0x04003396 RID: 13206
		public static readonly Mesh plane025 = MeshMakerPlanes.NewPlaneMesh(0.25f);

		// Token: 0x04003397 RID: 13207
		public static readonly Mesh plane03 = MeshMakerPlanes.NewPlaneMesh(0.3f);

		// Token: 0x04003398 RID: 13208
		public static readonly Mesh plane05 = MeshMakerPlanes.NewPlaneMesh(0.5f);

		// Token: 0x04003399 RID: 13209
		public static readonly Mesh plane08 = MeshMakerPlanes.NewPlaneMesh(0.8f);

		// Token: 0x0400339A RID: 13210
		public static readonly Mesh plane10 = MeshMakerPlanes.NewPlaneMesh(1f);

		// Token: 0x0400339B RID: 13211
		public static readonly Mesh plane10Back = MeshMakerPlanes.NewPlaneMesh(1f, false, true);

		// Token: 0x0400339C RID: 13212
		public static readonly Mesh plane10Flip = MeshMakerPlanes.NewPlaneMesh(1f, true);

		// Token: 0x0400339D RID: 13213
		public static readonly Mesh plane14 = MeshMakerPlanes.NewPlaneMesh(1.4f);

		// Token: 0x0400339E RID: 13214
		public static readonly Mesh plane20 = MeshMakerPlanes.NewPlaneMesh(2f);

		// Token: 0x0400339F RID: 13215
		public static readonly Mesh wholeMapPlane;

		// Token: 0x040033A0 RID: 13216
		private static Dictionary<Vector2, Mesh> planes = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x040033A1 RID: 13217
		private static Dictionary<Vector2, Mesh> planesFlip = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x040033A2 RID: 13218
		public static readonly Mesh circle = MeshMakerCircles.MakeCircleMesh(1f);

		// Token: 0x040033A3 RID: 13219
		public static readonly Mesh[] pies = new Mesh[361];

		// Token: 0x06004D54 RID: 19796 RVA: 0x00284B34 File Offset: 0x00282F34
		static MeshPool()
		{
			for (int i = 0; i < 361; i++)
			{
				MeshPool.pies[i] = MeshMakerCircles.MakePieMesh(i);
			}
			MeshPool.wholeMapPlane = MeshMakerPlanes.NewWholeMapPlane();
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x00284C78 File Offset: 0x00283078
		public static Mesh GridPlane(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planes.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, false, false, false);
				MeshPool.planes.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x00284CB8 File Offset: 0x002830B8
		public static Mesh GridPlaneFlip(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planesFlip.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, true, false, false);
				MeshPool.planesFlip.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x00284CF8 File Offset: 0x002830F8
		private static Vector2 RoundedToHundredths(this Vector2 v)
		{
			return new Vector2((float)((int)(v.x * 100f)) / 100f, (float)((int)(v.y * 100f)) / 100f);
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x00284D3C File Offset: 0x0028313C
		[DebugOutput]
		[Category("System")]
		public static void MeshPoolStats()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MeshPool stats:");
			stringBuilder.AppendLine("Planes: " + MeshPool.planes.Count);
			stringBuilder.AppendLine("PlanesFlip: " + MeshPool.planesFlip.Count);
			Log.Message(stringBuilder.ToString(), false);
		}
	}
}
