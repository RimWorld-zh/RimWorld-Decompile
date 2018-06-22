using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000222 RID: 546
	public class ThoughtWorker_Greedy : ThoughtWorker
	{
		// Token: 0x06000A10 RID: 2576 RVA: 0x000594A4 File Offset: 0x000578A4
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
