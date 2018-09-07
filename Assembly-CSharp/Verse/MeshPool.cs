using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	[HasDebugOutput]
	[StaticConstructorOnStartup]
	public static class MeshPool
	{
		private const int MaxGridMeshSize = 15;

		private const float HumanlikeBodyWidth = 1.5f;

		private const float HumanlikeHeadAverageWidth = 1.5f;

		private const float HumanlikeHeadNarrowWidth = 1.3f;

		public static readonly GraphicMeshSet humanlikeBodySet = new GraphicMeshSet(1.5f);

		public static readonly GraphicMeshSet humanlikeHeadSet = new GraphicMeshSet(1.5f);

		public static readonly GraphicMeshSet humanlikeHairSetAverage = new GraphicMeshSet(1.5f);

		public static readonly GraphicMeshSet humanlikeHairSetNarrow = new GraphicMeshSet(1.3f, 1.5f);

		public static readonly Mesh plane025 = MeshMakerPlanes.NewPlaneMesh(0.25f);

		public static readonly Mesh plane03 = MeshMakerPlanes.NewPlaneMesh(0.3f);

		public static readonly Mesh plane05 = MeshMakerPlanes.NewPlaneMesh(0.5f);

		public static readonly Mesh plane08 = MeshMakerPlanes.NewPlaneMesh(0.8f);

		public static readonly Mesh plane10 = MeshMakerPlanes.NewPlaneMesh(1f);

		public static readonly Mesh plane10Back = MeshMakerPlanes.NewPlaneMesh(1f, false, true);

		public static readonly Mesh plane10Flip = MeshMakerPlanes.NewPlaneMesh(1f, true);

		public static readonly Mesh plane14 = MeshMakerPlanes.NewPlaneMesh(1.4f);

		public static readonly Mesh plane20 = MeshMakerPlanes.NewPlaneMesh(2f);

		public static readonly Mesh wholeMapPlane;

		private static Dictionary<Vector2, Mesh> planes = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		private static Dictionary<Vector2, Mesh> planesFlip = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		public static readonly Mesh circle = MeshMakerCircles.MakeCircleMesh(1f);

		public static readonly Mesh[] pies = new Mesh[361];

		static MeshPool()
		{
			for (int i = 0; i < 361; i++)
			{
				MeshPool.pies[i] = MeshMakerCircles.MakePieMesh(i);
			}
			MeshPool.wholeMapPlane = MeshMakerPlanes.NewWholeMapPlane();
		}

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

		private static Vector2 RoundedToHundredths(this Vector2 v)
		{
			return new Vector2((float)((int)(v.x * 100f)) / 100f, (float)((int)(v.y * 100f)) / 100f);
		}

		[Category("System")]
		[DebugOutput]
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
