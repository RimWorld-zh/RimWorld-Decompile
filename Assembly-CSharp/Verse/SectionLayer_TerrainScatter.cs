using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C4D RID: 3149
	public class SectionLayer_TerrainScatter : SectionLayer
	{
		// Token: 0x04002F6B RID: 12139
		private List<SectionLayer_TerrainScatter.Scatterable> scats = new List<SectionLayer_TerrainScatter.Scatterable>();

		// Token: 0x06004566 RID: 17766 RVA: 0x0024B22E File Offset: 0x0024962E
		public SectionLayer_TerrainScatter(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06004567 RID: 17767 RVA: 0x0024B24C File Offset: 0x0024964C
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x0024B268 File Offset: 0x00249668
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			this.scats.RemoveAll((SectionLayer_TerrainScatter.Scatterable scat) => !scat.IsOnValidTerrain);
			int num = 0;
			TerrainDef[] topGrid = base.Map.terrainGrid.topGrid;
			CellRect cellRect = this.section.CellRect;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (topGrid[cellIndices.CellToIndex(j, i)].scatterType != null)
					{
						num++;
					}
				}
			}
			num /= 40;
			int num2 = 0;
			while (this.scats.Count < num && num2 < 200)
			{
				num2++;
				IntVec3 randomCell = this.section.CellRect.RandomCell;
				string terrScatType = base.Map.terrainGrid.TerrainAt(randomCell).scatterType;
				if (terrScatType != null && !randomCell.Filled(base.Map))
				{
					ScatterableDef def2;
					if ((from def in DefDatabase<ScatterableDef>.AllDefs
					where def.scatterType == terrScatType
					select def).TryRandomElement(out def2))
					{
						Vector3 loc = new Vector3((float)randomCell.x + Rand.Value, (float)randomCell.y, (float)randomCell.z + Rand.Value);
						SectionLayer_TerrainScatter.Scatterable scatterable = new SectionLayer_TerrainScatter.Scatterable(def2, loc, base.Map);
						this.scats.Add(scatterable);
						scatterable.PrintOnto(this);
					}
				}
			}
			for (int k = 0; k < this.scats.Count; k++)
			{
				this.scats[k].PrintOnto(this);
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x02000C4E RID: 3150
		private class Scatterable
		{
			// Token: 0x04002F6D RID: 12141
			private Map map;

			// Token: 0x04002F6E RID: 12142
			public ScatterableDef def;

			// Token: 0x04002F6F RID: 12143
			public Vector3 loc;

			// Token: 0x04002F70 RID: 12144
			public float size;

			// Token: 0x04002F71 RID: 12145
			public float rotation;

			// Token: 0x0600456A RID: 17770 RVA: 0x0024B49C File Offset: 0x0024989C
			public Scatterable(ScatterableDef def, Vector3 loc, Map map)
			{
				this.def = def;
				this.loc = loc;
				this.map = map;
				this.size = Rand.Range(def.minSize, def.maxSize);
				this.rotation = Rand.Range(0f, 360f);
			}

			// Token: 0x0600456B RID: 17771 RVA: 0x0024B4F4 File Offset: 0x002498F4
			public void PrintOnto(SectionLayer layer)
			{
				Printer_Plane.PrintPlane(layer, this.loc, Vector2.one * this.size, this.def.mat, this.rotation, false, null, null, 0.01f, 0f);
			}

			// Token: 0x17000AF2 RID: 2802
			// (get) Token: 0x0600456C RID: 17772 RVA: 0x0024B53C File Offset: 0x0024993C
			public bool IsOnValidTerrain
			{
				get
				{
					IntVec3 c = this.loc.ToIntVec3();
					TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
					return this.def.scatterType == terrainDef.scatterType && !c.Filled(this.map);
				}
			}
		}
	}
}
