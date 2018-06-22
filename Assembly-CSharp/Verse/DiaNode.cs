using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EBE RID: 3774
	public class DiaNode
	{
		// Token: 0x0600595C RID: 22876 RVA: 0x002DCF91 File Offset: 0x002DB391
		public DiaNode(string text)
		{
			this.text = text;
		}

		// Token: 0x0600595D RID: 22877 RVA: 0x002DCFAC File Offset: 0x002DB3AC
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

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x0600595E RID: 22878 RVA: 0x002DD090 File Offset: 0x002DB490
		protected Dialog_NodeTree OwnerBox
		{
			get
			{
				return Find.WindowStack.WindowOfType<Dialog_NodeTree>();
			}
		}

		// Token: 0x0600595F RID: 22879 RVA: 0x002DD0AF File Offset: 0x002DB4AF
		public void PreClose()
		{
		}

		// Token: 0x04003B99 RID: 15257
		public string text;

		// Token: 0x04003B9A RID: 15258
		public List<DiaOption> options = new List<DiaOption>();

		// Token: 0x04003B9B RID: 15259
		protected DiaNodeMold def;
	}
}
