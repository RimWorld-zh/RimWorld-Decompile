using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000E2E RID: 3630
	public class Dialog_DebugOptionListLister : Dialog_DebugOptionLister
	{
		// Token: 0x040038D6 RID: 14550
		protected List<DebugMenuOption> options;

		// Token: 0x0600560D RID: 22029 RVA: 0x002C5E62 File Offset: 0x002C4262
		public Dialog_DebugOptionListLister(IEnumerable<DebugMenuOption> options)
		{
			this.options = options.ToList<DebugMenuOption>();
		}

		// Token: 0x0600560E RID: 22030 RVA: 0x002C5E78 File Offset: 0x002C4278
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

		// Token: 0x0600560F RID: 22031 RVA: 0x002C5F14 File Offset: 0x002C4314
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
	}
}
