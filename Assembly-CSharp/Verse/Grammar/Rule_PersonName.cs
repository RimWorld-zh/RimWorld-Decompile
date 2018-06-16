using System;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x02000BEE RID: 3054
	public class Rule_PersonName : Rule
	{
		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06004292 RID: 17042 RVA: 0x00230968 File Offset: 0x0022ED68
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.selectionWeight;
			}
		}

		// Token: 0x06004293 RID: 17043 RVA: 0x00230984 File Offset: 0x0022ED84
		public override string Generate()
		{
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard);
			Gender gender = this.gender;
			if (gender == Gender.None)
			{
				gender = ((Rand.Value >= 0.5f) ? Gender.Female : Gender.Male);
			}
			return nameBank.GetName(PawnNameSlot.First, gender);
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x002309CC File Offset: 0x0022EDCC
		public override string ToString()
		{
			return this.keyword + "->(personname)";
		}

		// Token: 0x04002D83 RID: 11651
		public int selectionWeight = 1;

		// Token: 0x04002D84 RID: 11652
		public Gender gender;
	}
}
