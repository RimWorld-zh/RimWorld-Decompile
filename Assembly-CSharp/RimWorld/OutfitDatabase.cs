using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public sealed class OutfitDatabase : IExposable
	{
		private List<Outfit> outfits = new List<Outfit>();

		[CompilerGenerated]
		private static Func<Outfit, int> <>f__am$cache0;

		public OutfitDatabase()
		{
			this.GenerateStartingOutfits();
		}

		public List<Outfit> AllOutfits
		{
			get
			{
				return this.outfits;
			}
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
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
			{
				if (pawn.outfits != null && pawn.outfits.CurrentOutfit == outfit)
				{
					return new AcceptanceReport("OutfitInUse".Translate(new object[]
					{
						pawn
					}));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.outfits != null && pawn2.outfits.CurrentOutfit == outfit)
				{
					pawn2.outfits.CurrentOutfit = null;
				}
			}
			this.outfits.Remove(outfit);
			return AcceptanceReport.WasAccepted;
		}

		public Outfit MakeNewOutfit()
		{
			int num;
			if (this.outfits.Any<Outfit>())
			{
				num = this.outfits.Max((Outfit o) => o.uniqueId) + 1;
			}
			else
			{
				num = 1;
			}
			int uniqueId = num;
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
			outfit2.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.apparel != null && thingDef.apparel.defaultOutfitTags != null && thingDef.apparel.defaultOutfitTags.Contains("Worker"))
				{
					outfit2.filter.SetAllow(thingDef, true);
				}
			}
			Outfit outfit3 = this.MakeNewOutfit();
			outfit3.label = "OutfitSoldier".Translate();
			outfit3.filter.SetDisallowAll(null, null);
			outfit3.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef2.apparel != null && thingDef2.apparel.defaultOutfitTags != null && thingDef2.apparel.defaultOutfitTags.Contains("Soldier"))
				{
					outfit3.filter.SetAllow(thingDef2, true);
				}
			}
			Outfit outfit4 = this.MakeNewOutfit();
			outfit4.label = "OutfitNudist".Translate();
			outfit4.filter.SetDisallowAll(null, null);
			outfit4.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef3 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef3.apparel != null && !thingDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs) && !thingDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
				{
					outfit4.filter.SetAllow(thingDef3, true);
				}
			}
		}

		[CompilerGenerated]
		private static int <MakeNewOutfit>m__0(Outfit o)
		{
			return o.uniqueId;
		}
	}
}
