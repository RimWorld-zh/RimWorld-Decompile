using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_NeedSpace : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.needs.space == null)
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
					switch (p.needs.space.CurCategory)
					{
					case SpaceCategory.VeryCramped:
					{
						result = ThoughtState.ActiveAtStage(0);
						break;
					}
					case SpaceCategory.Cramped:
					{
						result = ThoughtState.ActiveAtStage(1);
						break;
					}
					case SpaceCategory.Normal:
					{
						result = ThoughtState.Inactive;
						break;
					}
					case SpaceCategory.Spacious:
					{
						result = ThoughtState.ActiveAtStage(2);
						break;
					}
					default:
					{
						throw new InvalidOperationException("Unknown SpaceCategory");
					}
					}
				}
			}
			return result;
		}
	}
}
