using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EC1 RID: 3777
	public class DiaNodeMold
	{
		// Token: 0x06005940 RID: 22848 RVA: 0x002DB4B8 File Offset: 0x002D98B8
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

		// Token: 0x04003B91 RID: 15249
		public string name = "Unnamed";

		// Token: 0x04003B92 RID: 15250
		public bool unique = false;

		// Token: 0x04003B93 RID: 15251
		public List<string> texts = new List<string>();

		// Token: 0x04003B94 RID: 15252
		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		// Token: 0x04003B95 RID: 15253
		[Unsaved]
		public bool isRoot = true;

		// Token: 0x04003B96 RID: 15254
		[Unsaved]
		public bool used = false;

		// Token: 0x04003B97 RID: 15255
		[Unsaved]
		public DiaNodeType nodeType = DiaNodeType.Undefined;
	}
}
