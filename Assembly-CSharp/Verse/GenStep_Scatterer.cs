using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C5D RID: 3165
	public abstract class GenStep_Scatterer : GenStep
	{
		// Token: 0x04002F98 RID: 12184
		public int count = -1;

		// Token: 0x04002F99 RID: 12185
		public FloatRange countPer10kCellsRange = FloatRange.Zero;

		// Token: 0x04002F9A RID: 12186
		public bool nearPlayerStart = false;

		// Token: 0x04002F9B RID: 12187
		public bool nearMapCenter = false;

		// Token: 0x04002F9C RID: 12188
		public float minSpacing = 10f;

		// Token: 0x04002F9D RID: 12189
		public bool spotMustBeStandable = false;

		// Token: 0x04002F9E RID: 12190
		public int minDistToPlayerStart = 0;

		// Token: 0x04002F9F RID: 12191
		public int minEdgeDist = 0;

		// Token: 0x04002FA0 RID: 12192
		public int extraNoBuildEdgeDist = 0;

		// Token: 0x04002FA1 RID: 12193
		public List<ScattererValidator> validators = new List<ScattererValidator>();

		// Token: 0x04002FA2 RID: 12194
		public bool allowInWaterBiome = true;

		// Token: 0x04002FA3 RID: 12195
		public bool warnOnFail = true;

		// Token: 0x04002FA4 RID: 12196
		[Unsaved]
		protected List<IntVec3> usedSpots = new List<IntVec3>();

		// Token: 0x04002FA5 RID: 12197
		private const int ScatterNearPlayerRadius = 20;

		// Token: 0x0600459F RID: 17823 RVA: 0x00091D44 File Offset: 0x00090144
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

		// Token: 0x060045A0 RID: 17824 RVA: 0x00091DC4 File Offset: 0x000901C4
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

		// Token: 0x060045A1 RID: 17825
		protected abstract void ScatterAt(IntVec3 loc, Map map, int count = 1);

		// Token: 0x060045A2 RID: 17826 RVA: 0x00091EBC File Offset: 0x000902BC
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

		// Token: 0x060045A3 RID: 17827 RVA: 0x00091FD0 File Offset: 0x000903D0
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

		// Token: 0x060045A4 RID: 17828 RVA: 0x00092030 File Offset: 0x00090430
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

		// Token: 0x060045A5 RID: 17829 RVA: 0x00092070 File Offset: 0x00090470
		public static int CountFromPer10kCells(float countPer10kCells, Map map, int mapSize = -1)
		{
			if (mapSize < 0)
			{
				mapSize = map.Size.x;
			}
			int num = Mathf.RoundToInt(10000f / countPer10kCells);
			return Mathf.RoundToInt((float)(mapSize * mapSize) / (float)num);
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x000920B5 File Offset: 0x000904B5
		public void ForceScatterAt(IntVec3 loc, Map map)
		{
			this.ScatterAt(loc, map, 1);
		}
	}
}
