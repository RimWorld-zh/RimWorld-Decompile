using Verse;

namespace RimWorld
{
	public abstract class ThoughtWorker_RoomImpressiveness : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				Room room = p.GetRoom(RegionType.Set_Passable);
				if (room == null)
				{
					result = ThoughtState.Inactive;
				}
				else
				{
					int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
					result = ((base.def.stages[scoreStageIndex] != null) ? ThoughtState.ActiveAtStage(scoreStageIndex) : ThoughtState.Inactive);
				}
			}
			return result;
		}
	}
}
