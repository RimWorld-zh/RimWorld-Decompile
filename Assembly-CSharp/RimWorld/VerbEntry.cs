using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000514 RID: 1300
	public struct VerbEntry
	{
		// Token: 0x04000DE5 RID: 3557
		public Verb verb;

		// Token: 0x04000DE6 RID: 3558
		private float cachedSelectionWeight;

		// Token: 0x06001787 RID: 6023 RVA: 0x000CE631 File Offset: 0x000CCA31
		public VerbEntry(Verb verb, Pawn pawn, Thing equipment = null)
		{
			this.verb = verb;
			this.cachedSelectionWeight = verb.verbProps.AdjustedMeleeSelectionWeight(verb, pawn, equipment);
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06001788 RID: 6024 RVA: 0x000CE650 File Offset: 0x000CCA50
		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x000CE670 File Offset: 0x000CCA70
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

		// Token: 0x0600178A RID: 6026 RVA: 0x000CE6A8 File Offset: 0x000CCAA8
		public override string ToString()
		{
			return this.verb.ToString() + " - " + this.cachedSelectionWeight;
		}
	}
}
