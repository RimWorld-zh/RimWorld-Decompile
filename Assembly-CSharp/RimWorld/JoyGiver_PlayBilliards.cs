using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_PlayBilliards : JoyGiver_InteractBuilding
	{
		public JoyGiver_PlayBilliards()
		{
		}

		protected override bool CanDoDuringParty
		{
			get
			{
				return true;
			}
		}

		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			Job result;
			if (!JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(t))
			{
				result = null;
			}
			else
			{
				result = new Job(this.def.jobDef, t);
			}
			return result;
		}

		public static bool ThingHasStandableSpaceOnAllSides(Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			CellRect.CellRectIterator iterator = cellRect.ExpandedBy(1).GetIterator();
			while (!iterator.Done())
			{
				IntVec3 c = iterator.Current;
				if (!cellRect.Contains(c))
				{
					if (!c.Standable(t.Map))
					{
						return false;
					}
				}
				iterator.MoveNext();
			}
			return true;
		}
	}
}
