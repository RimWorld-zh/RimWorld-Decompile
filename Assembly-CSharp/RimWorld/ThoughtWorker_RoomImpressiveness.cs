using System;
using Verse;

namespace RimWorld
{
	public abstract class ThoughtWorker_RoomImpressiveness : ThoughtWorker
	{
		protected ThoughtWorker_RoomImpressiveness()
		{
		}

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
					if (this.def.stages[scoreStageIndex] == null)
					{
						result = ThoughtState.Inactive;
					}
					else
					{
						result = ThoughtState.ActiveAtStage(scoreStageIndex);
					}
				}
			}
			return result;
		}
	}
}
