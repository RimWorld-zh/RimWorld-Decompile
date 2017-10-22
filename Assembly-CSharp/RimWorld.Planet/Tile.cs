using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public class Tile
	{
		public struct RoadLink
		{
			public int neighbor;

			public RoadDef road;
		}

		public struct RiverLink
		{
			public int neighbor;

			public RiverDef river;
		}

		public const int Invalid = -1;

		public BiomeDef biome = null;

		public float elevation = 100f;

		public Hilliness hilliness = Hilliness.Undefined;

		public float temperature = 20f;

		public float rainfall = 0f;

		public float swampiness;

		public WorldFeature feature;

		public List<RoadLink> roads = null;

		public List<RiverLink> rivers = null;

		public bool WaterCovered
		{
			get
			{
				return this.elevation <= 0.0;
			}
		}

		public List<RoadLink> VisibleRoads
		{
			get
			{
				return (!this.biome.allowRoads) ? null : this.roads;
			}
		}

		public List<RiverLink> VisibleRivers
		{
			get
			{
				return (!this.biome.allowRivers) ? null : this.rivers;
			}
		}

		public override string ToString()
		{
			return "(" + this.biome + " elev=" + this.elevation + "m hill=" + this.hilliness + " temp=" + this.temperature + "°C rain=" + this.rainfall + "mm swampiness=" + this.swampiness.ToStringPercent() + " roads=" + ((this.roads != null) ? this.roads.Count : 0) + " rivers=" + ((this.rivers != null) ? this.rivers.Count : 0) + ")";
		}
	}
}
