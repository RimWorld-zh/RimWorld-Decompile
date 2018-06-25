using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BEB RID: 3051
	public class Rule_File : Rule
	{
		// Token: 0x04002D8C RID: 11660
		[MayTranslate]
		public string path = null;

		// Token: 0x04002D8D RID: 11661
		[MayTranslate]
		[TranslationCanChangeCount]
		public List<string> pathList = new List<string>();

		// Token: 0x04002D8E RID: 11662
		[Unsaved]
		private List<string> cachedStrings = new List<string>();

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x0600428F RID: 17039 RVA: 0x00231394 File Offset: 0x0022F794
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.cachedStrings.Count;
			}
		}

		// Token: 0x06004290 RID: 17040 RVA: 0x002313B8 File Offset: 0x0022F7B8
		public override string Generate()
		{
			return this.cachedStrings.RandomElement<string>();
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x002313D8 File Offset: 0x0022F7D8
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

		// Token: 0x06004292 RID: 17042 RVA: 0x00231454 File Offset: 0x0022F854
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

		// Token: 0x06004293 RID: 17043 RVA: 0x002314C4 File Offset: 0x0022F8C4
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
