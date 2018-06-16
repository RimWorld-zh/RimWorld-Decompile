using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000516 RID: 1302
	public struct VerbEntry
	{
		// Token: 0x0600178B RID: 6027 RVA: 0x000CE495 File Offset: 0x000CC895
		public VerbEntry(Verb verb, Pawn pawn, Thing equipment = null)
		{
			this.verb = verb;
			this.cachedSelectionWeight = verb.verbProps.AdjustedMeleeSelectionWeight(verb, pawn, equipment);
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x000CE4B4 File Offset: 0x000CC8B4
		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x000CE4D4 File Offset: 0x000CC8D4
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

		// Token: 0x0600178E RID: 6030 RVA: 0x000CE50C File Offset: 0x000CC90C
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
