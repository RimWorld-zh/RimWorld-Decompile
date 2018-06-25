using System;
using System.Collections.Generic;

namespace Verse
{
	public class DiaNode
	{
		public string text;

		public List<DiaOption> options = new List<DiaOption>();

		protected DiaNodeMold def;

		public DiaNode(string text)
		{
			this.text = text;
		}

		public DiaNode(DiaNodeMold newDef)
		{
			this.def = newDef;
			this.def.used = true;
			this.text = this.def.texts.RandomElement<string>();
			if (this.def.optionList.Count > 0)
			{
				foreach (DiaOptionMold diaOptionMold in this.def.optionList)
				{
					this.options.Add(new DiaOption(diaOptionMold));
				}
			}
			else
			{
				this.options.Add(new DiaOption("OK".Translate()));
			}
		}

		protected Dialog_NodeTree OwnerBox
		{
			get
			{
				return Find.WindowStack.WindowOfType<Dialog_NodeTree>();
			}
		}

		public void PreClose()
		{
		}
	}
}
