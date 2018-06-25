using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007EA RID: 2026
	public class Designator_ZoneAddStockpile_Dumping : Designator_ZoneAddStockpile
	{
		// Token: 0x06002D07 RID: 11527 RVA: 0x0017AF70 File Offset: 0x00179370
		public Designator_ZoneAddStockpile_Dumping()
		{
			this.preset = StorageSettingsPreset.DumpingStockpile;
			this.defaultLabel = this.preset.PresetName();
			this.defaultDesc = "DesignatorZoneCreateStorageDumpingDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
			this.hotKey = KeyBindingDefOf.Misc3;
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x0017AFC8 File Offset: 0x001793C8
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.StorageTab, OpportunityType.GoodToKnow);
		}
	}
}
