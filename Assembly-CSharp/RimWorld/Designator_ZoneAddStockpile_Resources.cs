using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_ZoneAddStockpile_Resources : Designator_ZoneAddStockpile
	{
		public Designator_ZoneAddStockpile_Resources()
		{
			base.preset = StorageSettingsPreset.DefaultStockpile;
			base.defaultLabel = base.preset.PresetName();
			base.defaultDesc = "DesignatorZoneCreateStorageResourcesDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
			base.hotKey = KeyBindingDefOf.Misc1;
			base.tutorTag = "ZoneAddStockpile_Resources";
		}

		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.StorageTab, OpportunityType.GoodToKnow);
		}
	}
}
