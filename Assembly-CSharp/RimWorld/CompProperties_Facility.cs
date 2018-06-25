using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000247 RID: 583
	public class CompProperties_Facility : CompProperties
	{
		// Token: 0x04000486 RID: 1158
		[Unsaved]
		public List<ThingDef> linkableBuildings = null;

		// Token: 0x04000487 RID: 1159
		public List<StatModifier> statOffsets = null;

		// Token: 0x04000488 RID: 1160
		public int maxSimultaneous = 1;

		// Token: 0x04000489 RID: 1161
		public bool mustBePlacedAdjacent = false;

		// Token: 0x0400048A RID: 1162
		public bool mustBePlacedAdjacentCardinalToBedHead = false;

		// Token: 0x0400048B RID: 1163
		public bool canLinkToMedBedsOnly = false;

		// Token: 0x0400048C RID: 1164
		public float maxDistance = 8f;

		// Token: 0x06000A7B RID: 2683 RVA: 0x0005F2B8 File Offset: 0x0005D6B8
		public CompProperties_Facility()
		{
			this.compClass = typeof(CompFacility);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0005F314 File Offset: 0x0005D714
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
