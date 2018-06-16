using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F6 RID: 1014
	public class GenStep_Animals : GenStep
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x00097278 File Offset: 0x00095678
		public override int SeedPart
		{
			get
			{
				return 1298760307;
			}
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00097294 File Offset: 0x00095694
		public override void Generate(Map map)
		{
			int num = 0;
			while (!map.wildAnimalSpawner.AnimalEcosystemFull)
			{
				num++;
				if (num >= 10000)
				{
					Log.Error("Too many iterations.", false);
					break;
				}
				IntVec3 loc = RCellFinder.RandomAnimalSpawnCell_MapGen(map);
				if (!map.wildAnimalSpawner.SpawnRandomWildAnimalAt(loc))
				{
					break;
				}
			}
		}
	}
}
