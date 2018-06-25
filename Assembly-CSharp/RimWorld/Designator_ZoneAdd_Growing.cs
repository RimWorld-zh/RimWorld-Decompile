using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007EC RID: 2028
	public class Designator_ZoneAdd_Growing : Designator_ZoneAdd
	{
		// Token: 0x06002D0B RID: 11531 RVA: 0x0017AD94 File Offset: 0x00179194
		public Designator_ZoneAdd_Growing()
		{
			this.zoneTypeToPlace = typeof(Zone_Growing);
			this.defaultLabel = "GrowingZone".Translate();
			this.defaultDesc = "DesignatorGrowingZoneDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true);
			this.hotKey = KeyBindingDefOf.Misc2;
			this.tutorTag = "ZoneAdd_Growing";
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002D0C RID: 11532 RVA: 0x0017AE00 File Offset: 0x00179200
		protected override string NewZoneLabel
		{
			get
			{
				return "GrowingZone".Translate();
			}
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x0017AE20 File Offset: 0x00179220
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!base.CanDesignateCell(c).Accepted)
			{
				result = false;
			}
			else if (base.Map.fertilityGrid.FertilityAt(c) < ThingDefOf.Plant_Potato.plant.fertilityMin)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x0017AE8C File Offset: 0x0017928C
		protected override Zone MakeNewZone()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
			return new Zone_Growing(Find.CurrentMap.zoneManager);
		}
	}
}
