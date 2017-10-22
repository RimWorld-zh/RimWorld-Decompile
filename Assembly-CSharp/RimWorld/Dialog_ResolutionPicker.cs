using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Dialog_ResolutionPicker : Dialog_OptionLister
	{
		public Dialog_ResolutionPicker()
		{
			base.doCloseButton = true;
			base.absorbInputAroundWindow = true;
		}

		protected override void DoListingItems()
		{
			List<Resolution> list = (from r in Screen.resolutions
			where r.width >= 1024 && r.height >= 768
			orderby r.width, r.height
			select r).ToList();
			if (list.Count == 0)
			{
				for (int i = 0; i < 30; i++)
				{
					list.Add(new Resolution
					{
						height = 768,
						width = 1024
					});
				}
			}
			List<Resolution>.Enumerator enumerator = list.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Resolution current = enumerator.Current;
					if (base.listing.ButtonText(Dialog_Options.ResToString(current.width, current.height), (string)null))
					{
						if (!ResolutionUtility.UIScaleSafeWithResolution(Prefs.UIScale, current.width, current.height))
						{
							Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageSound.RejectInput);
						}
						else
						{
							Find.WindowStack.TryRemove(this, true);
							ResolutionUtility.SafeSetResolution(current);
						}
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
