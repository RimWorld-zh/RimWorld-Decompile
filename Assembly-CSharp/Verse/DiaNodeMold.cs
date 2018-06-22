using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EC0 RID: 3776
	public class DiaNodeMold
	{
		// Token: 0x06005961 RID: 22881 RVA: 0x002DD104 File Offset: 0x002DB504
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

		// Token: 0x04003BA1 RID: 15265
		public string name = "Unnamed";

		// Token: 0x04003BA2 RID: 15266
		public bool unique = false;

		// Token: 0x04003BA3 RID: 15267
		public List<string> texts = new List<string>();

		// Token: 0x04003BA4 RID: 15268
		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		// Token: 0x04003BA5 RID: 15269
		[Unsaved]
		public bool isRoot = true;

		// Token: 0x04003BA6 RID: 15270
		[Unsaved]
		public bool used = false;

		// Token: 0x04003BA7 RID: 15271
		[Unsaved]
		public DiaNodeType nodeType = DiaNodeType.Undefined;
	}
}
