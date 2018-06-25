using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_PrisonCellImpressiveness : ThoughtWorker_RoomImpressiveness
	{
		public ThoughtWorker_PrisonCellImpressiveness()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (!p.IsPrisoner)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				ThoughtState thoughtState = base.CurrentStateInternal(p);
				if (thoughtState.Active && p.GetRoom(RegionType.Set_Passable).Role == RoomRoleDefOf.PrisonCell)
				{
					result = thoughtState;
				}
				else
				{
					result = ThoughtState.Inactive;
				}
			}
			return result;
		}
	}
}
