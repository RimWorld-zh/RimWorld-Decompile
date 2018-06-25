using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C4B RID: 3147
	internal class SectionLayer_Snow : SectionLayer
	{
		// Token: 0x04002F6C RID: 12140
		private float[] vertDepth = new float[9];

		// Token: 0x04002F6D RID: 12141
		private static readonly Color32 ColorClear = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);

		// Token: 0x04002F6E RID: 12142
		private static readonly Color32 ColorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x06004556 RID: 17750 RVA: 0x0024A3BE File Offset: 0x002487BE
		public SectionLayer_Snow(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Snow;
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06004557 RID: 17751 RVA: 0x0024A3E0 File Offset: 0x002487E0
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawSnow;
			}
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x0024A3FC File Offset: 0x002487FC
		private bool Filled(int index)
		{
			Building building = base.Map.edificeGrid[index];
			return building != null && building.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x0024A43C File Offset: 0x0024883C
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.Snow);
			if (subMesh.mesh.vertexCount == 0)
			{
				SectionLayerGeometryMaker_Solid.MakeBaseGeometry(this.section, subMesh, AltitudeLayer.Terrain);
			}
			subMesh.Clear(MeshParts.Colors);
			float[] depthGridDirect_Unsafe = base.Map.snowGrid.DepthGridDirect_Unsafe;
			CellRect cellRect = this.section.CellRect;
			int num = base.Map.Size.z - 1;
			int num2 = base.Map.Size.x - 1;
			bool flag = false;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					float num3 = depthGridDirect_Unsafe[cellIndices.CellToIndex(i, j)];
					int num4 = cellIndices.CellToIndex(i, j - 1);
					float num5 = (j <= 0) ? num3 : depthGridDirect_Unsafe[num4];
					num4 = cellIndices.CellToIndex(i - 1, j - 1);
					float num6 = (j <= 0 || i <= 0) ? num3 : depthGridDirect_Unsafe[num4];
					num4 = cellIndices.CellToIndex(i - 1, j);
					float num7 = (i <= 0) ? num3 : depthGridDirect_Unsafe[num4];
					num4 = cellIndices.CellToIndex(i - 1, j + 1);
					float num8 = (j >= num || i <= 0) ? num3 : depthGridDirect_Unsafe[num4];
					num4 = cellIndices.CellToIndex(i, j + 1);
					float num9 = (j >= num) ? num3 : depthGridDirect_Unsafe[num4];
					num4 = cellIndices.CellToIndex(i + 1, j + 1);
					float num10 = (j >= num || i >= num2) ? num3 : depthGridDirect_Unsafe[num4];
					num4 = cellIndices.CellToIndex(i + 1, j);
					float num11 = (i >= num2) ? num3 : depthGridDirect_Unsafe[num4];
					num4 = cellIndices.CellToIndex(i + 1, j - 1);
					float num12 = (j <= 0 || i >= num2) ? num3 : depthGridDirect_Unsafe[num4];
					this.vertDepth[0] = (num5 + num6 + num7 + num3) / 4f;
					this.vertDepth[1] = (num7 + num3) / 2f;
					this.vertDepth[2] = (num7 + num8 + num9 + num3) / 4f;
					this.vertDepth[3] = (num9 + num3) / 2f;
					this.vertDepth[4] = (num9 + num10 + num11 + num3) / 4f;
					this.vertDepth[5] = (num11 + num3) / 2f;
					this.vertDepth[6] = (num11 + num12 + num5 + num3) / 4f;
					this.vertDepth[7] = (num5 + num3) / 2f;
					this.vertDepth[8] = num3;
					for (int k = 0; k < 9; k++)
					{
						if (this.vertDepth[k] > 0.01f)
						{
							flag = true;
						}
						subMesh.colors.Add(SectionLayer_Snow.SnowDepthColor(this.vertDepth[k]));
					}
				}
			}
			if (flag)
			{
				subMesh.disabled = false;
				subMesh.FinalizeMesh(MeshParts.Colors);
			}
			else
			{
				subMesh.disabled = true;
			}
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x0024A7A0 File Offset: 0x00248BA0
		private static Color32 SnowDepthColor(float snowDepth)
		{
			return Color32.Lerp(SectionLayer_Snow.ColorClear, SectionLayer_Snow.ColorWhite, snowDepth);
		}
	}
}
