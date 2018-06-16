using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EC3 RID: 3779
	public class DiaNodeList
	{
		// Token: 0x06005944 RID: 22852 RVA: 0x002DB5C4 File Offset: 0x002D99C4
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

		// Token: 0x04003B99 RID: 15257
		public string Name = "NeedsName";

		// Token: 0x04003B9A RID: 15258
		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x04003B9B RID: 15259
		public List<string> NodeNames = new List<string>();
	}
}
