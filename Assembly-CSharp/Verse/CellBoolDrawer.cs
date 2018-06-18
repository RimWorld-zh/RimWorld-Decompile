using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C02 RID: 3074
	public class CellBoolDrawer
	{
		// Token: 0x0600432C RID: 17196 RVA: 0x0023736C File Offset: 0x0023576C
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, float opacity = 0.33f)
		{
			this.giver = giver;
			this.mapSizeX = mapSizeX;
			this.mapSizeZ = mapSizeZ;
			this.opacity = opacity;
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x002373C1 File Offset: 0x002357C1
		public void MarkForDraw()
		{
			this.wantDraw = true;
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x002373CB File Offset: 0x002357CB
		public void CellBoolDrawerUpdate()
		{
			if (this.wantDraw)
			{
				this.ActuallyDraw();
				this.wantDraw = false;
			}
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x002373E8 File Offset: 0x002357E8
		private void ActuallyDraw()
		{
			if (this.dirty)
			{
				this.RegenerateMesh();
			}
			for (int i = 0; i < this.meshes.Count; i++)
			{
				Graphics.DrawMesh(this.meshes[i], Vector3.zero, Quaternion.identity, this.material, 0);
			}
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x00237447 File Offset: 0x00235847
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x00237454 File Offset: 0x00235854
		public void RegenerateMesh()
		{
			for (int i = 0; i < this.meshes.Count; i++)
			{
				this.meshes[i].Clear();
			}
			int num = 0;
			int num2 = 0;
			if (this.meshes.Count < num + 1)
			{
				Mesh mesh = new Mesh();
				mesh.name = "CellBoolDrawer";
				this.meshes.Add(mesh);
			}
			Mesh mesh2 = this.meshes[num];
			CellRect cellRect = new CellRect(0, 0, this.mapSizeX, this.mapSizeZ);
			float y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			bool careAboutVertexColors = false;
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
				{
					int index = CellIndicesUtility.CellToIndex(j, k, this.mapSizeX);
					if (this.giver.GetCellBool(index))
					{
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)k));
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)k));
						Color cellExtraColor = this.giver.GetCellExtraColor(index);
						CellBoolDrawer.colors.Add(cellExtraColor);
						CellBoolDrawer.colors.Add(cellExtraColor);
						CellBoolDrawer.colors.Add(cellExtraColor);
						CellBoolDrawer.colors.Add(cellExtraColor);
						if (cellExtraColor != Color.white)
						{
							careAboutVertexColors = true;
						}
						int count = CellBoolDrawer.verts.Count;
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 3);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 1);
						num2++;
						if (num2 >= 16383)
						{
							this.FinalizeWorkingDataIntoMesh(mesh2);
							num++;
							if (this.meshes.Count < num + 1)
							{
								Mesh mesh3 = new Mesh();
								mesh3.name = "CellBoolDrawer";
								this.meshes.Add(mesh3);
							}
							mesh2 = this.meshes[num];
							num2 = 0;
						}
					}
				}
			}
			this.FinalizeWorkingDataIntoMesh(mesh2);
			this.CreateMaterialIfNeeded(careAboutVertexColors);
			this.dirty = false;
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x002376F4 File Offset: 0x00235AF4
		private void FinalizeWorkingDataIntoMesh(Mesh mesh)
		{
			if (CellBoolDrawer.verts.Count > 0)
			{
				mesh.SetVertices(CellBoolDrawer.verts);
				CellBoolDrawer.verts.Clear();
				mesh.SetTriangles(CellBoolDrawer.tris, 0);
				CellBoolDrawer.tris.Clear();
				mesh.SetColors(CellBoolDrawer.colors);
				CellBoolDrawer.colors.Clear();
			}
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x00237754 File Offset: 0x00235B54
		private void CreateMaterialIfNeeded(bool careAboutVertexColors)
		{
			if (this.material == null || this.materialCaresAboutVertexColors != careAboutVertexColors)
			{
				this.material = SolidColorMaterials.SimpleSolidColorMaterial(new Color(this.giver.Color.r, this.giver.Color.g, this.giver.Color.b, this.opacity * this.giver.Color.a), careAboutVertexColors);
				this.materialCaresAboutVertexColors = careAboutVertexColors;
				this.material.renderQueue = 3600;
			}
		}

		// Token: 0x04002DE2 RID: 11746
		public ICellBoolGiver giver;

		// Token: 0x04002DE3 RID: 11747
		private bool wantDraw = false;

		// Token: 0x04002DE4 RID: 11748
		private Material material;

		// Token: 0x04002DE5 RID: 11749
		private bool materialCaresAboutVertexColors;

		// Token: 0x04002DE6 RID: 11750
		private bool dirty = true;

		// Token: 0x04002DE7 RID: 11751
		private List<Mesh> meshes = new List<Mesh>();

		// Token: 0x04002DE8 RID: 11752
		private int mapSizeX;

		// Token: 0x04002DE9 RID: 11753
		private int mapSizeZ;

		// Token: 0x04002DEA RID: 11754
		private float opacity = 0.33f;

		// Token: 0x04002DEB RID: 11755
		private static List<Vector3> verts = new List<Vector3>();

		// Token: 0x04002DEC RID: 11756
		private static List<int> tris = new List<int>();

		// Token: 0x04002DED RID: 11757
		private static List<Color> colors = new List<Color>();

		// Token: 0x04002DEE RID: 11758
		private const float DefaultOpacity = 0.33f;

		// Token: 0x04002DEF RID: 11759
		private const int MaxCellsPerMesh = 16383;
	}
}
