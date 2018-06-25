using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompProperties_Facility : CompProperties
	{
		[Unsaved]
		public List<ThingDef> linkableBuildings = null;

		public List<StatModifier> statOffsets = null;

		public int maxSimultaneous = 1;

		public bool mustBePlacedAdjacent = false;

		public bool mustBePlacedAdjacentCardinalToBedHead = false;

		public bool canLinkToMedBedsOnly = false;

		public float maxDistance = 8f;

		public CompProperties_Facility()
		{
			this.compClass = typeof(CompFacility);
		}

		public override void ResolveReferences(ThingDef parentDef)
		{
			this.linkableBuildings = new List<ThingDef>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				CompProperties_AffectedByFacilities compProperties = allDefsListForReading[i].GetCompProperties<CompProperties_AffectedByFacilities>();
				if (compProperties != null && compProperties.linkableFacilities != null)
				{
					for (int j = 0; j < compProperties.linkableFacilities.Count; j++)
					{
						if (compProperties.linkableFacilities[j] == parentDef)
						{
							this.linkableBuildings.Add(allDefsListForReading[i]);
							break;
						}
					}
				}
			}
		}
	}
}
