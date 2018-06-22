using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E7 RID: 2023
	public class Designator_ZoneAddStockpile_Resources : Designator_ZoneAddStockpile
	{
		// Token: 0x06002D02 RID: 11522 RVA: 0x0017AB44 File Offset: 0x00178F44
		public Designator_ZoneAddStockpile_Resources()
		{
			this.preset = StorageSettingsPreset.DefaultStockpile;
			this.defaultLabel = this.preset.PresetName();
			this.defaultDesc = "DesignatorZoneCreateStorageResourcesDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
			this.hotKey = KeyBindingDefOf.Misc1;
			this.tutorTag = "ZoneAddStockpile_Resources";
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x0017ABA7 File Offset: 0x00178FA7
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.StorageTab, OpportunityType.GoodToKnow);
		}
	}
}
