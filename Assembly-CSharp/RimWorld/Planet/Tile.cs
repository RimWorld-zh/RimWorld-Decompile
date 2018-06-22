using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A4 RID: 1444
	public class Tile
	{
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001B92 RID: 7058 RVA: 0x000EE1D0 File Offset: 0x000EC5D0
		public bool WaterCovered
		{
			get
			{
				return this.elevation <= 0f;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001B93 RID: 7059 RVA: 0x000EE1F8 File Offset: 0x000EC5F8
		public List<Tile.RoadLink> Roads
		{
			get
			{
				return (!this.biome.allowRoads) ? null : this.potentialRoads;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001B94 RID: 7060 RVA: 0x000EE22C File Offset: 0x000EC62C
		public List<Tile.RiverLink> Rivers
		{
			get
			{
				return (!this.biome.allowRivers) ? null : this.potentialRivers;
			}
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x000EE260 File Offset: 0x000EC660
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

		// Token: 0x04001060 RID: 4192
		public const int Invalid = -1;

		// Token: 0x04001061 RID: 4193
		public BiomeDef biome;

		// Token: 0x04001062 RID: 4194
		public float elevation = 100f;

		// Token: 0x04001063 RID: 4195
		public Hilliness hilliness = Hilliness.Undefined;

		// Token: 0x04001064 RID: 4196
		public float temperature = 20f;

		// Token: 0x04001065 RID: 4197
		public float rainfall = 0f;

		// Token: 0x04001066 RID: 4198
		public float swampiness;

		// Token: 0x04001067 RID: 4199
		public WorldFeature feature;

		// Token: 0x04001068 RID: 4200
		public List<Tile.RoadLink> potentialRoads;

		// Token: 0x04001069 RID: 4201
		public List<Tile.RiverLink> potentialRivers;

		// Token: 0x020005A5 RID: 1445
		public struct RoadLink
		{
			// Token: 0x0400106A RID: 4202
			public int neighbor;

			// Token: 0x0400106B RID: 4203
			public RoadDef road;
		}

		// Token: 0x020005A6 RID: 1446
		public struct RiverLink
		{
			// Token: 0x0400106C RID: 4204
			public int neighbor;

			// Token: 0x0400106D RID: 4205
			public RiverDef river;
		}
	}
}
