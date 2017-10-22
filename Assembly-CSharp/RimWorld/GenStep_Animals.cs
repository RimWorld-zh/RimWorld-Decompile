using Verse;

namespace RimWorld
{
	public class GenStep_Animals : GenStep
	{
		public override void Generate(Map map)
		{
			int num = 0;
			while (true)
			{
				if (!map.wildSpawner.AnimalEcosystemFull)
				{
					num++;
					if (num < 10000)
					{
						IntVec3 loc = RCellFinder.RandomAnimalSpawnCell_MapGen(map);
						if (!map.wildSpawner.SpawnRandomWildAnimalAt(loc))
							return;
						continue;
					}
					break;
				}
				return;
			}
			Log.Error("Too many iterations.");
		}
	}
}
