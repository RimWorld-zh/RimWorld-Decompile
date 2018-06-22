using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C4A RID: 3146
	internal class SectionLayer_Terrain : SectionLayer
	{
		// Token: 0x0600455D RID: 17757 RVA: 0x0024AA69 File Offset: 0x00248E69
		public SectionLayer_Terrain(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x0600455E RID: 17758 RVA: 0x0024AA7C File Offset: 0x00248E7C
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x0024AA98 File Offset: 0x00248E98
		public virtual Material GetMaterialFor(TerrainDef terrain)
		{
			return terrain.DrawMatSingle;
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x0024AAB4 File Offset: 0x00248EB4
		public bool AllowRenderingFor(TerrainDef terrain)
		{
			return DebugViewSettings.drawTerrainWater || !terrain.HasTag("Water");
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x0024AAE4 File Offset: 0x00248EE4
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			TerrainGrid terrainGrid = base.Map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			TerrainDef[] array = new TerrainDef[8];
			HashSet<TerrainDef> hashSet = new HashSet<TerrainDef>();
			bool[] array2 = new bool[8];
			foreach (IntVec3 intVec in cellRect)
			{
				hashSet.Clear();
				TerrainDef terrainDef = terrainGrid.TerrainAt(intVec);
				LayerSubMesh subMesh = base.GetSubMesh(this.GetMaterialFor(terrainDef));
				if (subMesh != null && this.AllowRenderingFor(terrainDef))
				{
					int count = subMesh.verts.Count;
					subMesh.verts.Add(new Vector3((float)intVec.x, 0f, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)intVec.x, 0f, (float)(intVec.z + 1)));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)(intVec.z + 1)));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)intVec.z));
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 1);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count + 3);
				}
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = intVec + GenAdj.AdjacentCellsAroundBottom[i];
					if (!c.InBounds(base.Map))
					{
						array[i] = terrainDef;
					}
					else
					{
						TerrainDef terrainDef2 = terrainGrid.TerrainAt(c);
						Thing edifice = c.GetEdifice(base.Map);
						if (edifice != null && edifice.def.coversFloor)
						{
							terrainDef2 = TerrainDefOf.Underwall;
						}
						array[i] = terrainDef2;
						if (terrainDef2 != terrainDef && terrainDef2.edgeType != TerrainDef.TerrainEdgeType.Hard && terrainDef2.renderPrecedence >= terrainDef.renderPrecedence)
						{
							if (!hashSet.Contains(terrainDef2))
							{
								hashSet.Add(terrainDef2);
							}
						}
					}
				}
				foreach (TerrainDef terrainDef3 in hashSet)
				{
					LayerSubMesh subMesh2 = base.GetSubMesh(this.GetMaterialFor(terrainDef3));
					if (subMesh2 != null && this.AllowRenderingFor(terrainDef3))
					{
						int count = subMesh2.verts.Count;
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)intVec.z + 0.5f));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)intVec.z + 0.5f));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)intVec.z + 0.5f));
						for (int j = 0; j < 8; j++)
						{
							array2[j] = false;
						}
						for (int k = 0; k < 8; k++)
						{
							if (k % 2 == 0)
							{
								if (array[k] == terrainDef3)
								{
									array2[(k - 1 + 8) % 8] = true;
									array2[k] = true;
									array2[(k + 1) % 8] = true;
								}
							}
							else if (array[k] == terrainDef3)
							{
								array2[k] = true;
							}
						}
						for (int l = 0; l < 8; l++)
						{
							if (array2[l])
							{
								subMesh2.colors.Add(SectionLayer_Terrain.ColorWhite);
							}
							else
							{
								subMesh2.colors.Add(SectionLayer_Terrain.ColorClear);
							}
						}
						subMesh2.colors.Add(SectionLayer_Terrain.ColorClear);
						for (int m = 0; m < 8; m++)
						{
							subMesh2.tris.Add(count + m);
							subMesh2.tris.Add(count + (m + 1) % 8);
							subMesh2.tris.Add(count + 8);
						}
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x04002F69 RID: 12137
		private static readonly Color32 ColorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04002F6A RID: 12138
		private static readonly Color32 ColorClear = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
	}
}
