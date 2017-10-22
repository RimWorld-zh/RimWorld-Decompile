using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class Dialog_DebugOptionListLister : Dialog_DebugOptionLister
	{
		protected List<DebugMenuOption> options;

		public Dialog_DebugOptionListLister(IEnumerable<DebugMenuOption> options)
		{
			this.options = options.ToList();
		}

		protected override void DoListingItems()
		{
			foreach (DebugMenuOption option in this.options)
			{
				DebugMenuOption current = option;
				if (current.mode == DebugMenuOptionMode.Action)
				{
					base.DebugAction(current.label, current.method);
				}
				if (current.mode == DebugMenuOptionMode.Tool)
				{
					base.DebugToolMap(current.label, current.method);
				}
			}
		}

		public static void ShowSimpleDebugMenu<T>(IEnumerable<T> elements, Func<T, string> label, Action<T> chosen)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (T item in elements)
			{
				list.Add(new DebugMenuOption(label(item), DebugMenuOptionMode.Action, (Action)delegate()
				{
					chosen((T)item);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}
	}
}
