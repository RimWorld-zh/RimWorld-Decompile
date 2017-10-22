using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	internal class RiverMaker
	{
		private ModuleBase generator;

		private ModuleBase shallowizer;

		private float surfaceLevel;

		public RiverMaker(Vector3 center, float angleA, float angleB, RiverDef riverDef)
		{
			this.surfaceLevel = (float)(riverDef.widthOnMap / 2.0);
			this.generator = new DistFromAxis(1f);
			this.generator = new Bend((float)((angleB - (angleA + 180.0) + 360.0) % 360.0), 50f, this.generator);
			this.generator = new Rotate(0.0, 0.0 - angleA, 0.0, this.generator);
			this.generator = new Translate(0.0 - center.x, 0.0, 0.0 - center.z, this.generator);
			ModuleBase lhs = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, 2147483647), QualityMode.Medium);
			ModuleBase lhs2 = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, 2147483647), QualityMode.Medium);
			ModuleBase rhs = new Const(8.0);
			lhs = new Multiply(lhs, rhs);
			lhs2 = new Multiply(lhs2, rhs);
			this.generator = new Displace(this.generator, lhs, new Const(0.0), lhs2);
			this.shallowizer = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, 2147483647), QualityMode.Medium);
			this.shallowizer = new Abs(this.shallowizer);
		}

		public TerrainDef TerrainAt(IntVec3 loc)
		{
			float value = this.generator.GetValue(loc);
			if (value < this.surfaceLevel - 2.0 && this.shallowizer.GetValue(loc) > 0.20000000298023224)
			{
				return TerrainDefOf.WaterMovingDeep;
			}
			if (value < this.surfaceLevel)
			{
				return TerrainDefOf.WaterMovingShallow;
			}
			return null;
		}
	}
}
