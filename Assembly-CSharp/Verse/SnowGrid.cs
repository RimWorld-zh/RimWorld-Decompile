using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C26 RID: 3110
	public sealed class SnowGrid : IExposable
	{
		// Token: 0x04002E6E RID: 11886
		private Map map;

		// Token: 0x04002E6F RID: 11887
		private float[] depthGrid;

		// Token: 0x04002E70 RID: 11888
		private double totalDepth = 0.0;

		// Token: 0x04002E71 RID: 11889
		public const float MaxDepth = 1f;

		// Token: 0x06004437 RID: 17463 RVA: 0x0023EBD7 File Offset: 0x0023CFD7
		public SnowGrid(Map map)
		{
			this.map = map;
			this.depthGrid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06004438 RID: 17464 RVA: 0x0023EC0C File Offset: 0x0023D00C
		internal float[] DepthGridDirect_Unsafe
		{
			get
			{
				return this.depthGrid;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06004439 RID: 17465 RVA: 0x0023EC28 File Offset: 0x0023D028
		public float TotalDepth
		{
			get
			{
				return (float)this.totalDepth;
			}
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x0023EC44 File Offset: 0x0023D044
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => SnowGrid.SnowFloatToShort(this.GetDepth(c)), delegate(IntVec3 c, ushort val)
			{
				this.depthGrid[this.map.cellIndices.CellToIndex(c)] = SnowGrid.SnowShortToFloat(val);
			}, "depthGrid");
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x0023EC70 File Offset: 0x0023D070
		private static ushort SnowFloatToShort(float depth)
		{
			depth = Mathf.Clamp(depth, 0f, 1f);
			depth *= 65535f;
			return (ushort)Mathf.RoundToInt(depth);
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x0023ECA8 File Offset: 0x0023D0A8
		private static float SnowShortToFloat(ushort depth)
		{
			return (float)depth / 65535f;
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x0023ECC8 File Offset: 0x0023D0C8
		private bool CanHaveSnow(int ind)
		{
			Building building = this.map.edificeGrid[ind];
			bool result;
			if (building != null && !SnowGrid.CanCoexistWithSnow(building.def))
			{
				result = false;
			}
			else
			{
				TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(ind);
				result = (terrainDef == null || terrainDef.holdSnow);
			}
			return result;
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x0023ED38 File Offset: 0x0023D138
		public static bool CanCoexistWithSnow(ThingDef def)
		{
			return def.category != ThingCategory.Building || def.Fillage != FillCategory.Full;
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x0023ED70 File Offset: 0x0023D170
		public void AddDepth(IntVec3 c, float depthToAdd)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			float num2 = this.depthGrid[num];
			if (num2 > 0f || depthToAdd >= 0f)
			{
				if (num2 < 0.999f || depthToAdd <= 1f)
				{
					if (!this.CanHaveSnow(num))
					{
						this.depthGrid[num] = 0f;
					}
					else
					{
						float num3 = num2 + depthToAdd;
						num3 = Mathf.Clamp(num3, 0f, 1f);
						float num4 = num3 - num2;
						this.totalDepth += (double)num4;
						if (Mathf.Abs(num4) > 0.0001f)
						{
							this.depthGrid[num] = num3;
							this.CheckVisualOrPathCostChange(c, num2, num3);
						}
					}
				}
			}
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x0023EE3C File Offset: 0x0023D23C
		public void SetDepth(IntVec3 c, float newDepth)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (!this.CanHaveSnow(num))
			{
				this.depthGrid[num] = 0f;
			}
			else
			{
				newDepth = Mathf.Clamp(newDepth, 0f, 1f);
				float num2 = this.depthGrid[num];
				this.depthGrid[num] = newDepth;
				float num3 = newDepth - num2;
				this.totalDepth += (double)num3;
				this.CheckVisualOrPathCostChange(c, num2, newDepth);
			}
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x0023EEBC File Offset: 0x0023D2BC
		private void CheckVisualOrPathCostChange(IntVec3 c, float oldDepth, float newDepth)
		{
			if (!Mathf.Approximately(oldDepth, newDepth))
			{
				if (Mathf.Abs(oldDepth - newDepth) > 0.15f || Rand.Value < 0.0125f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Snow, true, false);
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Things, true, false);
				}
				else if (newDepth == 0f)
				{
					this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Snow, true, false);
				}
				if (SnowUtility.GetSnowCategory(oldDepth) != SnowUtility.GetSnowCategory(newDepth))
				{
					this.map.pathGrid.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x0023EF70 File Offset: 0x0023D370
		public float GetDepth(IntVec3 c)
		{
			float result;
			if (!c.InBounds(this.map))
			{
				result = 0f;
			}
			else
			{
				result = this.depthGrid[this.map.cellIndices.CellToIndex(c)];
			}
			return result;
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x0023EFBC File Offset: 0x0023D3BC
		public SnowCategory GetCategory(IntVec3 c)
		{
			return SnowUtility.GetSnowCategory(this.GetDepth(c));
		}
	}
}
