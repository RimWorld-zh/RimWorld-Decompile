using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A8 RID: 1448
	public class Tile
	{
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001B9A RID: 7066 RVA: 0x000EE110 File Offset: 0x000EC510
		public bool WaterCovered
		{
			get
			{
				return this.elevation <= 0f;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001B9B RID: 7067 RVA: 0x000EE138 File Offset: 0x000EC538
		public List<Tile.RoadLink> Roads
		{
			get
			{
				return (!this.biome.allowRoads) ? null : this.potentialRoads;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001B9C RID: 7068 RVA: 0x000EE16C File Offset: 0x000EC56C
		public List<Tile.RiverLink> Rivers
		{
			get
			{
				return (!this.biome.allowRivers) ? null : this.potentialRivers;
			}
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x000EE1A0 File Offset: 0x000EC5A0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.biome,
				" elev=",
				this.elevation,
				"m hill=",
				this.hilliness,
				" temp=",
				this.temperature,
				"°C rain=",
				this.rainfall,
				"mm swampiness=",
				this.swampiness.ToStringPercent(),
				" potentialRoads=",
				(this.potentialRoads != null) ? this.potentialRoads.Count : 0,
				" (allowed=",
				this.biome.allowRoads,
				") potentialRivers=",
				(this.potentialRivers != null) ? this.potentialRivers.Count : 0,
				" (allowed=",
				this.biome.allowRivers,
				"))"
			});
		}

		// Token: 0x04001063 RID: 4195
		public const int Invalid = -1;

		// Token: 0x04001064 RID: 4196
		public BiomeDef biome;

		// Token: 0x04001065 RID: 4197
		public float elevation = 100f;

		// Token: 0x04001066 RID: 4198
		public Hilliness hilliness = Hilliness.Undefined;

		// Token: 0x04001067 RID: 4199
		public float temperature = 20f;

		// Token: 0x04001068 RID: 4200
		public float rainfall = 0f;

		// Token: 0x04001069 RID: 4201
		public float swampiness;

		// Token: 0x0400106A RID: 4202
		public WorldFeature feature;

		// Token: 0x0400106B RID: 4203
		public List<Tile.RoadLink> potentialRoads;

		// Token: 0x0400106C RID: 4204
		public List<Tile.RiverLink> potentialRivers;

		// Token: 0x020005A9 RID: 1449
		public struct RoadLink
		{
			// Token: 0x0400106D RID: 4205
			public int neighbor;

			// Token: 0x0400106E RID: 4206
			public RoadDef road;
		}

		// Token: 0x020005AA RID: 1450
		public struct RiverLink
		{
			// Token: 0x0400106F RID: 4207
			public int neighbor;

			// Token: 0x04001070 RID: 4208
			public RiverDef river;
		}
	}
}
