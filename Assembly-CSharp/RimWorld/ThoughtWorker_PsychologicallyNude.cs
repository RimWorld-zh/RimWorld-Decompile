using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_PsychologicallyNude : ThoughtWorker
	{
		public ThoughtWorker_PsychologicallyNude()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.apparel.PsychologicallyNude;
		}
	}
}
