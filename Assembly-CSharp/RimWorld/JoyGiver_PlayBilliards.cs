using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_PlayBilliards : JoyGiver_InteractBuilding
	{
		protected override bool CanDoDuringParty
		{
			get
			{
				return true;
			}
		}

		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			return JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(t) ? new Job(base.def.jobDef, t) : null;
		}

		public static bool ThingHasStandableSpaceOnAllSides(Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			CellRect.CellRectIterator iterator = cellRect.ExpandedBy(1).GetIterator();
			bool result;
			while (true)
			{
				if (!iterator.Done())
				{
					IntVec3 current = iterator.Current;
					if (!cellRect.Contains(current) && !current.Standable(t.Map))
					{
						result = false;
						break;
					}
					iterator.MoveNext();
					continue;
				}
				result = true;
				break;
			}
			return result;
		}
	}
}
