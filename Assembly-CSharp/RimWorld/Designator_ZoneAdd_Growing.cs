using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007EA RID: 2026
	public class Designator_ZoneAdd_Growing : Designator_ZoneAdd
	{
		// Token: 0x06002D07 RID: 11527 RVA: 0x0017AC44 File Offset: 0x00179044
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
		// (get) Token: 0x06002D08 RID: 11528 RVA: 0x0017ACB0 File Offset: 0x001790B0
		protected override string NewZoneLabel
		{
			get
			{
				return "GrowingZone".Translate();
			}
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x0017ACD0 File Offset: 0x001790D0
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

		// Token: 0x06002D0A RID: 11530 RVA: 0x0017AD3C File Offset: 0x0017913C
		protected override Zone MakeNewZone()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
			return new Zone_Growing(Find.CurrentMap.zoneManager);
		}
	}
}
