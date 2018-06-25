using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003FB RID: 1019
	public class GenStep_Plants : GenStep
	{
		// Token: 0x04000AA4 RID: 2724
		private const float ChanceToSkip = 0.001f;

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06001183 RID: 4483 RVA: 0x00097B2C File Offset: 0x00095F2C
		public override int SeedPart
		{
			get
			{
				return 578415222;
			}
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x00097B48 File Offset: 0x00095F48
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
	}
}
