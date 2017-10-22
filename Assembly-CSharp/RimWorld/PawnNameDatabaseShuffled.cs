using System;
using System.Collections;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class PawnNameDatabaseShuffled
	{
		private static Dictionary<PawnNameCategory, NameBank> banks;

		static PawnNameDatabaseShuffled()
		{
			PawnNameDatabaseShuffled.banks = new Dictionary<PawnNameCategory, NameBank>();
			IEnumerator enumerator = Enum.GetValues(typeof(PawnNameCategory)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					PawnNameCategory pawnNameCategory = (PawnNameCategory)enumerator.Current;
					if (pawnNameCategory != 0)
					{
						PawnNameDatabaseShuffled.banks.Add(pawnNameCategory, new NameBank(pawnNameCategory));
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard);
			nameBank.AddNamesFromFile(PawnNameSlot.First, Gender.Male, "First_Male");
			nameBank.AddNamesFromFile(PawnNameSlot.First, Gender.Female, "First_Female");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.Male, "Nick_Male");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.Female, "Nick_Female");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.None, "Nick_Unisex");
			nameBank.AddNamesFromFile(PawnNameSlot.Last, Gender.None, "Last");
			foreach (NameBank value in PawnNameDatabaseShuffled.banks.Values)
			{
				value.ErrorCheck();
			}
		}

		public static NameBank BankOf(PawnNameCategory category)
		{
			return PawnNameDatabaseShuffled.banks[category];
		}
	}
}
