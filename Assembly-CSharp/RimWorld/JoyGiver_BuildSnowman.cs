using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_BuildSnowman : JoyGiver
	{
		private const float MinSnowmanDepth = 0.5f;

		private const float MinDistBetweenSnowmen = 12f;

		[CompilerGenerated]
		private static Predicate<Region> <>f__am$cache0;

		public JoyGiver_BuildSnowman()
		{
		}

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

		[CompilerGenerated]
		private static bool <TryFindSnowmanBuildCell>m__0(Region r)
		{
			return r.Room.PsychologicallyOutdoors;
		}

		[CompilerGenerated]
		private sealed class <TryFindSnowmanBuildCell>c__AnonStorey0
		{
			internal Region rootReg;

			internal Pawn pawn;

			internal IntVec3 result;

			public <TryFindSnowmanBuildCell>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region from, Region r)
			{
				return r.Room == this.rootReg.Room;
			}

			internal bool <>m__1(Region r)
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 randomCell = r.RandomCell;
					if (JoyGiver_BuildSnowman.IsGoodSnowmanCell(randomCell, this.pawn))
					{
						this.result = randomCell;
						return true;
					}
				}
				return false;
			}
		}
	}
}
