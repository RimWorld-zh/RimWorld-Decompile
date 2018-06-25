using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse
{
	public class Dialog_DebugOptionListLister : Dialog_DebugOptionLister
	{
		protected List<DebugMenuOption> options;

		public Dialog_DebugOptionListLister(IEnumerable<DebugMenuOption> options)
		{
			this.options = options.ToList<DebugMenuOption>();
		}

		protected override void DoListingItems()
		{
			foreach (DebugMenuOption debugMenuOption in this.options)
			{
				if (debugMenuOption.mode == DebugMenuOptionMode.Action)
				{
					base.DebugAction(debugMenuOption.label, debugMenuOption.method);
				}
				if (debugMenuOption.mode == DebugMenuOptionMode.Tool)
				{
					base.DebugToolMap(debugMenuOption.label, debugMenuOption.method);
				}
			}
		}

		public static void ShowSimpleDebugMenu<T>(IEnumerable<T> elements, Func<T, string> label, Action<T> chosen)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<T> enumerator = elements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t = enumerator.Current;
					list.Add(new DebugMenuOption(label(t), DebugMenuOptionMode.Action, delegate()
					{
						chosen(t);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		[CompilerGenerated]
		private sealed class <ShowSimpleDebugMenu>c__AnonStorey0<T>
		{
			internal Action<T> chosen;

			public <ShowSimpleDebugMenu>c__AnonStorey0()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <ShowSimpleDebugMenu>c__AnonStorey1<T>
		{
			internal T t;

			internal Dialog_DebugOptionListLister.<ShowSimpleDebugMenu>c__AnonStorey0<T> <>f__ref$0;

			public <ShowSimpleDebugMenu>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.<>f__ref$0.chosen(this.t);
			}
		}
	}
}
