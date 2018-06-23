using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000245 RID: 581
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

		// Token: 0x06000A77 RID: 2679 RVA: 0x0005F168 File Offset: 0x0005D568
		public CompProperties_Facility()
		{
			this.compClass = typeof(CompFacility);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x0005F1C4 File Offset: 0x0005D5C4
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
