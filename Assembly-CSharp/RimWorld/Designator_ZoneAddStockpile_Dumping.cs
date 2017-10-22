using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_ZoneAddStockpile_Dumping : Designator_ZoneAddStockpile
	{
		public Designator_ZoneAddStockpile_Dumping()
		{
			base.preset = StorageSettingsPreset.DumpingStockpile;
			base.defaultLabel = base.preset.PresetName();
			base.defaultDesc = "DesignatorZoneCreateStorageDumpingDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
			base.hotKey = KeyBindingDefOf.Misc3;
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.StorageTab, OpportunityType.GoodToKnow);
		}
	}
}
