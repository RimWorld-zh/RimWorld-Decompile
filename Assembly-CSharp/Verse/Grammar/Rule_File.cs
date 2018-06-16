using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BEC RID: 3052
	public class Rule_File : Rule
	{
		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06004288 RID: 17032 RVA: 0x002306AC File Offset: 0x0022EAAC
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.cachedStrings.Count;
			}
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x002306D0 File Offset: 0x0022EAD0
		public override string Generate()
		{
			return this.cachedStrings.RandomElement<string>();
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x002306F0 File Offset: 0x0022EAF0
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

		// Token: 0x0600428B RID: 17035 RVA: 0x0023076C File Offset: 0x0022EB6C
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

		// Token: 0x0600428C RID: 17036 RVA: 0x002307DC File Offset: 0x0022EBDC
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

		// Token: 0x04002D7F RID: 11647
		private string path = null;

		// Token: 0x04002D80 RID: 11648
		private List<string> pathList = new List<string>();

		// Token: 0x04002D81 RID: 11649
		private List<string> cachedStrings = new List<string>();
	}
}
