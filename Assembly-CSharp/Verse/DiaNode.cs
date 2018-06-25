using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EC1 RID: 3777
	public class DiaNode
	{
		// Token: 0x04003BA1 RID: 15265
		public string text;

		// Token: 0x04003BA2 RID: 15266
		public List<DiaOption> options = new List<DiaOption>();

		// Token: 0x04003BA3 RID: 15267
		protected DiaNodeMold def;

		// Token: 0x0600595F RID: 22879 RVA: 0x002DD29D File Offset: 0x002DB69D
		public DiaNode(string text)
		{
			this.text = text;
		}

		// Token: 0x06005960 RID: 22880 RVA: 0x002DD2B8 File Offset: 0x002DB6B8
		public DiaNode(DiaNodeMold newDef)
		{
			this.def = newDef;
			this.def.used = true;
			this.text = this.def.texts.RandomElement<string>();
			if (this.def.optionList.Count > 0)
			{
				foreach (DiaOptionMold diaOptionMold in this.def.optionList)
				{
					this.options.Add(new DiaOption(diaOptionMold));
				}
			}
			else
			{
				this.options.Add(new DiaOption("OK".Translate()));
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06005961 RID: 22881 RVA: 0x002DD39C File Offset: 0x002DB79C
		protected Dialog_NodeTree OwnerBox
		{
			get
			{
				return Find.WindowStack.WindowOfType<Dialog_NodeTree>();
			}
		}

		// Token: 0x06005962 RID: 22882 RVA: 0x002DD3BB File Offset: 0x002DB7BB
		public void PreClose()
		{
		}
	}
}
