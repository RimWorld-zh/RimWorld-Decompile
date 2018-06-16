using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020003FA RID: 1018
	public class GenStep_RockChunks : GenStep
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06001183 RID: 4483 RVA: 0x000978C8 File Offset: 0x00095CC8
		public override int SeedPart
		{
			get
			{
				return 1898758716;
			}
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x000978E4 File Offset: 0x00095CE4
		public override void Generate(Map map)
		{
			if (!map.TileInfo.WaterCovered)
			{
				this.freqFactorNoise = new Perlin(0.014999999664723873, 2.0, 0.5, 6, Rand.Range(0, 999999), QualityMode.Medium);
				this.freqFactorNoise = new ScaleBias(1.0, 1.0, this.freqFactorNoise);
				NoiseDebugUI.StoreNoiseRender(this.freqFactorNoise, "rock_chunks_freq_factor");
				MapGenFloatGrid elevation = MapGenerator.Elevation;
				foreach (IntVec3 intVec in map.AllCells)
				{
					float num = 0.006f * this.freqFactorNoise.GetValue(intVec);
					if (elevation[intVec] < 0.55f && Rand.Value < num)
					{
						this.GrowLowRockFormationFrom(intVec, map);
					}
				}
				this.freqFactorNoise = null;
			}
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x000979FC File Offset: 0x00095DFC
		private void GrowLowRockFormationFrom(IntVec3 root, Map map)
		{
			ThingDef filth_RubbleRock = ThingDefOf.Filth_RubbleRock;
			ThingDef mineableThing = Find.World.NaturalRockTypesIn(map.Tile).RandomElement<ThingDef>().building.mineableThing;
			Rot4 random = Rot4.Random;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			IntVec3 intVec = root;
			for (;;)
			{
				Rot4 random2;
				do
				{
					random2 = Rot4.Random;
				}
				while (random2 == random);
				intVec += random2.FacingCell;
				if (!intVec.InBounds(map) || intVec.GetEdifice(map) != null || intVec.GetFirstItem(map) != null)
				{
					break;
				}
				if (elevation[intVec] > 0.55f)
				{
					break;
				}
				if (!map.terrainGrid.TerrainAt(intVec).affordances.Contains(TerrainAffordanceDefOf.Heavy))
				{
					break;
				}
				GenSpawn.Spawn(mineableThing, intVec, map, WipeMode.Vanish);
				IntVec3[] adjacentCellsAndInside = GenAdj.AdjacentCellsAndInside;
				int i = 0;
				while (i < adjacentCellsAndInside.Length)
				{
					IntVec3 b = adjacentCellsAndInside[i];
					if (Rand.Value < 0.5f)
					{
						IntVec3 c = intVec + b;
						if (c.InBounds(map))
						{
							bool flag = false;
							List<Thing> thingList = c.GetThingList(map);
							for (int j = 0; j < thingList.Count; j++)
							{
								Thing thing = thingList[j];
								if (thing.def.category != ThingCategory.Plant && thing.def.category != ThingCategory.Item && thing.def.category != ThingCategory.Pawn)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								FilthMaker.MakeFilth(c, map, filth_RubbleRock, 1);
							}
						}
					}
					IL_1AD:
					i++;
					continue;
					goto IL_1AD;
				}
			}
		}

		// Token: 0x04000AA1 RID: 2721
		private ModuleBase freqFactorNoise = null;

		// Token: 0x04000AA2 RID: 2722
		private const float ThreshLooseRock = 0.55f;

		// Token: 0x04000AA3 RID: 2723
		private const float PlaceProbabilityPerCell = 0.006f;

		// Token: 0x04000AA4 RID: 2724
		private const float RubbleProbability = 0.5f;
	}
}
