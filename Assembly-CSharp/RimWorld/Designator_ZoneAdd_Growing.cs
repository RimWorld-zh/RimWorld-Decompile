using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007EE RID: 2030
	public class Designator_ZoneAdd_Growing : Designator_ZoneAdd
	{
		// Token: 0x06002D0C RID: 11532 RVA: 0x0017A9D8 File Offset: 0x00178DD8
		public Designator_ZoneAdd_Growing()
		{
			this.zoneTypeToPlace = typeof(Zone_Growing);
			this.defaultLabel = "GrowingZone".Translate();
			this.defaultDesc = "DesignatorGrowingZoneDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true);
			this.hotKey = KeyBindingDefOf.Misc2;
			this.tutorTag = "ZoneAdd_Growing";
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002D0D RID: 11533 RVA: 0x0017AA44 File Offset: 0x00178E44
		protected override string NewZoneLabel
		{
			get
			{
				return "GrowingZone".Translate();
			}
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x0017AA64 File Offset: 0x00178E64
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

		// Token: 0x06002D0F RID: 11535 RVA: 0x0017AAD0 File Offset: 0x00178ED0
		protected override Zone MakeNewZone()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
			return new Zone_Growing(Find.CurrentMap.zoneManager);
		}
	}
}
