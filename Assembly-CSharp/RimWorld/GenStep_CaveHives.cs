using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020003E9 RID: 1001
	public class GenStep_CaveHives : GenStep
	{
		// Token: 0x04000A61 RID: 2657
		private List<IntVec3> rockCells = new List<IntVec3>();

		// Token: 0x04000A62 RID: 2658
		private List<IntVec3> possibleSpawnCells = new List<IntVec3>();

		// Token: 0x04000A63 RID: 2659
		private List<Hive> spawnedHives = new List<Hive>();

		// Token: 0x04000A64 RID: 2660
		private const int MinDistToOpenSpace = 10;

		// Token: 0x04000A65 RID: 2661
		private const int MinDistFromFactionBase = 50;

		// Token: 0x04000A66 RID: 2662
		private const float CaveCellsPerHive = 1000f;

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x0600111F RID: 4383 RVA: 0x00092AB4 File Offset: 0x00090EB4
		public override int SeedPart
		{
			get
			{
				return 349641510;
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00092AD0 File Offset: 0x00090ED0
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
					foreach (IntVec3 intVec in map.AllCells)
					{
						if (elevation[intVec] > num)
						{
							this.rockCells.Add(intVec);
						}
						if (caves[intVec] > 0f)
						{
							num2++;
						}
					}
					List<IntVec3> list = (from c in map.AllCells
					where map.thingGrid.ThingsAt(c).Any((Thing thing) => thing.Faction != null)
					select c).ToList<IntVec3>();
					GenMorphology.Dilate(list, 50, map, null);
					HashSet<IntVec3> hashSet = new HashSet<IntVec3>(list);
					int num3 = GenMath.RoundRandom((float)num2 / 1000f);
					GenMorphology.Erode(this.rockCells, 10, map, null);
					this.possibleSpawnCells.Clear();
					for (int i = 0; i < this.rockCells.Count; i++)
					{
						if (caves[this.rockCells[i]] > 0f && !hashSet.Contains(this.rockCells[i]))
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

		// Token: 0x06001121 RID: 4385 RVA: 0x00092CF4 File Offset: 0x000910F4
		private void TrySpawnHive(Map map)
		{
			IntVec3 intVec;
			if (this.TryFindHiveSpawnCell(map, out intVec))
			{
				this.possibleSpawnCells.Remove(intVec);
				Hive hive = (Hive)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Hive, null), intVec, map, WipeMode.Vanish);
				hive.SetFaction(Faction.OfInsects, null);
				hive.caveColony = true;
				(from x in hive.GetComps<CompSpawner>()
				where x.PropsSpawner.thingToSpawn == ThingDefOf.GlowPod
				select x).First<CompSpawner>().TryDoSpawn();
				hive.SpawnPawnsUntilPoints(Rand.Range(200f, 500f));
				hive.canSpawnPawns = false;
				hive.GetComp<CompSpawnerHives>().canSpawnHives = false;
				this.spawnedHives.Add(hive);
			}
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x00092DB8 File Offset: 0x000911B8
		private bool TryFindHiveSpawnCell(Map map, out IntVec3 spawnCell)
		{
			float num = -1f;
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < 3; i++)
			{
				IntVec3 intVec2;
				if (!(from x in this.possibleSpawnCells
				where x.Standable(map) && x.GetFirstItem(map) == null && x.GetFirstBuilding(map) == null && x.GetFirstPawn(map) == null
				select x).TryRandomElement(out intVec2))
				{
					break;
				}
				float num2 = -1f;
				for (int j = 0; j < this.spawnedHives.Count; j++)
				{
					float num3 = (float)intVec2.DistanceToSquared(this.spawnedHives[j].Position);
					if (num2 < 0f || num3 < num2)
					{
						num2 = num3;
					}
				}
				if (!intVec.IsValid || num2 > num)
				{
					intVec = intVec2;
					num = num2;
				}
			}
			spawnCell = intVec;
			return spawnCell.IsValid;
		}
	}
}
