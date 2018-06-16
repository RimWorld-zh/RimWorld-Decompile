using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EC2 RID: 3778
	public class DiaNodeMold
	{
		// Token: 0x06005942 RID: 22850 RVA: 0x002DB480 File Offset: 0x002D9880
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

		// Token: 0x04003B92 RID: 15250
		public string name = "Unnamed";

		// Token: 0x04003B93 RID: 15251
		public bool unique = false;

		// Token: 0x04003B94 RID: 15252
		public List<string> texts = new List<string>();

		// Token: 0x04003B95 RID: 15253
		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		// Token: 0x04003B96 RID: 15254
		[Unsaved]
		public bool isRoot = true;

		// Token: 0x04003B97 RID: 15255
		[Unsaved]
		public bool used = false;

		// Token: 0x04003B98 RID: 15256
		[Unsaved]
		public DiaNodeType nodeType = DiaNodeType.Undefined;
	}
}
