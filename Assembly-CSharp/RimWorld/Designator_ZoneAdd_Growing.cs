using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_ZoneAdd_Growing : Designator_ZoneAdd
	{
		protected override string NewZoneLabel
		{
			get
			{
				return "GrowingZone".Translate();
			}
		}

		public Designator_ZoneAdd_Growing()
		{
			base.zoneTypeToPlace = typeof(Zone_Growing);
			base.defaultLabel = "GrowingZone".Translate();
			base.defaultDesc = "DesignatorGrowingZoneDesc".Translate();
			base.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true);
			base.hotKey = KeyBindingDefOf.Misc2;
			base.tutorTag = "ZoneAdd_Growing";
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!base.CanDesignateCell(c).Accepted)
			{
				return false;
			}
			if (base.Map.fertilityGrid.FertilityAt(c) < ThingDefOf.PlantPotato.plant.fertilityMin)
			{
				return false;
			}
			return true;
		}

		protected override Zone MakeNewZone()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
			return new Zone_Growing(Find.VisibleMap.zoneManager);
		}
	}
}
