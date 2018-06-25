using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000304 RID: 772
	public sealed class OutfitDatabase : IExposable
	{
		// Token: 0x04000856 RID: 2134
		private List<Outfit> outfits = new List<Outfit>();

		// Token: 0x06000CCD RID: 3277 RVA: 0x00070694 File Offset: 0x0006EA94
		public OutfitDatabase()
		{
			this.GenerateStartingOutfits();
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x000706B0 File Offset: 0x0006EAB0
		public List<Outfit> AllOutfits
		{
			get
			{
				return this.outfits;
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x000706CB File Offset: 0x0006EACB
		public void ExposeData()
		{
			Scribe_Collections.Look<Outfit>(ref this.outfits, "outfits", LookMode.Deep, new object[0]);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x000706E8 File Offset: 0x0006EAE8
		public Outfit DefaultOutfit()
		{
			if (this.outfits.Count == 0)
			{
				this.MakeNewOutfit();
			}
			return this.outfits[0];
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00070720 File Offset: 0x0006EB20
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

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0007083C File Offset: 0x0006EC3C
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

		// Token: 0x06000CD3 RID: 3283 RVA: 0x000708DC File Offset: 0x0006ECDC
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
	}
}
