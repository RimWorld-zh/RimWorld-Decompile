using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public sealed class OutfitDatabase : IExposable
	{
		private List<Outfit> outfits = new List<Outfit>();

		public List<Outfit> AllOutfits
		{
			get
			{
				return this.outfits;
			}
		}

		public OutfitDatabase()
		{
			this.GenerateStartingOutfits();
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Outfit>(ref this.outfits, "outfits", LookMode.Deep, new object[0]);
		}

		public Outfit DefaultOutfit()
		{
			if (this.outfits.Count == 0)
			{
				this.MakeNewOutfit();
			}
			return this.outfits[0];
		}

		public AcceptanceReport TryDelete(Outfit outfit)
		{
			foreach (Pawn allMapsCaravansAndTravelingTransportPod in PawnsFinder.AllMapsCaravansAndTravelingTransportPods)
			{
				if (allMapsCaravansAndTravelingTransportPod.outfits != null && allMapsCaravansAndTravelingTransportPod.outfits.CurrentOutfit == outfit)
				{
					return new AcceptanceReport("OutfitInUse".Translate(allMapsCaravansAndTravelingTransportPod));
				}
			}
			foreach (Pawn item in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (item.outfits != null && item.outfits.CurrentOutfit == outfit)
				{
					item.outfits.CurrentOutfit = null;
				}
			}
			this.outfits.Remove(outfit);
			return AcceptanceReport.WasAccepted;
		}

		public Outfit MakeNewOutfit()
		{
			int uniqueId = (!this.outfits.Any()) ? 1 : (this.outfits.Max((Outfit o) => o.uniqueId) + 1);
			Outfit outfit = new Outfit(uniqueId, "Outfit".Translate() + " " + uniqueId.ToString());
			outfit.filter.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
			this.outfits.Add(outfit);
			return outfit;
		}

		private void GenerateStartingOutfits()
		{
			Outfit outfit = this.MakeNewOutfit();
			outfit.label = "OutfitAnything".Translate();
			Outfit outfit2 = this.MakeNewOutfit();
			outfit2.label = "OutfitWorker".Translate();
			outfit2.filter.SetDisallowAll(null, null);
			outfit2.filter.SetAllow(SpecialThingFilterDefOf.AllowNonDeadmansApparel, true);
			foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (allDef.apparel != null && allDef.apparel.defaultOutfitTags != null && allDef.apparel.defaultOutfitTags.Contains("Worker"))
				{
					outfit2.filter.SetAllow(allDef, true);
				}
			}
			Outfit outfit3 = this.MakeNewOutfit();
			outfit3.label = "OutfitSoldier".Translate();
			outfit3.filter.SetDisallowAll(null, null);
			outfit3.filter.SetAllow(SpecialThingFilterDefOf.AllowNonDeadmansApparel, true);
			foreach (ThingDef allDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (allDef2.apparel != null && allDef2.apparel.defaultOutfitTags != null && allDef2.apparel.defaultOutfitTags.Contains("Soldier"))
				{
					outfit3.filter.SetAllow(allDef2, true);
				}
			}
			Outfit outfit4 = this.MakeNewOutfit();
			outfit4.label = "OutfitNudist".Translate();
			outfit4.filter.SetDisallowAll(null, null);
			outfit4.filter.SetAllow(SpecialThingFilterDefOf.AllowNonDeadmansApparel, true);
			foreach (ThingDef allDef3 in DefDatabase<ThingDef>.AllDefs)
			{
				if (allDef3.apparel != null && !allDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs) && !allDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
				{
					outfit4.filter.SetAllow(allDef3, true);
				}
			}
		}
	}
}
