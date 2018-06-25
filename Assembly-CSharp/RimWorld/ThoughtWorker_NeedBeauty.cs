using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_NeedBeauty : ThoughtWorker
	{
		public ThoughtWorker_NeedBeauty()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (p.needs.beauty == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				switch (p.needs.beauty.CurCategory)
				{
				case BeautyCategory.Hideous:
					result = ThoughtState.ActiveAtStage(0);
					break;
				case BeautyCategory.VeryUgly:
					result = ThoughtState.ActiveAtStage(1);
					break;
				case BeautyCategory.Ugly:
					result = ThoughtState.ActiveAtStage(2);
					break;
				case BeautyCategory.Neutral:
					result = ThoughtState.Inactive;
					break;
				case BeautyCategory.Pretty:
					result = ThoughtState.ActiveAtStage(3);
					break;
				case BeautyCategory.VeryPretty:
					result = ThoughtState.ActiveAtStage(4);
					break;
				case BeautyCategory.Beautiful:
					result = ThoughtState.ActiveAtStage(5);
					break;
				default:
					throw new InvalidOperationException("Unknown BeautyCategory");
				}
			}
			return result;
		}
	}
}
