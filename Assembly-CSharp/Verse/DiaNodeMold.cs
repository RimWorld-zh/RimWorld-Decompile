using System;
using System.Collections.Generic;

namespace Verse
{
	public class DiaNodeMold
	{
		public string name = "Unnamed";

		public bool unique;

		public List<string> texts = new List<string>();

		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		[Unsaved]
		public bool isRoot = true;

		[Unsaved]
		public bool used;

		[Unsaved]
		public DiaNodeType nodeType;

		public void PostLoad()
		{
			int num = 0;
			List<string>.Enumerator enumerator = this.texts.ListFullCopy().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					this.texts[num] = current.Replace("\\n", Environment.NewLine);
					num++;
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			List<DiaOptionMold>.Enumerator enumerator2 = this.optionList.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					DiaOptionMold current2 = enumerator2.Current;
					List<DiaNodeMold>.Enumerator enumerator3 = current2.ChildNodes.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							DiaNodeMold current3 = enumerator3.Current;
							current3.PostLoad();
						}
					}
					finally
					{
						((IDisposable)(object)enumerator3).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
		}
	}
}
