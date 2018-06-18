using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C4E RID: 3150
	public class SectionLayer_TerrainScatter : SectionLayer
	{
		// Token: 0x0600455A RID: 17754 RVA: 0x00249D82 File Offset: 0x00248182
		public SectionLayer_TerrainScatter(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x0600455B RID: 17755 RVA: 0x00249DA0 File Offset: 0x002481A0
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x00249DBC File Offset: 0x002481BC
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

		// Token: 0x04002F61 RID: 12129
		private List<SectionLayer_TerrainScatter.Scatterable> scats = new List<SectionLayer_TerrainScatter.Scatterable>();

		// Token: 0x02000C4F RID: 3151
		private class Scatterable
		{
			// Token: 0x0600455E RID: 17758 RVA: 0x00249FF0 File Offset: 0x002483F0
			public Scatterable(ScatterableDef def, Vector3 loc, Map map)
			{
				this.def = def;
				this.loc = loc;
				this.map = map;
				this.size = Rand.Range(def.minSize, def.maxSize);
				this.rotation = Rand.Range(0f, 360f);
			}

			// Token: 0x0600455F RID: 17759 RVA: 0x0024A048 File Offset: 0x00248448
			public void PrintOnto(SectionLayer layer)
			{
				Printer_Plane.PrintPlane(layer, this.loc, Vector2.one * this.size, this.def.mat, this.rotation, false, null, null, 0.01f, 0f);
			}

			// Token: 0x17000AF1 RID: 2801
			// (get) Token: 0x06004560 RID: 17760 RVA: 0x0024A090 File Offset: 0x00248490
			public bool IsOnValidTerrain
			{
				get
				{
					IntVec3 c = this.loc.ToIntVec3();
					TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
					return this.def.scatterType == terrainDef.scatterType && !c.Filled(this.map);
				}
			}

			// Token: 0x04002F63 RID: 12131
			private Map map;

			// Token: 0x04002F64 RID: 12132
			public ScatterableDef def;

			// Token: 0x04002F65 RID: 12133
			public Vector3 loc;

			// Token: 0x04002F66 RID: 12134
			public float size;

			// Token: 0x04002F67 RID: 12135
			public float rotation;
		}
	}
}
