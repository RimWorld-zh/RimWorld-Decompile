using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Noise;

namespace RimWorld
{
	internal class RiverMaker
	{
		private ModuleBase generator;

		private ModuleBase coordinateX;

		private ModuleBase coordinateZ;

		private ModuleBase shallowizer;

		private float surfaceLevel;

		private float shallowFactor = 0.2f;

		private List<IntVec3> lhs = new List<IntVec3>();

		private List<IntVec3> rhs = new List<IntVec3>();

		public RiverMaker(Vector3 center, float angle, RiverDef riverDef)
		{
			this.surfaceLevel = (float)(riverDef.widthOnMap / 2.0);
			this.coordinateX = new AxisAsValueX();
			this.coordinateZ = new AxisAsValueZ();
			this.coordinateX = new Rotate(0.0, 0.0 - angle, 0.0, this.coordinateX);
			this.coordinateZ = new Rotate(0.0, 0.0 - angle, 0.0, this.coordinateZ);
			this.coordinateX = new Translate(0.0 - center.x, 0.0, 0.0 - center.z, this.coordinateX);
			this.coordinateZ = new Translate(0.0 - center.x, 0.0, 0.0 - center.z, this.coordinateZ);
			ModuleBase moduleBase = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, 2147483647), QualityMode.Medium);
			ModuleBase moduleBase2 = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, 2147483647), QualityMode.Medium);
			ModuleBase moduleBase3 = new Const(8.0);
			moduleBase = new Multiply(moduleBase, moduleBase3);
			moduleBase2 = new Multiply(moduleBase2, moduleBase3);
			this.coordinateX = new Displace(this.coordinateX, moduleBase, new Const(0.0), moduleBase2);
			this.coordinateZ = new Displace(this.coordinateZ, moduleBase, new Const(0.0), moduleBase2);
			this.generator = this.coordinateX;
			this.shallowizer = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, 2147483647), QualityMode.Medium);
			this.shallowizer = new Abs(this.shallowizer);
		}

		public TerrainDef TerrainAt(IntVec3 loc, bool recordForValidation = false)
		{
			float value = this.generator.GetValue(loc);
			float num = this.surfaceLevel - Mathf.Abs(value);
			if (num > 2.0 && this.shallowizer.GetValue(loc) > this.shallowFactor)
			{
				return TerrainDefOf.WaterMovingDeep;
			}
			if (num > 0.0)
			{
				if (recordForValidation)
				{
					if (value < 0.0)
					{
						this.lhs.Add(loc);
					}
					else
					{
						this.rhs.Add(loc);
					}
				}
				return TerrainDefOf.WaterMovingShallow;
			}
			return null;
		}

		public Vector3 WaterCoordinateAt(IntVec3 loc)
		{
			return new Vector3(this.coordinateX.GetValue(loc), 0f, this.coordinateZ.GetValue(loc));
		}

		public void ValidatePassage(Map map)
		{
			IntVec3 intVec = (from loc in this.lhs
			where loc.InBounds(map) && loc.GetTerrain(map) == TerrainDefOf.WaterMovingShallow
			select loc).RandomElementWithFallback(IntVec3.Invalid);
			IntVec3 intVec2 = (from loc in this.rhs
			where loc.InBounds(map) && loc.GetTerrain(map) == TerrainDefOf.WaterMovingShallow
			select loc).RandomElementWithFallback(IntVec3.Invalid);
			if (!(intVec == IntVec3.Invalid) && !(intVec2 == IntVec3.Invalid))
			{
				while (true)
				{
					if (!map.reachability.CanReach(intVec, intVec2, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings))
					{
						if (!(this.shallowFactor > 1.0))
						{
							this.shallowFactor += 0.1f;
							foreach (IntVec3 allCell in map.AllCells)
							{
								if (allCell.GetTerrain(map) == TerrainDefOf.WaterMovingDeep && this.shallowizer.GetValue(allCell) <= this.shallowFactor)
								{
									map.terrainGrid.SetTerrain(allCell, TerrainDefOf.WaterMovingShallow);
								}
							}
							continue;
						}
						break;
					}
					return;
				}
				Log.Error("Failed to make river shallow enough for passability");
			}
			else
			{
				Log.Error("Failed to find river edges in order to verify passability");
			}
		}
	}
}
