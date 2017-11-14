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
					Resolution item = default(Resolution);
					item.height = 768;
					item.width = 1024;
					list.Add(item);
				}
			}
			foreach (Resolution item2 in list)
			{
				if (base.listing.ButtonText(Dialog_Options.ResToString(item2.width, item2.height), null))
				{
					if (!ResolutionUtility.UIScaleSafeWithResolution(Prefs.UIScale, item2.width, item2.height))
					{
						Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageTypeDefOf.RejectInput);
					}
					else
					{
						Find.WindowStack.TryRemove(this, true);
						ResolutionUtility.SafeSetResolution(item2);
					}
				}
			}
		}
	}
}
