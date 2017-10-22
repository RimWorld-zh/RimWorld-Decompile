using System.Collections.Generic;

namespace Verse.Grammar
{
	public class Rule_File : Rule
	{
		private string path = (string)null;

		private List<string> pathList = new List<string>();

		private List<string> cachedStrings = new List<string>();

		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.cachedStrings.Count;
			}
		}

		public override string Generate()
		{
			return this.cachedStrings.RandomElement();
		}

		public override void Init()
		{
			if (!this.path.NullOrEmpty())
			{
				this.LoadStringsFromFile(this.path);
			}
			foreach (string path2 in this.pathList)
			{
				this.LoadStringsFromFile(path2);
			}
		}

		private void LoadStringsFromFile(string filePath)
		{
			List<string> list = default(List<string>);
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
			return this.path.NullOrEmpty() ? ((this.pathList.Count <= 0) ? (base.keyword + "->(Rule_File with no configuration)") : (base.keyword + "->(" + this.cachedStrings.Count + " strings from " + this.pathList.Count + " files)")) : (base.keyword + "->(" + this.cachedStrings.Count + " strings from file: " + this.path + ")");
		}
	}
}
