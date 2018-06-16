using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008FC RID: 2300
	public static class BuildFacilityCommandUtility
	{
		// Token: 0x06003536 RID: 13622 RVA: 0x001C72F4 File Offset: 0x001C56F4
		public static IEnumerable<Command> BuildFacilityCommands(BuildableDef building)
		{
			ThingDef thingDef = building as ThingDef;
			if (thingDef == null)
			{
				yield break;
			}
			CompProperties_AffectedByFacilities affectedByFacilities = thingDef.GetCompProperties<CompProperties_AffectedByFacilities>();
			if (affectedByFacilities == null)
			{
				yield break;
			}
			for (int i = 0; i < affectedByFacilities.linkableFacilities.Count; i++)
			{
				ThingDef facility = affectedByFacilities.linkableFacilities[i];
				Designator_Build des = BuildCopyCommandUtility.FindAllowedDesignator(facility, true);
				if (des != null)
				{
					yield return des;
				}
			}
			yield break;
		}
	}
}
