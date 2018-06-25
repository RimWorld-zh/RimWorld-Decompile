using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C4E RID: 3150
	public class SectionLayer_TerrainScatter : SectionLayer
	{
		// Token: 0x04002F72 RID: 12146
		private List<SectionLayer_TerrainScatter.Scatterable> scats = new List<SectionLayer_TerrainScatter.Scatterable>();

		// Token: 0x06004566 RID: 17766 RVA: 0x0024B50E File Offset: 0x0024990E
		public SectionLayer_TerrainScatter(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06004567 RID: 17767 RVA: 0x0024B52C File Offset: 0x0024992C
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x0024B548 File Offset: 0x00249948
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

		// Token: 0x02000C4F RID: 3151
		private class Scatterable
		{
			// Token: 0x04002F74 RID: 12148
			private Map map;

			// Token: 0x04002F75 RID: 12149
			public ScatterableDef def;

			// Token: 0x04002F76 RID: 12150
			public Vector3 loc;

			// Token: 0x04002F77 RID: 12151
			public float size;

			// Token: 0x04002F78 RID: 12152
			public float rotation;

			// Token: 0x0600456A RID: 17770 RVA: 0x0024B77C File Offset: 0x00249B7C
			public Scatterable(ScatterableDef def, Vector3 loc, Map map)
			{
				this.def = def;
				this.loc = loc;
				this.map = map;
				this.size = Rand.Range(def.minSize, def.maxSize);
				this.rotation = Rand.Range(0f, 360f);
			}

			// Token: 0x0600456B RID: 17771 RVA: 0x0024B7D4 File Offset: 0x00249BD4
			public void PrintOnto(SectionLayer layer)
			{
				Printer_Plane.PrintPlane(layer, this.loc, Vector2.one * this.size, this.def.mat, this.rotation, false, null, null, 0.01f, 0f);
			}

			// Token: 0x17000AF2 RID: 2802
			// (get) Token: 0x0600456C RID: 17772 RVA: 0x0024B81C File Offset: 0x00249C1C
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
