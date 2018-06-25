using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000226 RID: 550
	public class ThoughtWorker_Ascetic : ThoughtWorker
	{
		// Token: 0x06000A17 RID: 2583 RVA: 0x00059804 File Offset: 0x00057C04
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
					if (this.def.stages[scoreStageIndex] != null)
					{
						result = ThoughtState.ActiveAtStage(scoreStageIndex);
					}
					else
					{
						result = ThoughtState.Inactive;
					}
				}
			}
			return result;
		}
	}
}
