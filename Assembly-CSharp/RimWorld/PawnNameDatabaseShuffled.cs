using System;
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
			foreach (byte value in Enum.GetValues(typeof(PawnNameCategory)))
			{
				if (value != 0)
				{
					PawnNameDatabaseShuffled.banks.Add((PawnNameCategory)value, new NameBank((PawnNameCategory)value));
				}
			}
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard);
			nameBank.AddNamesFromFile(PawnNameSlot.First, Gender.Male, "First_Male");
			nameBank.AddNamesFromFile(PawnNameSlot.First, Gender.Female, "First_Female");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.Male, "Nick_Male");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.Female, "Nick_Female");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.None, "Nick_Unisex");
			nameBank.AddNamesFromFile(PawnNameSlot.Last, Gender.None, "Last");
			Dictionary<PawnNameCategory, NameBank>.ValueCollection.Enumerator enumerator2 = PawnNameDatabaseShuffled.banks.Values.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					NameBank current = enumerator2.Current;
					current.ErrorCheck();
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
		}

		public static NameBank BankOf(PawnNameCategory category)
		{
			return PawnNameDatabaseShuffled.banks[category];
		}
	}
}
