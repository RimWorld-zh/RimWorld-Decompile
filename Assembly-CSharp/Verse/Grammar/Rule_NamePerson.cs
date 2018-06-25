using System;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x02000BEB RID: 3051
	public class Rule_NamePerson : Rule
	{
		// Token: 0x04002D88 RID: 11656
		public Gender gender;

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06004295 RID: 17045 RVA: 0x002312D8 File Offset: 0x0022F6D8
		public override float BaseSelectionWeight
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x002312F4 File Offset: 0x0022F6F4
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

		// Token: 0x06004297 RID: 17047 RVA: 0x00231340 File Offset: 0x0022F740
		public override string ToString()
		{
			return this.keyword + "->(personname)";
		}
	}
}
