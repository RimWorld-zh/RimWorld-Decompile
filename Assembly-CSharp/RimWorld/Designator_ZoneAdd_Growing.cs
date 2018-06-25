using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007EC RID: 2028
	public class Designator_ZoneAdd_Growing : Designator_ZoneAdd
	{
		// Token: 0x06002D0A RID: 11530 RVA: 0x0017AFF8 File Offset: 0x001793F8
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
		// (get) Token: 0x06002D0B RID: 11531 RVA: 0x0017B064 File Offset: 0x00179464
		protected override string NewZoneLabel
		{
			get
			{
				return "GrowingZone".Translate();
			}
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x0017B084 File Offset: 0x00179484
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

		// Token: 0x06002D0D RID: 11533 RVA: 0x0017B0F0 File Offset: 0x001794F0
		protected override Zone MakeNewZone()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
			return new Zone_Growing(Find.CurrentMap.zoneManager);
		}
	}
}
