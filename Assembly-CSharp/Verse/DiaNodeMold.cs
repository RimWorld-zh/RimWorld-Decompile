using System;
using System.Collections.Generic;

namespace Verse
{
	public class DiaNodeMold
	{
		public string name = "Unnamed";

		public bool unique = false;

		public List<string> texts = new List<string>();

		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		[Unsaved]
		public bool isRoot = true;

		[Unsaved]
		public bool used = false;

		[Unsaved]
		public DiaNodeType nodeType = DiaNodeType.Undefined;

		public void PostLoad()
		{
			int num = 0;
			foreach (string item in this.texts.ListFullCopy())
			{
				this.texts[num] = item.Replace("\\n", Environment.NewLine);
				num++;
			}
			foreach (DiaOptionMold option in this.optionList)
			{
				foreach (DiaNodeMold childNode in option.ChildNodes)
				{
					childNode.PostLoad();
				}
			}
		}
	}
}
