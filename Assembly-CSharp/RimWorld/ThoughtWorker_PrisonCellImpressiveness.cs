using Verse;

namespace RimWorld
{
	public class ThoughtWorker_PrisonCellImpressiveness : ThoughtWorker_RoomImpressiveness
	{
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
				result = ((!thoughtState.Active || p.GetRoom(RegionType.Set_Passable).Role != RoomRoleDefOf.PrisonCell) ? ThoughtState.Inactive : thoughtState);
			}
			return result;
		}
	}
}
