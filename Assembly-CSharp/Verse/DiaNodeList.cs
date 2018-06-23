using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000EC1 RID: 3777
	public class DiaNodeList
	{
		// Token: 0x04003BA8 RID: 15272
		public string Name = "NeedsName";

		// Token: 0x04003BA9 RID: 15273
		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x04003BAA RID: 15274
		public List<string> NodeNames = new List<string>();

		// Token: 0x06005963 RID: 22883 RVA: 0x002DD248 File Offset: 0x002DB648
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
