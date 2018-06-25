using System;
using System.Globalization;

namespace Verse
{
	public abstract class LanguageWorker
	{
		protected LanguageWorker()
		{
		}

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

		public string WithIndefiniteArticlePostProcessed(string str)
		{
			return this.PostProcessed(this.WithIndefiniteArticle(str));
		}

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

		public string WithDefiniteArticlePostProcessed(string str)
		{
			return this.PostProcessed(this.WithDefiniteArticle(str));
		}

		public virtual string OrdinalNumber(int number)
		{
			return number.ToString();
		}

		public virtual string PostProcessed(string str)
		{
			str = str.MergeMultipleSpaces(true);
			return str;
		}

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

		public virtual string Pluralize(string str, int count = -1)
		{
			return str;
		}
	}
}
