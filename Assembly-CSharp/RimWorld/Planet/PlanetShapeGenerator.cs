using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005B7 RID: 1463
	public static class PlanetShapeGenerator
	{
		// Token: 0x040010C6 RID: 4294
		private static int subdivisionsCount;

		// Token: 0x040010C7 RID: 4295
		private static float radius;

		// Token: 0x040010C8 RID: 4296
		private static Vector3 viewCenter;

		// Token: 0x040010C9 RID: 4297
		private static float viewAngle;

		// Token: 0x040010CA RID: 4298
		private static List<TriangleIndices> tris = new List<TriangleIndices>();

		// Token: 0x040010CB RID: 4299
		private static List<Vector3> verts = new List<Vector3>();

		// Token: 0x040010CC RID: 4300
		private static List<Vector3> finalVerts;

		// Token: 0x040010CD RID: 4301
		private static List<int> tileIDToFinalVerts_offsets;

		// Token: 0x040010CE RID: 4302
		private static List<int> tileIDToNeighbors_offsets;

		// Token: 0x040010CF RID: 4303
		private static List<int> tileIDToNeighbors_values;

		// Token: 0x040010D0 RID: 4304
		private static List<TriangleIndices> newTris = new List<TriangleIndices>();

		// Token: 0x040010D1 RID: 4305
		private static List<int> generatedTileVerts = new List<int>();

		// Token: 0x040010D2 RID: 4306
		private static List<int> adjacentTris = new List<int>();

		// Token: 0x040010D3 RID: 4307
		private static List<int> tmpTileIDs = new List<int>();

		// Token: 0x040010D4 RID: 4308
		private static List<int> tmpVerts = new List<int>();

		// Token: 0x040010D5 RID: 4309
		private static List<int> tmpNeighborsToAdd = new List<int>();

		// Token: 0x040010D6 RID: 4310
		private static List<int> vertToTris_offsets = new List<int>();

		// Token: 0x040010D7 RID: 4311
		private static List<int> vertToTris_values = new List<int>();

		// Token: 0x040010D8 RID: 4312
		private static List<int> vertToTileIDs_offsets = new List<int>();

		// Token: 0x040010D9 RID: 4313
		private static List<int> vertToTileIDs_values = new List<int>();

		// Token: 0x040010DA RID: 4314
		private static List<int> tileIDToVerts_offsets = new List<int>();

		// Token: 0x040010DB RID: 4315
		private static List<int> tileIDToVerts_values = new List<int>();

		// Token: 0x040010DC RID: 4316
		private const int MaxTileVertices = 6;

		// Token: 0x06001C14 RID: 7188 RVA: 0x000F1C40 File Offset: 0x000F0040
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

		// Token: 0x06001C15 RID: 7189 RVA: 0x000F1C80 File Offset: 0x000F0080
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

		// Token: 0x06001C16 RID: 7190 RVA: 0x000F1CE9 File Offset: 0x000F00E9
		private static void ClearOrCreateMeshStaticData()
		{
			PlanetShapeGenerator.tris.Clear();
			PlanetShapeGenerator.verts.Clear();
			PlanetShapeGenerator.finalVerts = new List<Vector3>();
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x000F1D0A File Offset: 0x000F010A
		private static void CreateTileInfoStaticData()
		{
			PlanetShapeGenerator.tileIDToFinalVerts_offsets = new List<int>();
			PlanetShapeGenerator.tileIDToNeighbors_offsets = new List<int>();
			PlanetShapeGenerator.tileIDToNeighbors_values = new List<int>();
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x000F1D2C File Offset: 0x000F012C
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

		// Token: 0x06001C19 RID: 7193 RVA: 0x000F1DC6 File Offset: 0x000F01C6
		private static void ClearAndDeallocate<T>(ref List<T> list)
		{
			list.Clear();
			list.TrimExcess();
			list = new List<T>();
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x000F1DE0 File Offset: 0x000F01E0
		private static void Subdivide(bool lastPass)
		{
			PackedListOfLists.GenerateVertToTrisPackedList(PlanetShapeGenerator.verts, PlanetShapeGenerator.tris, PlanetShapeGenerator.vertToTris_offsets, PlanetShapeGenerator.vertToTris_values);
			int count = PlanetShapeGenerator.verts.Count;
			int i = 0;
			int count2 = PlanetShapeGenerator.tris.Count;
			while (i < count2)
			{
				TriangleIndices triangleIndices = PlanetShapeGenerator.tris[i];
				Vector3 vector = (PlanetShapeGenerator.verts[triangleIndices.v1] + PlanetShapeGenerator.verts[triangleIndices.v2] + PlanetShapeGenerator.verts[triangleIndices.v3]) / 3f;
				PlanetShapeGenerator.verts.Add(vector.normalized * PlanetShapeGenerator.radius);
				i++;
			}
			PlanetShapeGenerator.newTris.Clear();
			if (lastPass)
			{
				PlanetShapeGenerator.vertToTileIDs_offsets.Clear();
				PlanetShapeGenerator.vertToTileIDs_values.Clear();
				PlanetShapeGenerator.tileIDToVerts_offsets.Clear();
				PlanetShapeGenerator.tileIDToVerts_values.Clear();
				int j = 0;
				int count3 = PlanetShapeGenerator.verts.Count;
				while (j < count3)
				{
					PlanetShapeGenerator.vertToTileIDs_offsets.Add(PlanetShapeGenerator.vertToTileIDs_values.Count);
					if (j >= count)
					{
						for (int k = 0; k < 6; k++)
						{
							PlanetShapeGenerator.vertToTileIDs_values.Add(-1);
						}
					}
					j++;
				}
			}
			int l = 0;
			while (l < count)
			{
				PackedListOfLists.GetList<int>(PlanetShapeGenerator.vertToTris_offsets, PlanetShapeGenerator.vertToTris_values, l, PlanetShapeGenerator.adjacentTris);
				int count4 = PlanetShapeGenerator.adjacentTris.Count;
				if (!lastPass)
				{
					for (int m = 0; m < count4; m++)
					{
						int num = PlanetShapeGenerator.adjacentTris[m];
						int v = count + num;
						int nextOrderedVertex = PlanetShapeGenerator.tris[num].GetNextOrderedVertex(l);
						int num2 = -1;
						for (int n = 0; n < count4; n++)
						{
							if (m != n)
							{
								TriangleIndices triangleIndices2 = PlanetShapeGenerator.tris[PlanetShapeGenerator.adjacentTris[n]];
								if (triangleIndices2.v1 == nextOrderedVertex || triangleIndices2.v2 == nextOrderedVertex || triangleIndices2.v3 == nextOrderedVertex)
								{
									num2 = PlanetShapeGenerator.adjacentTris[n];
									break;
								}
							}
						}
						if (num2 >= 0)
						{
							int v2 = count + num2;
							PlanetShapeGenerator.newTris.Add(new TriangleIndices(l, v2, v));
						}
					}
				}
				else if (count4 == 5 || count4 == 6)
				{
					int num3 = 0;
					int nextOrderedVertex2 = PlanetShapeGenerator.tris[PlanetShapeGenerator.adjacentTris[num3]].GetNextOrderedVertex(l);
					int num4 = num3;
					int currentTriangleVertex = nextOrderedVertex2;
					PlanetShapeGenerator.generatedTileVerts.Clear();
					for (int num5 = 0; num5 < count4; num5++)
					{
						int item = count + PlanetShapeGenerator.adjacentTris[num4];
						PlanetShapeGenerator.generatedTileVerts.Add(item);
						int nextAdjacentTriangle = PlanetShapeGenerator.GetNextAdjacentTriangle(num4, currentTriangleVertex, PlanetShapeGenerator.adjacentTris);
						int nextOrderedVertex3 = PlanetShapeGenerator.tris[PlanetShapeGenerator.adjacentTris[nextAdjacentTriangle]].GetNextOrderedVertex(l);
						num4 = nextAdjacentTriangle;
						currentTriangleVertex = nextOrderedVertex3;
					}
					PlanetShapeGenerator.FinalizeGeneratedTile(PlanetShapeGenerator.generatedTileVerts);
				}
				IL_344:
				l++;
				continue;
				goto IL_344;
			}
			PlanetShapeGenerator.tris.Clear();
			PlanetShapeGenerator.tris.AddRange(PlanetShapeGenerator.newTris);
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x000F2158 File Offset: 0x000F0558
		private static void FinalizeGeneratedTile(List<int> generatedTileVerts)
		{
			if ((generatedTileVerts.Count != 5 && generatedTileVerts.Count != 6) || generatedTileVerts.Count > 6)
			{
				Log.Error("Planet shape generation internal error: generated a tile with " + generatedTileVerts.Count + " vertices. Only 5 and 6 are allowed.", false);
			}
			else if (!PlanetShapeGenerator.ShouldDiscardGeneratedTile(generatedTileVerts))
			{
				int count = PlanetShapeGenerator.tileIDToFinalVerts_offsets.Count;
				PlanetShapeGenerator.tileIDToFinalVerts_offsets.Add(PlanetShapeGenerator.finalVerts.Count);
				int i = 0;
				int count2 = generatedTileVerts.Count;
				while (i < count2)
				{
					int index = generatedTileVerts[i];
					PlanetShapeGenerator.finalVerts.Add(PlanetShapeGenerator.verts[index]);
					PlanetShapeGenerator.vertToTileIDs_values[PlanetShapeGenerator.vertToTileIDs_values.IndexOf(-1, PlanetShapeGenerator.vertToTileIDs_offsets[index])] = count;
					i++;
				}
				PackedListOfLists.AddList<int>(PlanetShapeGenerator.tileIDToVerts_offsets, PlanetShapeGenerator.tileIDToVerts_values, generatedTileVerts);
			}
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x000F2248 File Offset: 0x000F0648
		private static bool ShouldDiscardGeneratedTile(List<int> generatedTileVerts)
		{
			Vector3 a = Vector3.zero;
			int i = 0;
			int count = generatedTileVerts.Count;
			while (i < count)
			{
				a += PlanetShapeGenerator.verts[generatedTileVerts[i]];
				i++;
			}
			return !MeshUtility.VisibleForWorldgen(a / (float)generatedTileVerts.Count, PlanetShapeGenerator.radius, PlanetShapeGenerator.viewCenter, PlanetShapeGenerator.viewAngle);
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x000F22BC File Offset: 0x000F06BC
		private static void CalculateTileNeighbors()
		{
			List<int> list = new List<int>();
			int i = 0;
			int count = PlanetShapeGenerator.tileIDToVerts_offsets.Count;
			while (i < count)
			{
				PlanetShapeGenerator.tmpNeighborsToAdd.Clear();
				PackedListOfLists.GetList<int>(PlanetShapeGenerator.tileIDToVerts_offsets, PlanetShapeGenerator.tileIDToVerts_values, i, PlanetShapeGenerator.tmpVerts);
				int j = 0;
				int count2 = PlanetShapeGenerator.tmpVerts.Count;
				while (j < count2)
				{
					PackedListOfLists.GetList<int>(PlanetShapeGenerator.vertToTileIDs_offsets, PlanetShapeGenerator.vertToTileIDs_values, PlanetShapeGenerator.tmpVerts[j], PlanetShapeGenerator.tmpTileIDs);
					PackedListOfLists.GetList<int>(PlanetShapeGenerator.vertToTileIDs_offsets, PlanetShapeGenerator.vertToTileIDs_values, PlanetShapeGenerator.tmpVerts[(j + 1) % PlanetShapeGenerator.tmpVerts.Count], list);
					int k = 0;
					int count3 = PlanetShapeGenerator.tmpTileIDs.Count;
					while (k < count3)
					{
						int num = PlanetShapeGenerator.tmpTileIDs[k];
						if (num != i)
						{
							if (num != -1)
							{
								if (list.Contains(num))
								{
									PlanetShapeGenerator.tmpNeighborsToAdd.Add(num);
								}
							}
						}
						k++;
					}
					j++;
				}
				PackedListOfLists.AddList<int>(PlanetShapeGenerator.tileIDToNeighbors_offsets, PlanetShapeGenerator.tileIDToNeighbors_values, PlanetShapeGenerator.tmpNeighborsToAdd);
				i++;
			}
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x000F23F8 File Offset: 0x000F07F8
		private static int GetNextAdjacentTriangle(int currentAdjTriangleIndex, int currentTriangleVertex, List<int> adjacentTris)
		{
			int i = 0;
			int count = adjacentTris.Count;
			while (i < count)
			{
				if (currentAdjTriangleIndex != i)
				{
					TriangleIndices triangleIndices = PlanetShapeGenerator.tris[adjacentTris[i]];
					if (triangleIndices.v1 == currentTriangleVertex || triangleIndices.v2 == currentTriangleVertex || triangleIndices.v3 == currentTriangleVertex)
					{
						return i;
					}
				}
				i++;
			}
			Log.Error("Planet shape generation internal error: could not find next adjacent triangle.", false);
			return -1;
		}
	}
}
