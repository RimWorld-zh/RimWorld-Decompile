using System;
using Verse;

namespace RimWorld
{
	public struct VerbEntry
	{
		public Verb verb;

		private float cachedSelectionWeight;

		public VerbEntry(Verb verb, Pawn pawn, Thing equipment = null)
		{
			this.verb = verb;
			this.cachedSelectionWeight = verb.verbProps.AdjustedMeleeSelectionWeight(verb, pawn, equipment);
		}

		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		public float GetSelectionWeight(Thing target)
		{
			float result;
			if (!this.verb.IsUsableOn(target))
			{
				result = 0f;
			}
			else
			{
				result = this.cachedSelectionWeight;
			}
			return result;
		}

		public override string ToString()
		{
			return this.verb.ToString() + " - " + this.cachedSelectionWeight;
		}
	}
}
