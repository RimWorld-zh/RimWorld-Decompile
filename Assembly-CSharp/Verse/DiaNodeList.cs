using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EC4 RID: 3780
	public class DiaNodeList
	{
		// Token: 0x04003BB0 RID: 15280
		public string Name = "NeedsName";

		// Token: 0x04003BB1 RID: 15281
		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x04003BB2 RID: 15282
		public List<string> NodeNames = new List<string>();

		// Token: 0x06005966 RID: 22886 RVA: 0x002DD554 File Offset: 0x002DB954
		public DiaNodeMold RandomNodeFromList()
		{
			List<DiaNodeMold> list = this.Nodes.ListFullCopy<DiaNodeMold>();
			foreach (string nodeName in this.NodeNames)
			{
				list.Add(DialogDatabase.GetNodeNamed(nodeName));
			}
			foreach (DiaNodeMold diaNodeMold in list)
			{
				if (diaNodeMold.unique && diaNodeMold.used)
				{
					list.Remove(diaNodeMold);
				}
			}
			return list.RandomElement<DiaNodeMold>();
		}
	}
}
