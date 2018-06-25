using System;
using System.Globalization;

namespace Verse
{
	// Token: 0x02000BF4 RID: 3060
	public abstract class LanguageWorker
	{
		// Token: 0x060042D0 RID: 17104 RVA: 0x00235E2C File Offset: 0x0023422C
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

		// Token: 0x060042D1 RID: 17105 RVA: 0x00235E90 File Offset: 0x00234290
		public string WithIndefiniteArticlePostProcessed(string str)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str));
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x00235EB4 File Offset: 0x002342B4
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

		// Token: 0x060042D3 RID: 17107 RVA: 0x00235F18 File Offset: 0x00234318
		public string WithDefiniteArticlePostProcessed(string str)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str));
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x00235F3C File Offset: 0x0023433C
		public virtual string OrdinalNumber(int number)
		{
			return number.ToString();
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x00235F60 File Offset: 0x00234360
		public virtual string PostProcessed(string str)
		{
			str = str.MergeMultipleSpaces(true);
			return str;
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x00235F80 File Offset: 0x00234380
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

		// Token: 0x060042D7 RID: 17111 RVA: 0x00235FB8 File Offset: 0x002343B8
		public virtual string Pluralize(string str, int count = -1)
		{
			return str;
		}
	}
}
