using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EBF RID: 3775
	public class DiaNode
	{
		// Token: 0x0600593B RID: 22843 RVA: 0x002DB345 File Offset: 0x002D9745
		public DiaNode(string text)
		{
			this.text = text;
		}

		// Token: 0x0600593C RID: 22844 RVA: 0x002DB360 File Offset: 0x002D9760
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

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x0600593D RID: 22845 RVA: 0x002DB444 File Offset: 0x002D9844
		protected Dialog_NodeTree OwnerBox
		{
			get
			{
				return Find.WindowStack.WindowOfType<Dialog_NodeTree>();
			}
		}

		// Token: 0x0600593E RID: 22846 RVA: 0x002DB463 File Offset: 0x002D9863
		public void PreClose()
		{
		}

		// Token: 0x04003B89 RID: 15241
		public string text;

		// Token: 0x04003B8A RID: 15242
		public List<DiaOption> options = new List<DiaOption>();

		// Token: 0x04003B8B RID: 15243
		protected DiaNodeMold def;
	}
}
