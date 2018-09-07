using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_AlwaysActive : ThoughtWorker
	{
		public ThoughtWorker_AlwaysActive()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return true;
		}

		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			return true;
		}
	}
}
