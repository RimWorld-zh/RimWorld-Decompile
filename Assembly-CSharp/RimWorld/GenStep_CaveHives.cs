using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class GenStep_CaveHives : GenStep
	{
		private List<IntVec3> rockCells = new List<IntVec3>();

		private List<IntVec3> possibleSpawnCells = new List<IntVec3>();

		private List<Hive> spawnedHives = new List<Hive>();

		private const int MinDistToOpenSpace = 10;

		private const float CaveCellsPerHive = 1000f;

		public override void Generate(Map map)
		{
			if (Find.Storyteller.difficulty.allowCaveHives)
			{
				CompProperties_TemperatureDamaged compProperties = ThingDefOf.Hive.GetCompProperties<CompProperties_TemperatureDamaged>();
				if (compProperties.safeTemperatureRange.Includes(map.mapTemperature.OutdoorTemp))
				{
					MapGenFloatGrid caves = MapGenerator.Caves;
					MapGenFloatGrid elevation = MapGenerator.Elevation;
					float num = 0.7f;
					int num2 = 0;
					this.rockCells.Clear();
					foreach (IntVec3 allCell in map.AllCells)
					{
						if (elevation[allCell] > num)
						{
							this.rockCells.Add(allCell);
						}
						if (caves[allCell] > 0.0)
						{
							num2++;
						}
					}
					int num3 = GenMath.RoundRandom((float)((float)num2 / 1000.0));
					GenMorphology.Erode(this.rockCells, 10, map, null);
					this.possibleSpawnCells.Clear();
					for (int i = 0; i < this.rockCells.Count; i++)
					{
						if (caves[this.rockCells[i]] > 0.0)
						{
							this.possibleSpawnCells.Add(this.rockCells[i]);
						}
					}
					this.spawnedHives.Clear();
					for (int j = 0; j < num3; j++)
					{
						this.TrySpawnHive(map);
					}
					this.spawnedHives.Clear();
				}
			}
		}

		private void TrySpawnHive(Map map)
		{
			IntVec3 intVec = default(IntVec3);
			if (this.TryFindHiveSpawnCell(map, out intVec))
			{
				this.possibleSpawnCells.Remove(intVec);
				Hive hive = (Hive)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Hive, null), intVec, map);
				hive.SetFaction(Faction.OfInsects, null);
				(from x in hive.GetComps<CompSpawner>()
				where x.PropsSpawner.thingToSpawn == ThingDefOf.GlowPod
				select x).First().TryDoSpawn();
				hive.SpawnPawnsUntilPoints(Rand.Range(200f, 500f));
				hive.canSpawnPawns = false;
				hive.GetComp<CompSpawnerHives>().canSpawnHives = false;
				this.spawnedHives.Add(hive);
			}
		}

		private bool TryFindHiveSpawnCell(Map map, out IntVec3 spawnCell)
		{
			float num = -1f;
			IntVec3 intVec = IntVec3.Invalid;
			int num2 = 0;
			IntVec3 intVec2 = default(IntVec3);
			while (num2 < 3 && (from x in this.possibleSpawnCells
			where x.Standable(map) && x.GetFirstItem(map) == null && x.GetFirstBuilding(map) == null && x.GetFirstPawn(map) == null
			select x).TryRandomElement<IntVec3>(out intVec2))
			{
				float num3 = -1f;
				for (int i = 0; i < this.spawnedHives.Count; i++)
				{
					float num4 = (float)intVec2.DistanceToSquared(this.spawnedHives[i].Position);
					if (num3 < 0.0 || num4 < num3)
					{
						num3 = num4;
					}
				}
				if (!intVec.IsValid || num3 > num)
				{
					intVec = intVec2;
					num = num3;
				}
				num2++;
			}
			spawnCell = intVec;
			return spawnCell.IsValid;
		}
	}
}
