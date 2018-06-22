using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001F0 RID: 496
	public abstract class ThoughtWorker_RoomImpressiveness : ThoughtWorker
	{
		// Token: 0x060009A9 RID: 2473 RVA: 0x000572F0 File Offset: 0x000556F0
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
