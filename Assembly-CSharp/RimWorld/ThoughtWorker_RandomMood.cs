using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_RandomMood : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			switch ((p.GetHashCode() ^ (GenLocalDate.DayOfYear(p) + GenLocalDate.Year(p) * 60) * 391) % 10)
			{
			case 0:
			{
				result = ThoughtState.ActiveAtStage(0);
				break;
			}
			case 1:
			{
				result = ThoughtState.ActiveAtStage(1);
				break;
			}
			case 2:
			{
				result = ThoughtState.ActiveAtStage(1);
				break;
			}
			case 3:
			{
				result = ThoughtState.ActiveAtStage(1);
				break;
			}
			case 4:
			{
				result = ThoughtState.Inactive;
				break;
			}
			case 5:
			{
				result = ThoughtState.Inactive;
				break;
			}
			case 6:
			{
				result = ThoughtState.ActiveAtStage(2);
				break;
			}
			case 7:
			{
				result = ThoughtState.ActiveAtStage(2);
				break;
			}
			case 8:
			{
				result = ThoughtState.ActiveAtStage(2);
				break;
			}
			case 9:
			{
				result = ThoughtState.ActiveAtStage(3);
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
			}
			return result;
		}
	}
}
