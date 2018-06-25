using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	public class Rule_File : Rule
	{
		[MayTranslate]
		public string path = null;

		[MayTranslate]
		[TranslationCanChangeCount]
		public List<string> pathList = new List<string>();

		[Unsaved]
		private List<string> cachedStrings = new List<string>();

		public Rule_File()
		{
		}

		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.cachedStrings.Count;
			}
		}

		public override string Generate()
		{
			return this.cachedStrings.RandomElement<string>();
		}

		public override void Init()
		{
			if (!this.path.NullOrEmpty())
			{
				this.LoadStringsFromFile(this.path);
			}
			foreach (string filePath in this.pathList)
			{
				this.LoadStringsFromFile(filePath);
			}
		}

		private void LoadStringsFromFile(string filePath)
		{
			List<string> list;
			if (Translator.TryGetTranslatedStringsForFile(filePath, out list))
			{
				foreach (string item in list)
				{
					this.cachedStrings.Add(item);
				}
			}
		}

		public override string ToString()
		{
			string result;
			if (!this.path.NullOrEmpty())
			{
				result = string.Concat(new object[]
				{
					this.keyword,
					"->(",
					this.cachedStrings.Count,
					" strings from file: ",
					this.path,
					")"
				});
			}
			else if (this.pathList.Count > 0)
			{
				result = string.Concat(new object[]
				{
					this.keyword,
					"->(",
					this.cachedStrings.Count,
					" strings from ",
					this.pathList.Count,
					" files)"
				});
			}
			else
			{
				result = this.keyword + "->(Rule_File with no configuration)";
			}
			return result;
		}
	}
}
