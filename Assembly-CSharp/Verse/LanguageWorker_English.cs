using System;

namespace Verse
{
	// Token: 0x02000BF8 RID: 3064
	public class LanguageWorker_English : LanguageWorker
	{
		// Token: 0x060042D4 RID: 17108 RVA: 0x00235308 File Offset: 0x00233708
		public override string WithIndefiniteArticle(string str)
		{
			if (str.NullOrEmpty())
			{
				throw new ArgumentException();
			}
			return "a " + str;
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x0023533C File Offset: 0x0023373C
		public override string WithDefiniteArticle(string str)
		{
			if (str.NullOrEmpty())
			{
				throw new ArgumentException();
			}
			return "the " + str;
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x00235370 File Offset: 0x00233770
		public override string PostProcessed(string str)
		{
			str = base.PostProcessed(str);
			if (str.StartsWith("a ", StringComparison.OrdinalIgnoreCase) && str.Length >= 3)
			{
				if (str.Substring(2) == "hour" || str[2] == 'a' || str[2] == 'e' || str[2] == 'i' || str[2] == 'o' || str[2] == 'u')
				{
					str = str.Insert(1, "n");
				}
			}
			str = str.Replace(" a a", " an a");
			str = str.Replace(" a e", " an e");
			str = str.Replace(" a i", " an i");
			str = str.Replace(" a o", " an o");
			str = str.Replace(" a u", " an u");
			str = str.Replace(" a hour", " an hour");
			str = str.Replace(" A a", " An a");
			str = str.Replace(" A e", " An e");
			str = str.Replace(" A i", " An i");
			str = str.Replace(" A o", " An o");
			str = str.Replace(" A u", " An u");
			str = str.Replace(" A hour", " An hour");
			return str;
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x002354F4 File Offset: 0x002338F4
		public override string ToTitleCase(string str)
		{
			str = base.ToTitleCase(str);
			str = str.Replace(" No. ", " no. ");
			str = str.Replace(" The ", " the ");
			str = str.Replace(" A ", " a ");
			str = str.Replace(" For ", " for ");
			str = str.Replace(" In ", " in ");
			str = str.Replace(" With ", " with ");
			return str;
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x00235580 File Offset: 0x00233980
		public override string OrdinalNumber(int number)
		{
			int num = number % 10;
			int num2 = number / 10 % 10;
			if (num2 != 1)
			{
				if (num == 1)
				{
					return number + "st";
				}
				if (num == 2)
				{
					return number + "nd";
				}
				if (num == 3)
				{
					return number + "rd";
				}
			}
			return number + "th";
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x00235614 File Offset: 0x00233A14
		public override string Pluralize(string str, int count = -1)
		{
			string result;
			if (str.NullOrEmpty())
			{
				result = str;
			}
			else
			{
				char c = str[str.Length - 1];
				char c2 = (str.Length != 1) ? str[str.Length - 2] : '\0';
				bool flag = char.IsLetter(c2) && "oaieuyOAIEUY".IndexOf(c2) >= 0;
				bool flag2 = char.IsLetter(c2) && !flag;
				if (c == 'y' && flag2)
				{
					result = str.Substring(0, str.Length - 1) + "ies";
				}
				else
				{
					result = str + "s";
				}
			}
			return result;
		}
	}
}
