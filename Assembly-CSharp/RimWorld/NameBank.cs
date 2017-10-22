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
				int j = 0;
				int i;
				while (true)
				{
					if (j < NameBank.numGenders)
					{
						i = 0;
						if (i < NameBank.numSlots)
							break;
						j++;
						continue;
					}
					yield break;
				}
				yield return this.names[j, i];
				/*Error: Unable to find new state assignment for yield return*/;
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
				foreach (string item in list)
				{
					Log.Error("Duplicated name: " + item);
				}
				foreach (string item2 in allNameList)
				{
					if (item2.Trim() != item2)
					{
						Log.Error("Trimmable whitespace on name: [" + item2 + "]");
					}
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
			string result;
			string text;
			if (list.Count == 0)
			{
				Log.Error("Name list for gender=" + gender + " slot=" + slot + " is empty.");
				result = "Errorname";
			}
			else
			{
				while (true)
				{
					text = list.RandomElement();
					if (NameUseChecker.NameWordIsUsed(text))
					{
						num++;
						if (num <= 50)
							continue;
						goto IL_0084;
					}
					break;
				}
				result = text;
			}
			goto IL_0091;
			IL_0091:
			return result;
			IL_0084:
			result = text;
			goto IL_0091;
		}
	}
}
