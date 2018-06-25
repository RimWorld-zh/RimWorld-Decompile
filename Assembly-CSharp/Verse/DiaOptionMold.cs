using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Verse
{
	public class DiaOptionMold
	{
		public string Text = "OK".Translate();

		[XmlElement("Node")]
		public List<DiaNodeMold> ChildNodes = new List<DiaNodeMold>();

		[XmlElement("NodeName")]
		[DefaultValue("")]
		public List<string> ChildNodeNames = new List<string>();

		public DiaOptionMold()
		{
		}

		public DiaNodeMold RandomLinkNode()
		{
			List<DiaNodeMold> list = this.ChildNodes.ListFullCopy<DiaNodeMold>();
			foreach (string nodeName in this.ChildNodeNames)
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
			DiaNodeMold result;
			if (list.Count == 0)
			{
				result = null;
			}
			else
			{
				result = list.RandomElement<DiaNodeMold>();
			}
			return result;
		}
	}
}
