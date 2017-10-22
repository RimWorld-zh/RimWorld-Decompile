using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_BuildSnowman : JoyGiver
	{
		private const float MinSnowmanDepth = 0.5f;

		public override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!JoyUtility.EnjoyableOutsideNow(pawn, null))
			{
				result = null;
			}
			else if (pawn.Map.snowGrid.TotalDepth < 200.0)
			{
				result = null;
			}
			else
			{
				IntVec3 c = JoyGiver_BuildSnowman.TryFindSnowmanBuildCell(pawn);
				result = (c.IsValid ? new Job(base.def.jobDef, c) : null);
			}
			return result;
		}

		private static IntVec3 TryFindSnowmanBuildCell(Pawn pawn)
		{
			Region rootReg;
			IntVec3 result2;
			if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), (Predicate<Region>)((Region r) => r.Room.PsychologicallyOutdoors), 100, out rootReg, RegionType.Set_Passable))
			{
				result2 = IntVec3.Invalid;
			}
			else
			{
				IntVec3 result = IntVec3.Invalid;
				RegionTraverser.BreadthFirstTraverse(rootReg, (RegionEntryPredicate)((Region from, Region r) => r.Room == rootReg.Room), (RegionProcessor)delegate(Region r)
				{
					int num = 0;
					bool result3;
					while (true)
					{
						if (num < 5)
						{
							IntVec3 randomCell = r.RandomCell;
							if (JoyGiver_BuildSnowman.IsGoodSnowmanCell(randomCell, pawn))
							{
								result = randomCell;
								result3 = true;
								break;
							}
							num++;
							continue;
						}
						result3 = false;
						break;
					}
					return result3;
				}, 30, RegionType.Set_Passable);
				result2 = result;
			}
			return result2;
		}

		private static bool IsGoodSnowmanCell(IntVec3 c, Pawn pawn)
		{
			bool result;
			if (pawn.Map.snowGrid.GetDepth(c) < 0.5)
			{
				result = false;
			}
			else if (c.IsForbidden(pawn))
			{
				result = false;
			}
			else if (c.GetEdifice(pawn.Map) != null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
					if (!c2.InBounds(pawn.Map))
						goto IL_007e;
					if (!c2.Standable(pawn.Map))
						goto IL_0096;
					if (pawn.Map.reservationManager.IsReservedAndRespected(c2, pawn))
						goto IL_00b9;
				}
				result = true;
			}
			goto IL_00d4;
			IL_00b9:
			result = false;
			goto IL_00d4;
			IL_007e:
			result = false;
			goto IL_00d4;
			IL_0096:
			result = false;
			goto IL_00d4;
			IL_00d4:
			return result;
		}
	}
}
