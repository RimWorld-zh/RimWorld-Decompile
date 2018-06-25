using System;
using RimWorld;

namespace Verse.Grammar
{
	public class Rule_NamePerson : Rule
	{
		public Gender gender;

		public Rule_NamePerson()
		{
		}

		public override float BaseSelectionWeight
		{
			get
			{
				return 1f;
			}
		}

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

		public override string ToString()
		{
			return this.keyword + "->(personname)";
		}
	}
}
