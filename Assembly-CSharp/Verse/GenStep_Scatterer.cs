using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
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

		public bool allowInWaterBiome = true;

		public bool warnOnFail = true;

		[Unsaved]
		protected List<IntVec3> usedSpots = new List<IntVec3>();

		private const int ScatterNearPlayerRadius = 20;

		protected GenStep_Scatterer()
		{
		}

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
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}

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

		public static int CountFromPer10kCells(float countPer10kCells, Map map, int mapSize = -1)
		{
			if (mapSize < 0)
			{
				mapSize = map.Size.x;
			}
			int num = Mathf.RoundToInt(10000f / countPer10kCells);
			return Mathf.RoundToInt((float)(mapSize * mapSize) / (float)num);
		}

		public void ForceScatterAt(IntVec3 loc, Map map)
		{
			this.ScatterAt(loc, map, 1);
		}

		[CompilerGenerated]
		private sealed class <TryFindScatterCell>c__AnonStorey0
		{
			internal Map map;

			internal GenStep_Scatterer $this;

			public <TryFindScatterCell>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return this.$this.CanScatterAt(x, this.map);
			}

			internal bool <>m__1(IntVec3 x)
			{
				return this.$this.CanScatterAt(x, this.map);
			}

			internal bool <>m__2(IntVec3 x)
			{
				return this.$this.CanScatterAt(x, this.map);
			}
		}
	}
}
