using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F9 RID: 1017
	public class GenStep_Plants : GenStep
	{
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x000977E8 File Offset: 0x00095BE8
		public override int SeedPart
		{
			get
			{
				return 578415222;
			}
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00097804 File Offset: 0x00095C04
		public override void Generate(Map map)
		{
			map.regionAndRoomUpdater.Enabled = false;
			float currentPlantDensity = map.wildPlantSpawner.CurrentPlantDensity;
			float currentWholeMapNumDesiredPlants = map.wildPlantSpawner.CurrentWholeMapNumDesiredPlants;
			foreach (IntVec3 c in map.cellsInRandomOrder.GetAll())
			{
				if (!Rand.Chance(0.001f))
				{
					map.wildPlantSpawner.CheckSpawnWildPlantAt(c, currentPlantDensity, currentWholeMapNumDesiredPlants, true);
				}
			}
			map.regionAndRoomUpdater.Enabled = true;
		}

		// Token: 0x04000AA0 RID: 2720
		private const float ChanceToSkip = 0.001f;
	}
}
