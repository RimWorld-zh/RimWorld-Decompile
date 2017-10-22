using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Ascetic : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (!p.IsColonist)
			{
				result = false;
			}
			else
			{
				Room ownedRoom = p.ownership.OwnedRoom;
				if (ownedRoom == null)
				{
					result = false;
				}
				else
				{
					int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness));
					result = ((base.def.stages[scoreStageIndex] == null) ? ThoughtState.Inactive : ThoughtState.ActiveAtStage(scoreStageIndex));
				}
			}
			return result;
		}
	}
}
