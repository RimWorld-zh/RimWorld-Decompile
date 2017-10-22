using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class IncidentWorker_HerdMigration : IncidentWorker
	{
		private static readonly IntRange AnimalsCount = new IntRange(30, 50);

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			Map map = (Map)target;
			PawnKindDef pawnKindDef = default(PawnKindDef);
			IntVec3 intVec = default(IntVec3);
			IntVec3 intVec2 = default(IntVec3);
			return this.TryFindAnimalKind(map.Tile, out pawnKindDef) && this.TryFindStartAndEndCells(map, out intVec, out intVec2);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef pawnKindDef = default(PawnKindDef);
			bool result;
			IntVec3 intVec = default(IntVec3);
			IntVec3 near = default(IntVec3);
			if (!this.TryFindAnimalKind(map.Tile, out pawnKindDef))
			{
				result = false;
			}
			else if (!this.TryFindStartAndEndCells(map, out intVec, out near))
			{
				result = false;
			}
			else
			{
				Rot4 rot = Rot4.FromAngleFlat((map.Center - intVec).AngleFlat);
				List<Pawn> list = this.GenerateAnimals(pawnKindDef, map.Tile);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn newThing = list[i];
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
					GenSpawn.Spawn(newThing, loc, map, rot, false);
				}
				LordMaker.MakeNewLord(null, new LordJob_ExitMapNear(near, LocomotionUrgency.Walk, 12f, false, false), map, list);
				string text = string.Format(base.def.letterText, pawnKindDef.GetLabelPlural(-1)).CapitalizeFirst();
				string label = string.Format(base.def.letterLabel, pawnKindDef.GetLabelPlural(-1).CapitalizeFirst());
				Find.LetterStack.ReceiveLetter(label, text, base.def.letterDef, (Thing)list[0], (string)null);
				result = true;
			}
			return result;
		}

		private bool TryFindAnimalKind(int tile, out PawnKindDef animalKind)
		{
			return (from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Animal && k.RaceProps.herdAnimal && Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, k.race)
			select k).TryRandomElementByWeight<PawnKindDef>((Func<PawnKindDef, float>)((PawnKindDef x) => x.RaceProps.wildness), out animalKind);
		}

		private bool TryFindStartAndEndCells(Map map, out IntVec3 start, out IntVec3 end)
		{
			bool result;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out start, map, CellFinder.EdgeRoadChance_Animal, (Predicate<IntVec3>)null))
			{
				end = IntVec3.Invalid;
				result = false;
			}
			else
			{
				end = IntVec3.Invalid;
				int num = 0;
				while (num < 8)
				{
					IntVec3 startLocal = start;
					IntVec3 intVec = default(IntVec3);
					if (CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 x) => map.reachability.CanReach(startLocal, x, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly)), map, CellFinder.EdgeRoadChance_Ignore, out intVec))
					{
						if (!end.IsValid || intVec.DistanceToSquared(start) > end.DistanceToSquared(start))
						{
							end = intVec;
						}
						num++;
						continue;
					}
					break;
				}
				result = end.IsValid;
			}
			return result;
		}

		private List<Pawn> GenerateAnimals(PawnKindDef animalKind, int tile)
		{
			int randomInRange = IncidentWorker_HerdMigration.AnimalsCount.RandomInRange;
			List<Pawn> list = new List<Pawn>();
			for (int num = 0; num < randomInRange; num++)
			{
				PawnGenerationRequest request = new PawnGenerationRequest(animalKind, null, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, default(float?), default(float?), default(float?), default(Gender?), default(float?), (string)null);
				Pawn item = PawnGenerator.GeneratePawn(request);
				list.Add(item);
			}
			return list;
		}
	}
}
