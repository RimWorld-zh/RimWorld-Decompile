using System;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x02000BEC RID: 3052
	public class Rule_NamePerson : Rule
	{
		// Token: 0x04002D8F RID: 11663
		public Gender gender;

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06004295 RID: 17045 RVA: 0x002315B8 File Offset: 0x0022F9B8
		public override float BaseSelectionWeight
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x002315D4 File Offset: 0x0022F9D4
		public override string Generate()
		{
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard);
			Gender gender = this.gender;
			if (gender == Gender.None)
			{
				gender = ((Rand.Value >= 0.5f) ? Gender.Female : Gender.Male);
			}
			return nameBank.GetName(PawnNameSlot.First, gender, false);
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x00231620 File Offset: 0x0022FA20
		public override string ToString()
		{
			return this.keyword + "->(personname)";
		}
	}
}
