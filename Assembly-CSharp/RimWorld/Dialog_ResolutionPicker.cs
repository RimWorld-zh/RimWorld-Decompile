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
			foreach (Resolution item in list)
			{
				if (base.listing.ButtonText(Dialog_Options.ResToString(item.width, item.height), (string)null))
				{
					if (!ResolutionUtility.UIScaleSafeWithResolution(Prefs.UIScale, item.width, item.height))
					{
						Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageTypeDefOf.RejectInput);
					}
					else
					{
						Find.WindowStack.TryRemove(this, true);
						ResolutionUtility.SafeSetResolution(item);
					}
				}
			}
		}
	}
}
