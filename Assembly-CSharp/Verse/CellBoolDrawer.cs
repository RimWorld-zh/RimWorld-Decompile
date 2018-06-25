using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C02 RID: 3074
	public class CellBoolDrawer
	{
		// Token: 0x04002DF3 RID: 11763
		public ICellBoolGiver giver;

		// Token: 0x04002DF4 RID: 11764
		private bool wantDraw = false;

		// Token: 0x04002DF5 RID: 11765
		private Material material;

		// Token: 0x04002DF6 RID: 11766
		private bool materialCaresAboutVertexColors;

		// Token: 0x04002DF7 RID: 11767
		private bool dirty = true;

		// Token: 0x04002DF8 RID: 11768
		private List<Mesh> meshes = new List<Mesh>();

		// Token: 0x04002DF9 RID: 11769
		private int mapSizeX;

		// Token: 0x04002DFA RID: 11770
		private int mapSizeZ;

		// Token: 0x04002DFB RID: 11771
		private float opacity = 0.33f;

		// Token: 0x04002DFC RID: 11772
		private static List<Vector3> verts = new List<Vector3>();

		// Token: 0x04002DFD RID: 11773
		private static List<int> tris = new List<int>();

		// Token: 0x04002DFE RID: 11774
		private static List<Color> colors = new List<Color>();

		// Token: 0x04002DFF RID: 11775
		private const float DefaultOpacity = 0.33f;

		// Token: 0x04002E00 RID: 11776
		private const int MaxCellsPerMesh = 16383;

		// Token: 0x06004338 RID: 17208 RVA: 0x00238AB8 File Offset: 0x00236EB8
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, float opacity = 0.33f)
		{
			this.giver = giver;
			this.mapSizeX = mapSizeX;
			this.mapSizeZ = mapSizeZ;
			this.opacity = opacity;
		}

		// Token: 0x06004339 RID: 17209 RVA: 0x00238B0D File Offset: 0x00236F0D
		public void MarkForDraw()
		{
			this.wantDraw = true;
		}

		// Token: 0x0600433A RID: 17210 RVA: 0x00238B17 File Offset: 0x00236F17
		public void CellBoolDrawerUpdate()
		{
			if (this.wantDraw)
			{
				this.ActuallyDraw();
				this.wantDraw = false;
			}
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x00238B34 File Offset: 0x00236F34
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

		// Token: 0x0600433C RID: 17212 RVA: 0x00238B93 File Offset: 0x00236F93
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x00238BA0 File Offset: 0x00236FA0
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

		// Token: 0x0600433E RID: 17214 RVA: 0x00238E40 File Offset: 0x00237240
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

		// Token: 0x0600433F RID: 17215 RVA: 0x00238EA0 File Offset: 0x002372A0
		private void CreateMaterialIfNeeded(bool careAboutVertexColors)
		{
			if (this.material == null || this.materialCaresAboutVertexColors != careAboutVertexColors)
			{
				this.material = SolidColorMaterials.SimpleSolidColorMaterial(new Color(this.giver.Color.r, this.giver.Color.g, this.giver.Color.b, this.opacity * this.giver.Color.a), careAboutVertexColors);
				this.materialCaresAboutVertexColors = careAboutVertexColors;
				this.material.renderQueue = 3600;
			}
		}
	}
}
