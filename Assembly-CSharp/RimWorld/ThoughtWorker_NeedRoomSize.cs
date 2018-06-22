using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000231 RID: 561
	public class ThoughtWorker_NeedRoomSize : ThoughtWorker
	{
		// Token: 0x06000A30 RID: 2608 RVA: 0x00059EE8 File Offset: 0x000582E8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.needs.roomsize == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				Room room = p.GetRoom(RegionType.Set_Passable);
				if (room == null || room.PsychologicallyOutdoors)
				{
					result = ThoughtState.Inactive;
				}
				else
				{
					switch (p.needs.roomsize.CurCategory)
					{
					case RoomSizeCategory.VeryCramped:
						result = ThoughtState.ActiveAtStage(0);
						break;
					case RoomSizeCategory.Cramped:
						result = ThoughtState.ActiveAtStage(1);
						break;
					case RoomSizeCategory.Normal:
						result = ThoughtState.Inactive;
						break;
					case RoomSizeCategory.Spacious:
						result = ThoughtState.ActiveAtStage(2);
						break;
					default:
						throw new InvalidOperationException("Unknown RoomSizeCategory");
					}
				}
			}
			return result;
		}
	}
}
