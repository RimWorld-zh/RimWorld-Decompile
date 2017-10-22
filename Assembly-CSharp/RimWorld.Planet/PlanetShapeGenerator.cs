using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class PlanetShapeGenerator
	{
		private const int MaxTileVertices = 6;

		private static int subdivisionsCount;

		private static float radius;

		private static Vector3 viewCenter;

		private static float viewAngle;

		private static List<TriangleIndices> tris = new List<TriangleIndices>();

		private static List<Vector3> verts = new List<Vector3>();

		private static List<Vector3> finalVerts;

		private static List<int> tileIDToFinalVerts_offsets;

		private static List<int> tileIDToNeighbors_offsets;

		private static List<int> tileIDToNeighbors_values;

		private static List<TriangleIndices> newTris = new List<TriangleIndices>();

		private static List<int> generatedTileVerts = new List<int>();

		private static List<int> adjacentTris = new List<int>();

		private static List<int> tmpTileIDs = new List<int>();

		private static List<int> tmpVerts = new List<int>();

		private static List<int> tmpNeighborsToAdd = new List<int>();

		private static List<int> vertToTris_offsets = new List<int>();

		private static List<int> vertToTris_values = new List<int>();

		private static List<int> vertToTileIDs_offsets = new List<int>();

		private static List<int> vertToTileIDs_values = new List<int>();

		private static List<int> tileIDToVerts_offsets = new List<int>();

		private static List<int> tileIDToVerts_values = new List<int>();

		public static void Generate(int subdivisionsCount, out List<Vector3> outVerts, out List<int> outTileIDToVerts_offsets, out List<int> outTileIDToNeighbors_offsets, out List<int> outTileIDToNeighbors_values, float radius, Vector3 viewCenter, float viewAngle)
		{
			PlanetShapeGenerator.subdivisionsCount = subdivisionsCount;
			PlanetShapeGenerator.radius = radius;
			PlanetShapeGenerator.viewCenter = viewCenter;
			PlanetShapeGenerator.viewAngle = viewAngle;
			PlanetShapeGenerator.DoGenerate();
			outVerts = PlanetShapeGenerator.finalVerts;
			outTileIDToVerts_offsets = PlanetShapeGenerator.tileIDToFinalVerts_offsets;
			outTileIDToNeighbors_offsets = PlanetShapeGenerator.tileIDToNeighbors_offsets;
			outTileIDToNeighbors_values = PlanetShapeGenerator.tileIDToNeighbors_values;
		}

		private static void DoGenerate()
		{
			PlanetShapeGenerator.ClearOrCreateMeshStaticData();
			PlanetShapeGenerator.CreateTileInfoStaticData();
			IcosahedronGenerator.GenerateIcosahedron(PlanetShapeGenerator.verts, PlanetShapeGenerator.tris, PlanetShapeGenerator.radius, PlanetShapeGenerator.viewCenter, PlanetShapeGenerator.viewAngle);
			for (int i = 0; i < PlanetShapeGenerator.subdivisionsCount + 1; i++)
			{
				bool lastPass = i == PlanetShapeGenerator.subdivisionsCount;
				PlanetShapeGenerator.Subdivide(lastPass);
			}
			PlanetShapeGenerator.CalculateTileNeighbors();
			PlanetShapeGenerator.ClearAndDeallocateWorkingLists();
		}

		private static void ClearOrCreateMeshStaticData()
		{
			PlanetShapeGenerator.tris.Clear();
			PlanetShapeGenerator.verts.Clear();
			PlanetShapeGenerator.finalVerts = new List<Vector3>();
		}

		private static void CreateTileInfoStaticData()
		{
			PlanetShapeGenerator.tileIDToFinalVerts_offsets = new List<int>();
			PlanetShapeGenerator.tileIDToNeighbors_offsets = new List<int>();
			PlanetShapeGenerator.tileIDToNeighbors_values = new List<int>();
		}

		private static void ClearAndDeallocateWorkingLists()
		{
			PlanetShapeGenerator.ClearAndDeallocate<TriangleIndices>(ref PlanetShapeGenerator.tris);
			PlanetShapeGenerator.ClearAndDeallocate<Vector3>(ref PlanetShapeGenerator.verts);
			PlanetShapeGenerator.ClearAndDeallocate<TriangleIndices>(ref PlanetShapeGenerator.newTris);
			PlanetShapeGenerator.ClearAndDeallocate<int>(ref PlanetShapeGenerator.generatedTileVerts);
			PlanetShapeGenerator.ClearAndDeallocate<int>(ref PlanetShapeGenerator.adjacentTris);
			PlanetShapeGenerator.ClearAndDeallocate<int>(ref PlanetShapeGenerator.tmpTileIDs);
			PlanetShapeGenerator.ClearAndDeallocate<int>(ref PlanetShapeGenerator.tmpVerts);
			PlanetShapeGenerator.ClearAndDeallocate<int>(ref PlanetShapeGenerator.tmpNeighborsToAdd);
			PlanetShapeGenerator.ClearAndDeallocate<int>(ref PlanetShapeGenerator.vertToTris_offsets);
			PlanetShapeGenerator.ClearAndDeallocate<int>(ref PlanetShapeGenerator.vertToTris_values);
			PlanetShapeGenerator.ClearAndDeallocate<int>(ref PlanetShapeGenerator.vertToTileIDs_offsets);
			PlanetShapeGenerator.ClearAndDeallocate<int>(ref PlanetShapeGenerator.vertToTileIDs_values);
			PlanetShapeGenerator.ClearAndDeallocate<int>(ref PlanetShapeGenerator.tileIDToVerts_offsets);
			PlanetShapeGenerator.ClearAndDeallocate<int>(ref PlanetShapeGenerator.tileIDToVerts_values);
		}

		private static void ClearAndDeallocate<T>(ref List<T> list)
		{
			list.Clear();
			list.TrimExcess();
			list = new List<T>();
		}

		private static void Subdivide(bool lastPass)
		{
			PackedListOfLists.GenerateVertToTrisPackedList(PlanetShapeGenerator.verts, PlanetShapeGenerator.tris, PlanetShapeGenerator.vertToTris_offsets, PlanetShapeGenerator.vertToTris_values);
			int count = PlanetShapeGenerator.verts.Count;
			int num = 0;
			int count2 = PlanetShapeGenerator.tris.Count;
			while (num < count2)
			{
				TriangleIndices triangleIndices = PlanetShapeGenerator.tris[num];
				Vector3 vector = (PlanetShapeGenerator.verts[triangleIndices.v1] + PlanetShapeGenerator.verts[triangleIndices.v2] + PlanetShapeGenerator.verts[triangleIndices.v3]) / 3f;
				PlanetShapeGenerator.verts.Add(vector.normalized * PlanetShapeGenerator.radius);
				num++;
			}
			PlanetShapeGenerator.newTris.Clear();
			if (lastPass)
			{
				PlanetShapeGenerator.vertToTileIDs_offsets.Clear();
				PlanetShapeGenerator.vertToTileIDs_values.Clear();
				PlanetShapeGenerator.tileIDToVerts_offsets.Clear();
				PlanetShapeGenerator.tileIDToVerts_values.Clear();
				int num2 = 0;
				int count3 = PlanetShapeGenerator.verts.Count;
				while (num2 < count3)
				{
					PlanetShapeGenerator.vertToTileIDs_offsets.Add(PlanetShapeGenerator.vertToTileIDs_values.Count);
					if (num2 >= count)
					{
						for (int i = 0; i < 6; i++)
						{
							PlanetShapeGenerator.vertToTileIDs_values.Add(-1);
						}
					}
					num2++;
				}
			}
			for (int num3 = 0; num3 < count; num3++)
			{
				PackedListOfLists.GetList(PlanetShapeGenerator.vertToTris_offsets, PlanetShapeGenerator.vertToTris_values, num3, PlanetShapeGenerator.adjacentTris);
				int count4 = PlanetShapeGenerator.adjacentTris.Count;
				if (!lastPass)
				{
					for (int num4 = 0; num4 < count4; num4++)
					{
						int num5 = PlanetShapeGenerator.adjacentTris[num4];
						int v = count + num5;
						int nextOrderedVertex = PlanetShapeGenerator.tris[num5].GetNextOrderedVertex(num3);
						int num6 = -1;
						for (int j = 0; j < count4; j++)
						{
							if (num4 == j)
							{
								continue;
							}
							TriangleIndices triangleIndices2 = PlanetShapeGenerator.tris[PlanetShapeGenerator.adjacentTris[j]];
							if (triangleIndices2.v1 != nextOrderedVertex && triangleIndices2.v2 != nextOrderedVertex && triangleIndices2.v3 != nextOrderedVertex)
							{
								continue;
							}
							num6 = PlanetShapeGenerator.adjacentTris[j];
							break;
						}
						if (num6 >= 0)
						{
							int v2 = count + num6;
							PlanetShapeGenerator.newTris.Add(new TriangleIndices(num3, v2, v));
						}
					}
				}
				else if (count4 == 5 || count4 == 6)
				{
					int num7 = 0;
					int nextOrderedVertex2 = PlanetShapeGenerator.tris[PlanetShapeGenerator.adjacentTris[num7]].GetNextOrderedVertex(num3);
					int num8 = num7;
					int currentTriangleVertex = nextOrderedVertex2;
					PlanetShapeGenerator.generatedTileVerts.Clear();
					for (int num9 = 0; num9 < count4; num9++)
					{
						int item = count + PlanetShapeGenerator.adjacentTris[num8];
						PlanetShapeGenerator.generatedTileVerts.Add(item);
						int nextAdjancetTriangle = PlanetShapeGenerator.GetNextAdjancetTriangle(num8, currentTriangleVertex, PlanetShapeGenerator.adjacentTris);
						int nextOrderedVertex3 = PlanetShapeGenerator.tris[PlanetShapeGenerator.adjacentTris[nextAdjancetTriangle]].GetNextOrderedVertex(num3);
						num8 = nextAdjancetTriangle;
						currentTriangleVertex = nextOrderedVertex3;
					}
					PlanetShapeGenerator.FinalizeGeneratedTile(PlanetShapeGenerator.generatedTileVerts);
				}
			}
			PlanetShapeGenerator.tris.Clear();
			PlanetShapeGenerator.tris.AddRange(PlanetShapeGenerator.newTris);
		}

		private static void FinalizeGeneratedTile(List<int> generatedTileVerts)
		{
			if (generatedTileVerts.Count != 5 && generatedTileVerts.Count != 6)
			{
				goto IL_0024;
			}
			if (generatedTileVerts.Count > 6)
				goto IL_0024;
			if (!PlanetShapeGenerator.ShouldDiscardGeneratedTile(generatedTileVerts))
			{
				int count = PlanetShapeGenerator.tileIDToFinalVerts_offsets.Count;
				PlanetShapeGenerator.tileIDToFinalVerts_offsets.Add(PlanetShapeGenerator.finalVerts.Count);
				int num = 0;
				int count2 = generatedTileVerts.Count;
				while (num < count2)
				{
					int index = generatedTileVerts[num];
					PlanetShapeGenerator.finalVerts.Add(PlanetShapeGenerator.verts[index]);
					PlanetShapeGenerator.vertToTileIDs_values[PlanetShapeGenerator.vertToTileIDs_values.IndexOf(-1, PlanetShapeGenerator.vertToTileIDs_offsets[index])] = count;
					num++;
				}
				PackedListOfLists.AddList(PlanetShapeGenerator.tileIDToVerts_offsets, PlanetShapeGenerator.tileIDToVerts_values, generatedTileVerts);
			}
			return;
			IL_0024:
			Log.Error("Planet shape generation internal error: generated a tile with " + generatedTileVerts.Count + " vertices. Only 5 and 6 are allowed.");
		}

		private static bool ShouldDiscardGeneratedTile(List<int> generatedTileVerts)
		{
			Vector3 a = Vector3.zero;
			int num = 0;
			int count = generatedTileVerts.Count;
			while (num < count)
			{
				a += PlanetShapeGenerator.verts[generatedTileVerts[num]];
				num++;
			}
			return !MeshUtility.Visible(a / (float)generatedTileVerts.Count, PlanetShapeGenerator.radius, PlanetShapeGenerator.viewCenter, PlanetShapeGenerator.viewAngle);
		}

		private static void CalculateTileNeighbors()
		{
			List<int> list = new List<int>();
			int num = 0;
			int count = PlanetShapeGenerator.tileIDToVerts_offsets.Count;
			while (num < count)
			{
				PlanetShapeGenerator.tmpNeighborsToAdd.Clear();
				PackedListOfLists.GetList(PlanetShapeGenerator.tileIDToVerts_offsets, PlanetShapeGenerator.tileIDToVerts_values, num, PlanetShapeGenerator.tmpVerts);
				int num2 = 0;
				int count2 = PlanetShapeGenerator.tmpVerts.Count;
				while (num2 < count2)
				{
					PackedListOfLists.GetList(PlanetShapeGenerator.vertToTileIDs_offsets, PlanetShapeGenerator.vertToTileIDs_values, PlanetShapeGenerator.tmpVerts[num2], PlanetShapeGenerator.tmpTileIDs);
					PackedListOfLists.GetList(PlanetShapeGenerator.vertToTileIDs_offsets, PlanetShapeGenerator.vertToTileIDs_values, PlanetShapeGenerator.tmpVerts[(num2 + 1) % PlanetShapeGenerator.tmpVerts.Count], list);
					int num3 = 0;
					int count3 = PlanetShapeGenerator.tmpTileIDs.Count;
					while (num3 < count3)
					{
						int num4 = PlanetShapeGenerator.tmpTileIDs[num3];
						if (num4 != num && num4 != -1 && list.Contains(num4))
						{
							PlanetShapeGenerator.tmpNeighborsToAdd.Add(num4);
						}
						num3++;
					}
					num2++;
				}
				PackedListOfLists.AddList(PlanetShapeGenerator.tileIDToNeighbors_offsets, PlanetShapeGenerator.tileIDToNeighbors_values, PlanetShapeGenerator.tmpNeighborsToAdd);
				num++;
			}
		}

		private static int GetNextAdjancetTriangle(int currentAdjTriangleIndex, int currentTriangleVertex, List<int> adjacentTris)
		{
			int i = 0;
			int count = adjacentTris.Count;
			for (; i < count; i++)
			{
				if (currentAdjTriangleIndex != i)
				{
					TriangleIndices triangleIndices = PlanetShapeGenerator.tris[adjacentTris[i]];
					if (triangleIndices.v1 != currentTriangleVertex && triangleIndices.v2 != currentTriangleVertex && triangleIndices.v3 != currentTriangleVertex)
					{
						continue;
					}
					return i;
				}
			}
			Log.Error("Planet shape generation internal error: could not find next adjacent triangle.");
			return -1;
		}
	}
}
