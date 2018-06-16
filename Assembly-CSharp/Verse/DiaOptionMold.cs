using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Verse
{
	// Token: 0x02000EC5 RID: 3781
	public class DiaOptionMold
	{
		// Token: 0x0600594E RID: 22862 RVA: 0x002DB9A0 File Offset: 0x002D9DA0
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

		// Token: 0x04003BA6 RID: 15270
		public string Text = "OK".Translate();

		// Token: 0x04003BA7 RID: 15271
		[XmlElement("Node")]
		public List<DiaNodeMold> ChildNodes = new List<DiaNodeMold>();

		// Token: 0x04003BA8 RID: 15272
		[XmlElement("NodeName")]
		[DefaultValue("")]
		public List<string> ChildNodeNames = new List<string>();
	}
}
