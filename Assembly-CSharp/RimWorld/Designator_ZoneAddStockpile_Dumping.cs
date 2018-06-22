using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E8 RID: 2024
	public class Designator_ZoneAddStockpile_Dumping : Designator_ZoneAddStockpile
	{
		// Token: 0x06002D04 RID: 11524 RVA: 0x0017ABBC File Offset: 0x00178FBC
		public Designator_ZoneAddStockpile_Dumping()
		{
			this.preset = StorageSettingsPreset.DumpingStockpile;
			this.defaultLabel = this.preset.PresetName();
			this.defaultDesc = "DesignatorZoneCreateStorageDumpingDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
			this.hotKey = KeyBindingDefOf.Misc3;
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x0017AC14 File Offset: 0x00179014
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.StorageTab, OpportunityType.GoodToKnow);
		}
	}
}
