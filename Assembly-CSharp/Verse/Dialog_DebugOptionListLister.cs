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
			List<DebugMenuOption>.Enumerator enumerator = this.options.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DebugMenuOption current = enumerator.Current;
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
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
