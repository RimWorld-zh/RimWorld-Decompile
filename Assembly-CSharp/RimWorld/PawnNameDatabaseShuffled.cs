using System;
using System.Collections;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000484 RID: 1156
	[StaticConstructorOnStartup]
	public static class PawnNameDatabaseShuffled
	{
		// Token: 0x0600146D RID: 5229 RVA: 0x000B302C File Offset: 0x000B142C
		static PawnNameDatabaseShuffled()
		{
			IEnumerator enumerator = Enum.GetValues(typeof(PawnNameCategory)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					PawnNameCategory pawnNameCategory = (PawnNameCategory)obj;
					if (pawnNameCategory != PawnNameCategory.NoName)
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
			foreach (NameBank nameBank2 in PawnNameDatabaseShuffled.banks.Values)
			{
				nameBank2.ErrorCheck();
			}
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x000B3168 File Offset: 0x000B1568
		public static NameBank BankOf(PawnNameCategory category)
		{
			return PawnNameDatabaseShuffled.banks[category];
		}

		// Token: 0x04000C41 RID: 3137
		private static Dictionary<PawnNameCategory, NameBank> banks = new Dictionary<PawnNameCategory, NameBank>();
	}
}
