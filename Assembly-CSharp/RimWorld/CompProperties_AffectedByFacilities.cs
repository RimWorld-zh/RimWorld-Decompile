using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompProperties_AffectedByFacilities : CompProperties
	{
		public List<ThingDef> linkableFacilities;

		public CompProperties_AffectedByFacilities()
		{
			base.compClass = typeof(CompAffectedByFacilities);
		}
	}
}
