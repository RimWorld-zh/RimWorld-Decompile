using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000514 RID: 1300
	public struct VerbEntry
	{
		// Token: 0x04000DE9 RID: 3561
		public Verb verb;

		// Token: 0x04000DEA RID: 3562
		private float cachedSelectionWeight;

		// Token: 0x06001786 RID: 6022 RVA: 0x000CE899 File Offset: 0x000CCC99
		public VerbEntry(Verb verb, Pawn pawn, Thing equipment = null)
		{
			this.verb = verb;
			this.cachedSelectionWeight = verb.verbProps.AdjustedMeleeSelectionWeight(verb, pawn, equipment);
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06001787 RID: 6023 RVA: 0x000CE8B8 File Offset: 0x000CCCB8
		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x000CE8D8 File Offset: 0x000CCCD8
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

		// Token: 0x06001789 RID: 6025 RVA: 0x000CE910 File Offset: 0x000CCD10
		public override string ToString()
		{
			return this.verb.ToString() + " - " + this.cachedSelectionWeight;
		}
	}
}
