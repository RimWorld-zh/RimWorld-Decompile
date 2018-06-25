using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Imprisoned : ThoughtWorker
	{
		public ThoughtWorker_Imprisoned()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.IsPrisoner;
		}
	}
}
