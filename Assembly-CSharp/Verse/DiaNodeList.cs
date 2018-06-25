using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EC3 RID: 3779
	public class DiaNodeList
	{
		// Token: 0x04003BA8 RID: 15272
		public string Name = "NeedsName";

		// Token: 0x04003BA9 RID: 15273
		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x04003BAA RID: 15274
		public List<string> NodeNames = new List<string>();

		// Token: 0x06005966 RID: 22886 RVA: 0x002DD368 File Offset: 0x002DB768
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
