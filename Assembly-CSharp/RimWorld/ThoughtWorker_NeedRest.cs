using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_NeedRest : ThoughtWorker
	{
		public ThoughtWorker_NeedRest()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.needs.rest == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				switch (p.needs.rest.CurCategory)
				{
				case RestCategory.Rested:
					result = ThoughtState.Inactive;
					break;
				case RestCategory.Tired:
					result = ThoughtState.ActiveAtStage(0);
					break;
				case RestCategory.VeryTired:
					result = ThoughtState.ActiveAtStage(1);
					break;
				case RestCategory.Exhausted:
					result = ThoughtState.ActiveAtStage(2);
					break;
				default:
					throw new NotImplementedException();
				}
			}
			return result;
		}
	}
}
