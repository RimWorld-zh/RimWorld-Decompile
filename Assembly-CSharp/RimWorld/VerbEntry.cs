using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000516 RID: 1302
	public struct VerbEntry
	{
		// Token: 0x0600178C RID: 6028 RVA: 0x000CE4E9 File Offset: 0x000CC8E9
		public VerbEntry(Verb verb, Pawn pawn, Thing equipment = null)
		{
			this.verb = verb;
			this.cachedSelectionWeight = verb.verbProps.AdjustedMeleeSelectionWeight(verb, pawn, equipment);
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x000CE508 File Offset: 0x000CC908
		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x000CE528 File Offset: 0x000CC928
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

		// Token: 0x0600178F RID: 6031 RVA: 0x000CE560 File Offset: 0x000CC960
		public override string ToString()
		{
			return this.verb.ToString() + " - " + this.cachedSelectionWeight;
		}

		// Token: 0x04000DE8 RID: 3560
		public Verb verb;

		// Token: 0x04000DE9 RID: 3561
		private float cachedSelectionWeight;
	}
}
