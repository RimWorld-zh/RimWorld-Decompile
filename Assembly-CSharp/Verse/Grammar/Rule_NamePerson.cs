using System;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x02000BED RID: 3053
	public class Rule_NamePerson : Rule
	{
		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06004290 RID: 17040 RVA: 0x00230948 File Offset: 0x0022ED48
		public override float BaseSelectionWeight
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x00230964 File Offset: 0x0022ED64
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

		// Token: 0x06004292 RID: 17042 RVA: 0x002309B0 File Offset: 0x0022EDB0
		public override string ToString()
		{
			return this.keyword + "->(personname)";
		}

		// Token: 0x04002D82 RID: 11650
		public Gender gender;
	}
}
