using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C5E RID: 3166
	public abstract class GenStep_Scatterer : GenStep
	{
		// Token: 0x06004595 RID: 17813 RVA: 0x000919F8 File Offset: 0x0008FDF8
		public override void Generate(Map map)
		{
			if (this.allowInWaterBiome || !map.TileInfo.WaterCovered)
			{
				int num = this.CalculateFinalCount(map);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec;
					if (!this.TryFindScatterCell(map, out intVec))
					{
						return;
					}
					this.ScatterAt(intVec, map, 1);
					this.usedSpots.Add(intVec);
				}
				this.usedSpots.Clear();
			}
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x00091A78 File Offset: 0x0008FE78
		protected virtual bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (this.nearMapCenter)
			{
				if (RCellFinder.TryFindRandomCellNearWith(map.Center, (IntVec3 x) => this.CanScatterAt(x, map), map, out result, 3, 2147483647))
				{
					return true;
				}
			}
			else
			{
				if (this.nearPlayerStart)
				{
					result = CellFinder.RandomClosewalkCellNear(MapGenerator.PlayerStartSpot, map, 20, (IntVec3 x) => this.CanScatterAt(x, map));
					return true;
				}
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith(5, (IntVec3 x) => this.CanScatterAt(x, map), map, out result))
				{
					return true;
				}
			}
			if (this.warnOnFail)
			{
				Log.Warning("Scatterer " + this.ToString() + " could not find cell to generate at.", false);
			}
			return false;
		}

		// Token: 0x06004597 RID: 17815
		protected abstract void ScatterAt(IntVec3 loc, Map map, int count = 1);

		// Token: 0x06004598 RID: 17816 RVA: 0x00091B70 File Offset: 0x0008FF70
		protected virtual bool CanScatterAt(IntVec3 loc, Map map)
		{
			bool result;
			if (this.extraNoBuildEdgeDist > 0 && loc.CloseToEdge(map, this.extraNoBuildEdgeDist + 10))
			{
				result = false;
			}
			else if (this.minEdgeDist > 0 && loc.CloseToEdge(map, this.minEdgeDist))
			{
				result = false;
			}
			else if (this.NearUsedSpot(loc, this.minSpacing))
			{
				result = false;
			}
			else if ((map.Center - loc).LengthHorizontalSquared < this.minDistToPlayerStart * this.minDistToPlayerStart)
			{
				result = false;
			}
			else if (this.spotMustBeStandable && !loc.Standable(map))
			{
				result = false;
			}
			else
			{
				if (this.validators != null)
				{
					for (int i = 0; i < this.validators.Count; i++)
					{
						if (!this.validators[i].Allows(loc, map))
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x00091C84 File Offset: 0x00090084
		protected bool NearUsedSpot(IntVec3 c, float dist)
		{
			for (int i = 0; i < this.usedSpots.Count; i++)
			{
				if ((float)(this.usedSpots[i] - c).LengthHorizontalSquared <= dist * dist)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x00091CE4 File Offset: 0x000900E4
		protected int CalculateFinalCount(Map map)
		{
			int result;
			if (this.count < 0)
			{
				result = GenStep_Scatterer.CountFromPer10kCells(this.countPer10kCellsRange.RandomInRange, map, -1);
			}
			else
			{
				result = this.count;
			}
			return result;
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x00091D24 File Offset: 0x00090124
		public static int CountFromPer10kCells(float countPer10kCells, Map map, int mapSize = -1)
		{
			if (mapSize < 0)
			{
				mapSize = map.Size.x;
			}
			int num = Mathf.RoundToInt(10000f / countPer10kCells);
			return Mathf.RoundToInt((float)(mapSize * mapSize) / (float)num);
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x00091D69 File Offset: 0x00090169
		public void ForceScatterAt(IntVec3 loc, Map map)
		{
			this.ScatterAt(loc, map, 1);
		}

		// Token: 0x04002F89 RID: 12169
		public int count = -1;

		// Token: 0x04002F8A RID: 12170
		public FloatRange countPer10kCellsRange = FloatRange.Zero;

		// Token: 0x04002F8B RID: 12171
		public bool nearPlayerStart = false;

		// Token: 0x04002F8C RID: 12172
		public bool nearMapCenter = false;

		// Token: 0x04002F8D RID: 12173
		public float minSpacing = 10f;

		// Token: 0x04002F8E RID: 12174
		public bool spotMustBeStandable = false;

		// Token: 0x04002F8F RID: 12175
		public int minDistToPlayerStart = 0;

		// Token: 0x04002F90 RID: 12176
		public int minEdgeDist = 0;

		// Token: 0x04002F91 RID: 12177
		public int extraNoBuildEdgeDist = 0;

		// Token: 0x04002F92 RID: 12178
		public List<ScattererValidator> validators = new List<ScattererValidator>();

		// Token: 0x04002F93 RID: 12179
		public bool allowInWaterBiome = true;

		// Token: 0x04002F94 RID: 12180
		public bool warnOnFail = true;

		// Token: 0x04002F95 RID: 12181
		[Unsaved]
		protected List<IntVec3> usedSpots = new List<IntVec3>();

		// Token: 0x04002F96 RID: 12182
		private const int ScatterNearPlayerRadius = 20;
	}
}
