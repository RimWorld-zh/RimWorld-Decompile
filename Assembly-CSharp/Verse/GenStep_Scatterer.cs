using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public abstract class GenStep_Scatterer : GenStep
	{
		public int count = -1;

		public FloatRange countPer10kCellsRange = FloatRange.Zero;

		public bool nearPlayerStart = false;

		public bool nearMapCenter = false;

		public float minSpacing = 10f;

		public bool spotMustBeStandable = false;

		public int minDistToPlayerStart = 0;

		public int minEdgeDist = 0;

		public int extraNoBuildEdgeDist = 0;

		public List<ScattererValidator> validators = new List<ScattererValidator>();

		public bool allowOnWater = true;

		public bool warnOnFail = true;

		[Unsaved]
		protected List<IntVec3> usedSpots = new List<IntVec3>();

		private const int ScatterNearPlayerRadius = 20;

		public override void Generate(Map map)
		{
			if (!this.allowOnWater && map.TileInfo.WaterCovered)
				return;
			int num = this.CalculateFinalCount(map);
			int num2 = 0;
			while (num2 < num)
			{
				IntVec3 intVec = default(IntVec3);
				if (this.TryFindScatterCell(map, out intVec))
				{
					this.ScatterAt(intVec, map, 1);
					this.usedSpots.Add(intVec);
					num2++;
					continue;
				}
				return;
			}
			this.usedSpots.Clear();
		}

		protected virtual bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			bool result2;
			if (this.nearMapCenter)
			{
				if (RCellFinder.TryFindRandomCellNearWith(map.Center, (Predicate<IntVec3>)((IntVec3 x) => this.CanScatterAt(x, map)), map, out result, 3, 2147483647))
				{
					result2 = true;
					goto IL_00e6;
				}
			}
			else
			{
				if (this.nearPlayerStart)
				{
					result = CellFinder.RandomClosewalkCellNear(MapGenerator.PlayerStartSpot, map, 20, (Predicate<IntVec3>)((IntVec3 x) => this.CanScatterAt(x, map)));
					result2 = true;
					goto IL_00e6;
				}
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith(5, (Predicate<IntVec3>)((IntVec3 x) => this.CanScatterAt(x, map)), map, out result))
				{
					result2 = true;
					goto IL_00e6;
				}
			}
			if (this.warnOnFail)
			{
				Log.Warning("Scatterer " + this.ToString() + " could not find cell to generate at.");
			}
			result2 = false;
			goto IL_00e6;
			IL_00e6:
			return result2;
		}

		protected abstract void ScatterAt(IntVec3 loc, Map map, int count = 1);

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
							goto IL_00de;
					}
				}
				result = true;
			}
			goto IL_0103;
			IL_00de:
			result = false;
			goto IL_0103;
			IL_0103:
			return result;
		}

		protected bool NearUsedSpot(IntVec3 c, float dist)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.usedSpots.Count)
				{
					if ((float)(this.usedSpots[num] - c).LengthHorizontalSquared <= dist * dist)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		protected int CalculateFinalCount(Map map)
		{
			return (this.count >= 0) ? this.count : GenStep_Scatterer.CountFromPer10kCells(this.countPer10kCellsRange.RandomInRange, map, -1);
		}

		public static int CountFromPer10kCells(float countPer10kCells, Map map, int mapSize = -1)
		{
			if (mapSize < 0)
			{
				IntVec3 size = map.Size;
				mapSize = size.x;
			}
			int num = Mathf.RoundToInt((float)(10000.0 / countPer10kCells));
			return Mathf.RoundToInt((float)(mapSize * mapSize) / (float)num);
		}

		public void ForceScatterAt(IntVec3 loc, Map map)
		{
			this.ScatterAt(loc, map, 1);
		}
	}
}
