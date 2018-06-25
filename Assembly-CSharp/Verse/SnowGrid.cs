using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C25 RID: 3109
	public sealed class SnowGrid : IExposable
	{
		// Token: 0x04002E67 RID: 11879
		private Map map;

		// Token: 0x04002E68 RID: 11880
		private float[] depthGrid;

		// Token: 0x04002E69 RID: 11881
		private double totalDepth = 0.0;

		// Token: 0x04002E6A RID: 11882
		public const float MaxDepth = 1f;

		// Token: 0x06004437 RID: 17463 RVA: 0x0023E8F7 File Offset: 0x0023CCF7
		public SnowGrid(Map map)
		{
			this.map = map;
			this.depthGrid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06004438 RID: 17464 RVA: 0x0023E92C File Offset: 0x0023CD2C
		internal float[] DepthGridDirect_Unsafe
		{
			get
			{
				return this.depthGrid;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06004439 RID: 17465 RVA: 0x0023E948 File Offset: 0x0023CD48
		public float TotalDepth
		{
			get
			{
				return (float)this.totalDepth;
			}
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x0023E964 File Offset: 0x0023CD64
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => SnowGrid.SnowFloatToShort(this.GetDepth(c)), delegate(IntVec3 c, ushort val)
			{
				this.depthGrid[this.map.cellIndices.CellToIndex(c)] = SnowGrid.SnowShortToFloat(val);
			}, "depthGrid");
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x0023E990 File Offset: 0x0023CD90
		private static ushort SnowFloatToShort(float depth)
		{
			depth = Mathf.Clamp(depth, 0f, 1f);
			depth *= 65535f;
			return (ushort)Mathf.RoundToInt(depth);
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x0023E9C8 File Offset: 0x0023CDC8
		private static float SnowShortToFloat(ushort depth)
		{
			return (float)depth / 65535f;
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x0023E9E8 File Offset: 0x0023CDE8
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

		// Token: 0x0600443E RID: 17470 RVA: 0x0023EA58 File Offset: 0x0023CE58
		public static bool CanCoexistWithSnow(ThingDef def)
		{
			return def.category != ThingCategory.Building || def.Fillage != FillCategory.Full;
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x0023EA90 File Offset: 0x0023CE90
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

		// Token: 0x06004440 RID: 17472 RVA: 0x0023EB5C File Offset: 0x0023CF5C
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

		// Token: 0x06004441 RID: 17473 RVA: 0x0023EBDC File Offset: 0x0023CFDC
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

		// Token: 0x06004442 RID: 17474 RVA: 0x0023EC90 File Offset: 0x0023D090
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

		// Token: 0x06004443 RID: 17475 RVA: 0x0023ECDC File Offset: 0x0023D0DC
		public SnowCategory GetCategory(IntVec3 c)
		{
			return SnowUtility.GetSnowCategory(this.GetDepth(c));
		}
	}
}
