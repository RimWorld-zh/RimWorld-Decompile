using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000E2F RID: 3631
	public class Dialog_DebugOptionListLister : Dialog_DebugOptionLister
	{
		// Token: 0x040038DE RID: 14558
		protected List<DebugMenuOption> options;

		// Token: 0x0600560D RID: 22029 RVA: 0x002C604E File Offset: 0x002C444E
		public Dialog_DebugOptionListLister(IEnumerable<DebugMenuOption> options)
		{
			this.options = options.ToList<DebugMenuOption>();
		}

		// Token: 0x0600560E RID: 22030 RVA: 0x002C6064 File Offset: 0x002C4464
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

		// Token: 0x0600560F RID: 22031 RVA: 0x002C6100 File Offset: 0x002C4500
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
