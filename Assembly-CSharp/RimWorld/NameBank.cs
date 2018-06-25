using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000483 RID: 1155
	public class NameBank
	{
		// Token: 0x04000C32 RID: 3122
		public PawnNameCategory nameType;

		// Token: 0x04000C33 RID: 3123
		private List<string>[,] names;

		// Token: 0x04000C34 RID: 3124
		private static readonly int numGenders = Enum.GetValues(typeof(Gender)).Length;

		// Token: 0x04000C35 RID: 3125
		private static readonly int numSlots = Enum.GetValues(typeof(PawnNameSlot)).Length;

		// Token: 0x06001466 RID: 5222 RVA: 0x000B2BF0 File Offset: 0x000B0FF0
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

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06001467 RID: 5223 RVA: 0x000B2C64 File Offset: 0x000B1064
		private IEnumerable<List<string>> AllNameLists
		{
			get
			{
				for (int i = 0; i < NameBank.numGenders; i++)
				{
					for (int j = 0; j < NameBank.numSlots; j++)
					{
						yield return this.names[i, j];
					}
				}
				yield break;
			}
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x000B2C90 File Offset: 0x000B1090
		public void ErrorCheck()
		{
			foreach (List<string> list in this.AllNameLists)
			{
				List<string> list2 = (from x in list
				group x by x into g
				where g.Count<string>() > 1
				select g.Key).ToList<string>();
				foreach (string str in list2)
				{
					Log.Error("Duplicated name: " + str, false);
				}
				foreach (string text in list)
				{
					if (text.Trim() != text)
					{
						Log.Error("Trimmable whitespace on name: [" + text + "]", false);
					}
				}
			}
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x000B2E3C File Offset: 0x000B123C
		private List<string> NamesFor(PawnNameSlot slot, Gender gender)
		{
			return this.names[(int)gender, (int)slot];
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x000B2E60 File Offset: 0x000B1260
		public void AddNames(PawnNameSlot slot, Gender gender, IEnumerable<string> namesToAdd)
		{
			foreach (string item in namesToAdd)
			{
				this.NamesFor(slot, gender).Add(item);
			}
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x000B2EC0 File Offset: 0x000B12C0
		public void AddNamesFromFile(PawnNameSlot slot, Gender gender, string fileName)
		{
			this.AddNames(slot, gender, GenFile.LinesFromFile("Names/" + fileName));
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x000B2EDC File Offset: 0x000B12DC
		public string GetName(PawnNameSlot slot, Gender gender = Gender.None, bool checkIfAlreadyUsed = true)
		{
			List<string> list = this.NamesFor(slot, gender);
			int num = 0;
			string result;
			if (list.Count == 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Name list for gender=",
					gender,
					" slot=",
					slot,
					" is empty."
				}), false);
				result = "Errorname";
			}
			else
			{
				string text;
				for (;;)
				{
					text = list.RandomElement<string>();
					if (checkIfAlreadyUsed && !NameUseChecker.NameWordIsUsed(text))
					{
						break;
					}
					num++;
					if (num > 50)
					{
						goto Block_4;
					}
				}
				return text;
				Block_4:
				result = text;
			}
			return result;
		}
	}
}
