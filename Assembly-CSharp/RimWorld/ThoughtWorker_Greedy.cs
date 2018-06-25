using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000224 RID: 548
	public class ThoughtWorker_Greedy : ThoughtWorker
	{
		// Token: 0x06000A13 RID: 2579 RVA: 0x00059620 File Offset: 0x00057A20
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
