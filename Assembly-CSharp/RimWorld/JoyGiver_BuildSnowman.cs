using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000FC RID: 252
	public class JoyGiver_BuildSnowman : JoyGiver
	{
		// Token: 0x040002D4 RID: 724
		private const float MinSnowmanDepth = 0.5f;

		// Token: 0x040002D5 RID: 725
		private const float MinDistBetweenSnowmen = 12f;

		// Token: 0x0600054D RID: 1357 RVA: 0x00039A78 File Offset: 0x00037E78
		public override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.story.WorkTypeIsDisabled(WorkTypeDefOf.Construction))
			{
				result = null;
			}
			else if (!JoyUtility.EnjoyableOutsideNow(pawn, null))
			{
				result = null;
			}
			else if (pawn.Map.snowGrid.TotalDepth < 200f)
			{
				result = null;
			}
			else
			{
				IntVec3 c = JoyGiver_BuildSnowman.TryFindSnowmanBuildCell(pawn);
				if (!c.IsValid)
				{
					result = null;
				}
				else
				{
					result = new Job(this.def.jobDef, c);
				}
			}
			return result;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00039B10 File Offset: 0x00037F10
		private static IntVec3 TryFindSnowmanBuildCell(Pawn pawn)
		{
			Region rootReg;
			IntVec3 result2;
			if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), (Region r) => r.Room.PsychologicallyOutdoors, 100, out rootReg, RegionType.Set_Passable))
			{
				result2 = IntVec3.Invalid;
			}
			else
			{
				IntVec3 result = IntVec3.Invalid;
				RegionTraverser.BreadthFirstTraverse(rootReg, (Region from, Region r) => r.Room == rootReg.Room, delegate(Region r)
				{
					for (int i = 0; i < 5; i++)
					{
						IntVec3 randomCell = r.RandomCell;
						if (JoyGiver_BuildSnowman.IsGoodSnowmanCell(randomCell, pawn))
						{
							result = randomCell;
							return true;
						}
					}
					return false;
				}, 30, RegionType.Set_Passable);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00039BC0 File Offset: 0x00037FC0
		private static bool IsGoodSnowmanCell(IntVec3 c, Pawn pawn)
		{
			bool result;
			if (pawn.Map.snowGrid.GetDepth(c) < 0.5f)
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
					{
						return false;
					}
					if (!c2.Standable(pawn.Map))
					{
						return false;
					}
					if (pawn.Map.reservationManager.IsReservedAndRespected(c2, pawn))
					{
						return false;
					}
				}
				List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Snowman);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Position.InHorDistOf(c, 12f))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}
	}
}
