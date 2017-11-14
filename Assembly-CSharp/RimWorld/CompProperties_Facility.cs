using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompProperties_Facility : CompProperties
	{
		[Unsaved]
		public List<ThingDef> linkableBuildings;

		public List<StatModifier> statOffsets;

		public int maxSimultaneous = 1;

		public bool mustBePlacedAdjacent;

		public bool mustBePlacedAdjacentCardinalToBedHead;

		public bool canLinkToMedBedsOnly;

		public float maxDistance = 8f;

		public CompProperties_Facility()
		{
			base.compClass = typeof(CompFacility);
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
					int num = 0;
					while (num < compProperties.linkableFacilities.Count)
					{
						if (compProperties.linkableFacilities[num] != parentDef)
						{
							num++;
							continue;
						}
						this.linkableBuildings.Add(allDefsListForReading[i]);
						break;
					}
				}
			}
		}
	}
}
