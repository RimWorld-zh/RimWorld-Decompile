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

		public DiaNodeMold RandomLinkNode()
		{
			List<DiaNodeMold> list = this.ChildNodes.ListFullCopy();
			List<string>.Enumerator enumerator = this.ChildNodeNames.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					list.Add(DialogDatabase.GetNodeNamed(current));
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			List<DiaNodeMold>.Enumerator enumerator2 = list.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					DiaNodeMold current2 = enumerator2.Current;
					if (current2.unique && current2.used)
					{
						list.Remove(current2);
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list.RandomElement();
		}
	}
}
