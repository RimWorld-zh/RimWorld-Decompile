using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EC3 RID: 3779
	public class DiaNodeMold
	{
		// Token: 0x04003BA9 RID: 15273
		public string name = "Unnamed";

		// Token: 0x04003BAA RID: 15274
		public bool unique = false;

		// Token: 0x04003BAB RID: 15275
		public List<string> texts = new List<string>();

		// Token: 0x04003BAC RID: 15276
		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		// Token: 0x04003BAD RID: 15277
		[Unsaved]
		public bool isRoot = true;

		// Token: 0x04003BAE RID: 15278
		[Unsaved]
		public bool used = false;

		// Token: 0x04003BAF RID: 15279
		[Unsaved]
		public DiaNodeType nodeType = DiaNodeType.Undefined;

		// Token: 0x06005964 RID: 22884 RVA: 0x002DD410 File Offset: 0x002DB810
		public void PostLoad()
		{
			int num = 0;
			foreach (string text in this.texts.ListFullCopy<string>())
			{
				this.texts[num] = text.Replace("\\n", Environment.NewLine);
				num++;
			}
			foreach (DiaOptionMold diaOptionMold in this.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.PostLoad();
				}
			}
		}
	}
}
