using System;
using System.Globalization;

namespace Verse
{
	// Token: 0x02000BF7 RID: 3063
	public abstract class LanguageWorker
	{
		// Token: 0x060042CC RID: 17100 RVA: 0x00235174 File Offset: 0x00233574
		public virtual string WithIndefiniteArticle(string str)
		{
			if (str.NullOrEmpty())
			{
				throw new ArgumentException();
			}
			string result;
			if ("IndefiniteForm".CanTranslate())
			{
				result = "IndefiniteForm".Translate(new object[]
				{
					str
				});
			}
			else
			{
				result = "IndefiniteArticle".Translate() + " " + str;
			}
			return result;
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x002351D8 File Offset: 0x002335D8
		public string WithIndefiniteArticlePostProcessed(string str)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str));
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x002351FC File Offset: 0x002335FC
		public virtual string WithDefiniteArticle(string str)
		{
			if (str.NullOrEmpty())
			{
				throw new ArgumentException();
			}
			string result;
			if ("DefiniteForm".CanTranslate())
			{
				result = "DefiniteForm".Translate(new object[]
				{
					str
				});
			}
			else
			{
				result = "DefiniteArticle".Translate() + " " + str;
			}
			return result;
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x00235260 File Offset: 0x00233660
		public string WithDefiniteArticlePostProcessed(string str)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str));
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x00235284 File Offset: 0x00233684
		public virtual string OrdinalNumber(int number)
		{
			return number.ToString();
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x002352A8 File Offset: 0x002336A8
		public virtual string PostProcessed(string str)
		{
			str = str.Replace("  ", " ");
			return str;
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x002352D0 File Offset: 0x002336D0
		public virtual string ToTitleCase(string str)
		{
			string result;
			if (str.NullOrEmpty())
			{
				result = str;
			}
			else
			{
				result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
			}
			return result;
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x00235308 File Offset: 0x00233708
		public virtual string Pluralize(string str, int count = -1)
		{
			return str;
		}
	}
}
