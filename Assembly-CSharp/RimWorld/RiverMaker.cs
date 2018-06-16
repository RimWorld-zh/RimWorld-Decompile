using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000405 RID: 1029
	internal class RiverMaker
	{
		// Token: 0x060011B3 RID: 4531 RVA: 0x00099FC8 File Offset: 0x000983C8
		public RiverMaker(Vector3 center, float angle, RiverDef riverDef)
		{
			this.surfaceLevel = riverDef.widthOnMap / 2f;
			this.coordinateX = new AxisAsValueX();
			this.coordinateZ = new AxisAsValueZ();
			this.coordinateX = new Rotate(0.0, (double)(-(double)angle), 0.0, this.coordinateX);
			this.coordinateZ = new Rotate(0.0, (double)(-(double)angle), 0.0, this.coordinateZ);
			this.coordinateX = new Translate((double)(-(double)center.x), 0.0, (double)(-(double)center.z), this.coordinateX);
			this.coordinateZ = new Translate((double)(-(double)center.x), 0.0, (double)(-(double)center.z), this.coordinateZ);
			ModuleBase x = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			ModuleBase z = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			ModuleBase moduleBase = new Const(8.0);
			x = new Multiply(x, moduleBase);
			z = new Multiply(z, moduleBase);
			this.coordinateX = new Displace(this.coordinateX, x, new Const(0.0), z);
			this.coordinateZ = new Displace(this.coordinateZ, x, new Const(0.0), z);
			this.generator = this.coordinateX;
			this.shallowizer = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			this.shallowizer = new Abs(this.shallowizer);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0009A1D8 File Offset: 0x000985D8
		public TerrainDef TerrainAt(IntVec3 loc, bool recordForValidation = false)
		{
			float value = this.generator.GetValue(loc);
			float num = this.surfaceLevel - Mathf.Abs(value);
			TerrainDef result;
			if (num > 2f && this.shallowizer.GetValue(loc) > this.shallowFactor)
			{
				result = TerrainDefOf.WaterMovingChestDeep;
			}
			else if (num > 0f)
			{
				if (recordForValidation)
				{
					if (value < 0f)
					{
						this.lhs.Add(loc);
					}
					else
					{
						this.rhs.Add(loc);
					}
				}
				result = TerrainDefOf.WaterMovingShallow;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0009A280 File Offset: 0x00098680
		public Vector3 WaterCoordinateAt(IntVec3 loc)
		{
			return new Vector3(this.coordinateX.GetValue(loc), 0f, this.coordinateZ.GetValue(loc));
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0009A2B8 File Offset: 0x000986B8
		public void ValidatePassage(Map map)
		{
			IntVec3 intVec = (from loc in this.lhs
			where loc.InBounds(map) && loc.GetTerrain(map) == TerrainDefOf.WaterMovingShallow
			select loc).RandomElementWithFallback(IntVec3.Invalid);
			IntVec3 intVec2 = (from loc in this.rhs
			where loc.InBounds(map) && loc.GetTerrain(map) == TerrainDefOf.WaterMovingShallow
			select loc).RandomElementWithFallback(IntVec3.Invalid);
			if (intVec == IntVec3.Invalid || intVec2 == IntVec3.Invalid)
			{
				Log.Error("Failed to find river edges in order to verify passability", false);
			}
			else
			{
				while (!map.reachability.CanReach(intVec, intVec2, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings))
				{
					if (this.shallowFactor > 1f)
					{
						Log.Error("Failed to make river shallow enough for passability", false);
						break;
					}
					this.shallowFactor += 0.1f;
					foreach (IntVec3 intVec3 in map.AllCells)
					{
						if (intVec3.GetTerrain(map) == TerrainDefOf.WaterMovingChestDeep && this.shallowizer.GetValue(intVec3) <= this.shallowFactor)
						{
							map.terrainGrid.SetTerrain(intVec3, TerrainDefOf.WaterMovingShallow);
						}
					}
				}
			}
		}

		// Token: 0x04000AC9 RID: 2761
		private ModuleBase generator;

		// Token: 0x04000ACA RID: 2762
		private ModuleBase coordinateX;

		// Token: 0x04000ACB RID: 2763
		private ModuleBase coordinateZ;

		// Token: 0x04000ACC RID: 2764
		private ModuleBase shallowizer;

		// Token: 0x04000ACD RID: 2765
		private float surfaceLevel;

		// Token: 0x04000ACE RID: 2766
		private float shallowFactor = 0.2f;

		// Token: 0x04000ACF RID: 2767
		private List<IntVec3> lhs = new List<IntVec3>();

		// Token: 0x04000AD0 RID: 2768
		private List<IntVec3> rhs = new List<IntVec3>();
	}
}
