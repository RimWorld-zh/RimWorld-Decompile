using System;
using System.Globalization;

namespace Verse
{
	// Token: 0x02000BF2 RID: 3058
	public abstract class LanguageWorker
	{
		// Token: 0x060042CD RID: 17101 RVA: 0x00235D50 File Offset: 0x00234150
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

		// Token: 0x060042CE RID: 17102 RVA: 0x00235DB4 File Offset: 0x002341B4
		public string WithIndefiniteArticlePostProcessed(string str)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str));
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x00235DD8 File Offset: 0x002341D8
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

		// Token: 0x060042D0 RID: 17104 RVA: 0x00235E3C File Offset: 0x0023423C
		public string WithDefiniteArticlePostProcessed(string str)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str));
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x00235E60 File Offset: 0x00234260
		public virtual string OrdinalNumber(int number)
		{
			return number.ToString();
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x00235E84 File Offset: 0x00234284
		public virtual string PostProcessed(string str)
		{
			str = str.MergeMultipleSpaces(true);
			return str;
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x00235EA4 File Offset: 0x002342A4
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

		// Token: 0x060042D4 RID: 17108 RVA: 0x00235EDC File Offset: 0x002342DC
		public virtual string Pluralize(string str, int count = -1)
		{
			return str;
		}
	}
}
