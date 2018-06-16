using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C27 RID: 3111
	public sealed class SnowGrid : IExposable
	{
		// Token: 0x0600442D RID: 17453 RVA: 0x0023D47B File Offset: 0x0023B87B
		public SnowGrid(Map map)
		{
			this.map = map;
			this.depthGrid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x0600442E RID: 17454 RVA: 0x0023D4B0 File Offset: 0x0023B8B0
		internal float[] DepthGridDirect_Unsafe
		{
			get
			{
				return this.depthGrid;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x0600442F RID: 17455 RVA: 0x0023D4CC File Offset: 0x0023B8CC
		public float TotalDepth
		{
			get
			{
				return (float)this.totalDepth;
			}
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x0023D4E8 File Offset: 0x0023B8E8
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => SnowGrid.SnowFloatToShort(this.GetDepth(c)), delegate(IntVec3 c, ushort val)
			{
				this.depthGrid[this.map.cellIndices.CellToIndex(c)] = SnowGrid.SnowShortToFloat(val);
			}, "depthGrid");
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x0023D514 File Offset: 0x0023B914
		private static ushort SnowFloatToShort(float depth)
		{
			depth = Mathf.Clamp(depth, 0f, 1f);
			depth *= 65535f;
			return (ushort)Mathf.RoundToInt(depth);
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x0023D54C File Offset: 0x0023B94C
		private static float SnowShortToFloat(ushort depth)
		{
			return (float)depth / 65535f;
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x0023D56C File Offset: 0x0023B96C
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

		// Token: 0x06004434 RID: 17460 RVA: 0x0023D5DC File Offset: 0x0023B9DC
		public static bool CanCoexistWithSnow(ThingDef def)
		{
			return def.category != ThingCategory.Building || def.Fillage != FillCategory.Full;
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x0023D614 File Offset: 0x0023BA14
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

		// Token: 0x06004436 RID: 17462 RVA: 0x0023D6E0 File Offset: 0x0023BAE0
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

		// Token: 0x06004437 RID: 17463 RVA: 0x0023D760 File Offset: 0x0023BB60
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

		// Token: 0x06004438 RID: 17464 RVA: 0x0023D814 File Offset: 0x0023BC14
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

		// Token: 0x06004439 RID: 17465 RVA: 0x0023D860 File Offset: 0x0023BC60
		public SnowCategory GetCategory(IntVec3 c)
		{
			return SnowUtility.GetSnowCategory(this.GetDepth(c));
		}

		// Token: 0x04002E5F RID: 11871
		private Map map;

		// Token: 0x04002E60 RID: 11872
		private float[] depthGrid;

		// Token: 0x04002E61 RID: 11873
		private double totalDepth = 0.0;

		// Token: 0x04002E62 RID: 11874
		public const float MaxDepth = 1f;
	}
}
