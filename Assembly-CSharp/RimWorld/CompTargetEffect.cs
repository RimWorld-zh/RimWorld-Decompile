using System;
using Verse;

namespace RimWorld
{
	public abstract class CompTargetEffect : ThingComp
	{
		protected CompTargetEffect()
		{
		}

		public abstract void DoEffectOn(Pawn user, Thing target);
	}
}
