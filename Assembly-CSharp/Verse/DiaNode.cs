using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EC0 RID: 3776
	public class DiaNode
	{
		// Token: 0x0600593D RID: 22845 RVA: 0x002DB30D File Offset: 0x002D970D
		public DiaNode(string text)
		{
			this.text = text;
		}

		// Token: 0x0600593E RID: 22846 RVA: 0x002DB328 File Offset: 0x002D9728
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

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x0600593F RID: 22847 RVA: 0x002DB40C File Offset: 0x002D980C
		protected Dialog_NodeTree OwnerBox
		{
			get
			{
				return Find.WindowStack.WindowOfType<Dialog_NodeTree>();
			}
		}

		// Token: 0x06005940 RID: 22848 RVA: 0x002DB42B File Offset: 0x002D982B
		public void PreClose()
		{
		}

		// Token: 0x04003B8A RID: 15242
		public string text;

		// Token: 0x04003B8B RID: 15243
		public List<DiaOption> options = new List<DiaOption>();

		// Token: 0x04003B8C RID: 15244
		protected DiaNodeMold def;
	}
}
