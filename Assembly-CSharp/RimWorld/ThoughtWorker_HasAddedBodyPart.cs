using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_HasAddedBodyPart : ThoughtWorker
	{
		private const int NumPartsAllowedWithoutThought = 2;

		public ThoughtWorker_HasAddedBodyPart()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			int num = p.health.hediffSet.CountAddedParts();
			ThoughtState result;
			if (num > 0)
			{
				result = ThoughtState.ActiveAtStage(num - 2);
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
