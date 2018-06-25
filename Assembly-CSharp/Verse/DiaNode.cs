using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EC0 RID: 3776
	public class DiaNode
	{
		// Token: 0x04003B99 RID: 15257
		public string text;

		// Token: 0x04003B9A RID: 15258
		public List<DiaOption> options = new List<DiaOption>();

		// Token: 0x04003B9B RID: 15259
		protected DiaNodeMold def;

		// Token: 0x0600595F RID: 22879 RVA: 0x002DD0B1 File Offset: 0x002DB4B1
		public DiaNode(string text)
		{
			this.text = text;
		}

		// Token: 0x06005960 RID: 22880 RVA: 0x002DD0CC File Offset: 0x002DB4CC
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
		// (get) Token: 0x06005961 RID: 22881 RVA: 0x002DD1B0 File Offset: 0x002DB5B0
		protected Dialog_NodeTree OwnerBox
		{
			get
			{
				return Find.WindowStack.WindowOfType<Dialog_NodeTree>();
			}
		}

		// Token: 0x06005962 RID: 22882 RVA: 0x002DD1CF File Offset: 0x002DB5CF
		public void PreClose()
		{
		}
	}
}
