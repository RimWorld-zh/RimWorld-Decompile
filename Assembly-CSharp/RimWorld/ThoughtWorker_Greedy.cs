using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000222 RID: 546
	public class ThoughtWorker_Greedy : ThoughtWorker
	{
		// Token: 0x06000A12 RID: 2578 RVA: 0x00059460 File Offset: 0x00057860
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
					result = ThoughtState.ActiveAtStage(0);
				}
				else
				{
					int num = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness)) + 1;
					if (this.def.stages[num] != null)
					{
						result = ThoughtState.ActiveAtStage(num);
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
