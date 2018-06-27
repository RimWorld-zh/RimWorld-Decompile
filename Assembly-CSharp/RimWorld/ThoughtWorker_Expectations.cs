using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Expectations : ThoughtWorker
	{
		public ThoughtWorker_Expectations()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p);
			ThoughtState result;
			if (expectationDef == null)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				result = ThoughtState.ActiveAtStage(expectationDef.thoughtStage);
			}
			return result;
		}
	}
}
