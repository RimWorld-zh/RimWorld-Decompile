using System;
using System.Collections.Generic;

namespace Verse
{
	public class DiaNodeList
	{
		public string Name = "NeedsName";

		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		public List<string> NodeNames = new List<string>();

		public DiaNodeMold RandomNodeFromList()
		{
			List<DiaNodeMold> list = this.Nodes.ListFullCopy();
			List<string>.Enumerator enumerator = this.NodeNames.GetEnumerator();
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
			return list.RandomElement();
		}
	}
}
