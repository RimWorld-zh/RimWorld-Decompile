using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class NameBank
	{
		public PawnNameCategory nameType;

		private List<string>[,] names;

		private static readonly int numGenders = Enum.GetValues(typeof(Gender)).Length;

		private static readonly int numSlots = Enum.GetValues(typeof(PawnNameSlot)).Length;

		private IEnumerable<List<string>> AllNameLists
		{
			get
			{
				for (int j = 0; j < NameBank.numGenders; j++)
				{
					for (int i = 0; i < NameBank.numSlots; i++)
					{
						yield return this.names[j, i];
					}
				}
			}
		}

		public NameBank(PawnNameCategory ID)
		{
			this.nameType = ID;
			this.names = new List<string>[NameBank.numGenders, NameBank.numSlots];
			for (int i = 0; i < NameBank.numGenders; i++)
			{
				for (int j = 0; j < NameBank.numSlots; j++)
				{
					this.names[i, j] = new List<string>();
				}
			}
		}

		public void ErrorCheck()
		{
			foreach (List<string> allNameList in this.AllNameLists)
			{
				List<string> list = (from x in allNameList
				group x by x into g
				where g.Count() > 1
				select g.Key).ToList();
				List<string>.Enumerator enumerator2 = list.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						string current2 = enumerator2.Current;
						Log.Error("Duplicated name: " + current2);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
				List<string>.Enumerator enumerator3 = allNameList.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						string current3 = enumerator3.Current;
						if (current3.Trim() != current3)
						{
							Log.Error("Trimmable whitespace on name: [" + current3 + "]");
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator3).Dispose();
				}
			}
		}

		private List<string> NamesFor(PawnNameSlot slot, Gender gender)
		{
			return this.names[(uint)gender, (uint)slot];
		}

		public void AddNames(PawnNameSlot slot, Gender gender, IEnumerable<string> namesToAdd)
		{
			foreach (string item in namesToAdd)
			{
				this.NamesFor(slot, gender).Add(item);
			}
		}

		public void AddNamesFromFile(PawnNameSlot slot, Gender gender, string fileName)
		{
			this.AddNames(slot, gender, GenFile.LinesFromFile("Names/" + fileName));
		}

		public string GetName(PawnNameSlot slot, Gender gender = Gender.None)
		{
			List<string> list = this.NamesFor(slot, gender);
			int num = 0;
			if (list.Count == 0)
			{
				Log.Error("Name list for gender=" + gender + " slot=" + slot + " is empty.");
				return "Errorname";
			}
			string text;
			while (true)
			{
				text = list.RandomElement();
				if (!NameUseChecker.NameWordIsUsed(text))
				{
					return text;
				}
				num++;
				if (num > 50)
					break;
			}
			return text;
		}
	}
}
