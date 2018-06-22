using System;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x02000BE9 RID: 3049
	public class Rule_NamePerson : Rule
	{
		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06004292 RID: 17042 RVA: 0x002311FC File Offset: 0x0022F5FC
		public override float BaseSelectionWeight
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06004293 RID: 17043 RVA: 0x00231218 File Offset: 0x0022F618
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

		// Token: 0x06004294 RID: 17044 RVA: 0x00231264 File Offset: 0x0022F664
		public override string ToString()
		{
			return this.keyword + "->(personname)";
		}

		// Token: 0x04002D88 RID: 11656
		public Gender gender;
	}
}
