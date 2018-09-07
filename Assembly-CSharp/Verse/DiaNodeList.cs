using System;
using System.Collections.Generic;

namespace Verse
{
	public class DiaNodeList
	{
		public string Name = "NeedsName";

		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		public List<string> NodeNames = new List<string>();

		public DiaNodeList()
		{
		}

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
