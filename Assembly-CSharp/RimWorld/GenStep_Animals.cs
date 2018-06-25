using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020003F8 RID: 1016
	public class GenStep_Animals : GenStep
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x0600117B RID: 4475 RVA: 0x000975AC File Offset: 0x000959AC
		public override int SeedPart
		{
			get
			{
				return 1298760307;
			}
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x000975C8 File Offset: 0x000959C8
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
