using System;
using System.Globalization;

namespace Verse
{
	public abstract class LanguageWorker
	{
		public virtual string WithIndefiniteArticle(string str)
		{
			if (str.NullOrEmpty())
			{
				throw new ArgumentException();
			}
			return (!"IndefiniteForm".CanTranslate()) ? ("IndefiniteArticle".Translate() + " " + str) : "IndefiniteForm".Translate(str);
		}

		public virtual string WithDefiniteArticle(string str)
		{
			if (str.NullOrEmpty())
			{
				throw new ArgumentException();
			}
			return (!"DefiniteForm".CanTranslate()) ? ("DefiniteArticle".Translate() + " " + str) : "DefiniteForm".Translate(str);
		}

		public virtual string OrdinalNumber(int number)
		{
			return number.ToString();
		}

		public virtual string PostProcessed(string str)
		{
			str = str.Replace("  ", " ");
			return str;
		}

		public virtual string ToTitleCase(string str)
		{
			return (!str.NullOrEmpty()) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str) : str;
		}

		public virtual string Pluralize(string str, int count = -1)
		{
			return str;
		}
	}
}
