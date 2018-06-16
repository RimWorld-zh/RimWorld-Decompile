using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200023A RID: 570
	public class ApparelProperties
	{
		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0005D564 File Offset: 0x0005B964
		public ApparelLayerDef LastLayer
		{
			get
			{
				ApparelLayerDef result;
				if (this.layers.Count > 0)
				{
					result = this.layers[this.layers.Count - 1];
				}
				else
				{
					Log.ErrorOnce("Failed to get last layer on apparel item (see your config errors)", 31234937, false);
					result = ApparelLayerDefOf.Belt;
				}
				return result;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0005D5C0 File Offset: 0x0005B9C0
		public float HumanBodyCoverage
		{
			get
			{
				if (this.cachedHumanBodyCoverage < 0f)
				{
					this.cachedHumanBodyCoverage = 0f;
					List<BodyPartRecord> allParts = BodyDefOf.Human.AllParts;
					for (int i = 0; i < allParts.Count; i++)
					{
						if (this.CoversBodyPart(allParts[i]))
						{
							this.cachedHumanBodyCoverage += allParts[i].coverageAbs;
						}
					}
				}
				return this.cachedHumanBodyCoverage;
			}
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0005D648 File Offset: 0x0005BA48
		public static void ResetStaticData()
		{
			ApparelProperties.apparelRelevantGroups = (from td in DefDatabase<ThingDef>.AllDefs
			where td.IsApparel
			select td).SelectMany((ThingDef td) => td.apparel.bodyPartGroups).Distinct<BodyPartGroupDef>().ToArray<BodyPartGroupDef>();
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0005D6B0 File Offset: 0x0005BAB0
		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.layers.NullOrEmpty<ApparelLayerDef>())
			{
				yield return parentDef.defName + " apparel has no layers.";
			}
			yield break;
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0005D6E4 File Offset: 0x0005BAE4
		public bool CoversBodyPart(BodyPartRecord partRec)
		{
			for (int i = 0; i < partRec.groups.Count; i++)
			{
				if (this.bodyPartGroups.Contains(partRec.groups[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0005D73C File Offset: 0x0005BB3C
		public string GetCoveredOuterPartsString(BodyDef body)
		{
			IEnumerable<BodyPartRecord> source = from x in body.AllParts
			where x.depth == BodyPartDepth.Outside && x.groups.Any((BodyPartGroupDef y) => this.bodyPartGroups.Contains(y))
			select x;
			return (from part in source.Distinct<BodyPartRecord>()
			select part.Label).ToCommaList(true).CapitalizeFirst();
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0005D79C File Offset: 0x0005BB9C
		public string GetLayersString()
		{
			return (from layer in this.layers
			select layer.label).ToCommaList(true).CapitalizeFirst();
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0005D7E4 File Offset: 0x0005BBE4
		public BodyPartGroupDef[] GetInterferingBodyPartGroups(BodyDef body)
		{
			if (this.interferingBodyPartGroups == null || this.interferingBodyPartGroups.Length != DefDatabase<BodyDef>.DefCount)
			{
				this.interferingBodyPartGroups = new BodyPartGroupDef[DefDatabase<BodyDef>.DefCount][];
			}
			if (this.interferingBodyPartGroups[(int)body.index] == null)
			{
				BodyPartRecord[] source = (from part in body.AllParts
				where part.groups.Any((BodyPartGroupDef @group) => this.bodyPartGroups.Contains(@group))
				select part).ToArray<BodyPartRecord>();
				BodyPartGroupDef[] array = (from bpgd in source.SelectMany((BodyPartRecord bpr) => bpr.groups).Distinct<BodyPartGroupDef>()
				where ApparelProperties.apparelRelevantGroups.Contains(bpgd)
				select bpgd).ToArray<BodyPartGroupDef>();
				this.interferingBodyPartGroups[(int)body.index] = array;
			}
			return this.interferingBodyPartGroups[(int)body.index];
		}

		// Token: 0x040003F6 RID: 1014
		public List<BodyPartGroupDef> bodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x040003F7 RID: 1015
		public List<ApparelLayerDef> layers = new List<ApparelLayerDef>();

		// Token: 0x040003F8 RID: 1016
		[NoTranslate]
		public string wornGraphicPath = "";

		// Token: 0x040003F9 RID: 1017
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x040003FA RID: 1018
		[NoTranslate]
		public List<string> defaultOutfitTags = null;

		// Token: 0x040003FB RID: 1019
		public float wearPerDay = 0.4f;

		// Token: 0x040003FC RID: 1020
		public bool careIfWornByCorpse = true;

		// Token: 0x040003FD RID: 1021
		public bool hatRenderedFrontOfFace = false;

		// Token: 0x040003FE RID: 1022
		public bool useDeflectMetalEffect;

		// Token: 0x040003FF RID: 1023
		[Unsaved]
		private float cachedHumanBodyCoverage = -1f;

		// Token: 0x04000400 RID: 1024
		[Unsaved]
		private BodyPartGroupDef[][] interferingBodyPartGroups = null;

		// Token: 0x04000401 RID: 1025
		private static BodyPartGroupDef[] apparelRelevantGroups;
	}
}
