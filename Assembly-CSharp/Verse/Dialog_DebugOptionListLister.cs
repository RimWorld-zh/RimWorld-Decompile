using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000E2F RID: 3631
	public class Dialog_DebugOptionListLister : Dialog_DebugOptionLister
	{
		// Token: 0x060055ED RID: 21997 RVA: 0x002C417A File Offset: 0x002C257A
		public Dialog_DebugOptionListLister(IEnumerable<DebugMenuOption> options)
		{
			this.options = options.ToList<DebugMenuOption>();
		}

		// Token: 0x060055EE RID: 21998 RVA: 0x002C4190 File Offset: 0x002C2590
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

		// Token: 0x060055EF RID: 21999 RVA: 0x002C422C File Offset: 0x002C262C
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

		// Token: 0x040038C8 RID: 14536
		protected List<DebugMenuOption> options;
	}
}
