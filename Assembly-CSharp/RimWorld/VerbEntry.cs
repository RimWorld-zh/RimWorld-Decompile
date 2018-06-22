using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000512 RID: 1298
	public struct VerbEntry
	{
		// Token: 0x06001783 RID: 6019 RVA: 0x000CE4E1 File Offset: 0x000CC8E1
		public VerbEntry(Verb verb, Pawn pawn, Thing equipment = null)
		{
			this.verb = verb;
			this.cachedSelectionWeight = verb.verbProps.AdjustedMeleeSelectionWeight(verb, pawn, equipment);
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06001784 RID: 6020 RVA: 0x000CE500 File Offset: 0x000CC900
		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x000CE520 File Offset: 0x000CC920
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

		// Token: 0x06001786 RID: 6022 RVA: 0x000CE558 File Offset: 0x000CC958
		public override string ToString()
		{
			return this.verb.ToString() + " - " + this.cachedSelectionWeight;
		}

		// Token: 0x04000DE5 RID: 3557
		public Verb verb;

		// Token: 0x04000DE6 RID: 3558
		private float cachedSelectionWeight;
	}
}
