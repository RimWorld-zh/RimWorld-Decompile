using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F8 RID: 2296
	public static class BuildFacilityCommandUtility
	{
		// Token: 0x06003531 RID: 13617 RVA: 0x001C75A4 File Offset: 0x001C59A4
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
