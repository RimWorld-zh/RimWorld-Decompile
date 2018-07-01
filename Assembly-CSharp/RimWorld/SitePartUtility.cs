using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public static class SitePartUtility
	{
		public static string GetDescriptionDialogue(Site site, SitePart sitePart)
		{
			string result;
			if (sitePart != null && !sitePart.def.alwaysHidden)
			{
				result = sitePart.def.Worker.GetPostProcessedDescriptionDialogue(site, sitePart);
			}
			else
			{
				result = "HiddenOrNoSitePartDescription".Translate();
			}
			return result;
		}
	}
}
