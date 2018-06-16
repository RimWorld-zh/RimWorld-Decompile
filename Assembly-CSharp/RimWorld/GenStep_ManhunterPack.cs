using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200040B RID: 1035
	public class GenStep_ManhunterPack : GenStep
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x0009A99C File Offset: 0x00098D9C
		public override int SeedPart
		{
			get
			{
				return 457293335;
			}
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0009A9B8 File Offset: 0x00098DB8
		public override void Generate(Map map)
		{
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false);
			IntVec3 root;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && map.reachability.CanReachMapEdge(x, traverseParams) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= this.MinRoomCells, map, out root))
			{
				float randomInRange = this.pointsRange.RandomInRange;
				PawnKindDef animalKind;
				if (ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(randomInRange, map.Tile, out animalKind) || ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(randomInRange, -1, out animalKind))
				{
					List<Pawn> list = ManhunterPackIncidentUtility.GenerateAnimals(animalKind, map.Tile, randomInRange);
					for (int i = 0; i < list.Count; i++)
					{
						IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
						GenSpawn.Spawn(list[i], loc, map, Rot4.Random, WipeMode.Vanish, false);
						list[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
					}
				}
			}
		}

		// Token: 0x04000AD4 RID: 2772
		public FloatRange pointsRange = new FloatRange(300f, 500f);

		// Token: 0x04000AD5 RID: 2773
		private int MinRoomCells = 225;
	}
}
