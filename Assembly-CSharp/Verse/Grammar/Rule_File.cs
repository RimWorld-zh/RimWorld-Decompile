using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE8 RID: 3048
	public class Rule_File : Rule
	{
		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x0600428C RID: 17036 RVA: 0x00230FD8 File Offset: 0x0022F3D8
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.cachedStrings.Count;
			}
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x00230FFC File Offset: 0x0022F3FC
		public override string Generate()
		{
			return this.cachedStrings.RandomElement<string>();
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x0023101C File Offset: 0x0022F41C
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

		// Token: 0x0600428F RID: 17039 RVA: 0x00231098 File Offset: 0x0022F498
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

		// Token: 0x06004290 RID: 17040 RVA: 0x00231108 File Offset: 0x0022F508
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

		// Token: 0x04002D85 RID: 11653
		[MayTranslate]
		public string path = null;

		// Token: 0x04002D86 RID: 11654
		[MayTranslate]
		[TranslationCanChangeCount]
		public List<string> pathList = new List<string>();

		// Token: 0x04002D87 RID: 11655
		[Unsaved]
		private List<string> cachedStrings = new List<string>();
	}
}
