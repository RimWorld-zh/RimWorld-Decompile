using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Greedy : ThoughtWorker
	{
		public ThoughtWorker_Greedy()
		{
		}

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
