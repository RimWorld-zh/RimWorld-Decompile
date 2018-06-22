using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Verse
{
	// Token: 0x02000EC3 RID: 3779
	public class DiaOptionMold
	{
		// Token: 0x0600596D RID: 22893 RVA: 0x002DD624 File Offset: 0x002DBA24
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

		// Token: 0x04003BB5 RID: 15285
		public string Text = "OK".Translate();

		// Token: 0x04003BB6 RID: 15286
		[XmlElement("Node")]
		public List<DiaNodeMold> ChildNodes = new List<DiaNodeMold>();

		// Token: 0x04003BB7 RID: 15287
		[XmlElement("NodeName")]
		[DefaultValue("")]
		public List<string> ChildNodeNames = new List<string>();
	}
}
