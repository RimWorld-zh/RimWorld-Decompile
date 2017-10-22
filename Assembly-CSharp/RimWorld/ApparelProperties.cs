using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class ApparelProperties
	{
		public List<BodyPartGroupDef> bodyPartGroups = new List<BodyPartGroupDef>();

		public List<ApparelLayer> layers = new List<ApparelLayer>();

		public string wornGraphicPath = "";

		public List<string> tags = new List<string>();

		public List<string> defaultOutfitTags = null;

		public float wearPerDay = 0.4f;

		public bool careIfWornByCorpse = true;

		public bool hatRenderedFrontOfFace = false;

		[Unsaved]
		private float cachedHumanBodyCoverage = -1f;

		[Unsaved]
		private BodyPartGroupDef[][] interferingBodyPartGroups = null;

		private static BodyPartGroupDef[] apparelRelevantGroups = null;

		public ApparelLayer LastLayer
		{
			get
			{
				ApparelLayer result;
				if (this.layers.Count > 0)
				{
					result = this.layers[this.layers.Count - 1];
				}
				else
				{
					Log.ErrorOnce("Failed to get last layer on apparel item (see your config errors)", 31234937);
					result = ApparelLayer.Belt;
				}
				return result;
			}
		}

		public float HumanBodyCoverage
		{
			get
			{
				if (this.cachedHumanBodyCoverage < 0.0)
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

		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (!this.layers.NullOrEmpty())
				yield break;
			yield return parentDef.defName + " apparel has no layers.";
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public bool CoversBodyPart(BodyPartRecord partRec)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < partRec.groups.Count)
				{
					if (this.bodyPartGroups.Contains(partRec.groups[num]))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public string GetCoveredOuterPartsString(BodyDef body)
		{
			IEnumerable<BodyPartRecord> source = from x in body.AllParts
			where x.depth == BodyPartDepth.Outside && x.groups.Any((Predicate<BodyPartGroupDef>)((BodyPartGroupDef y) => this.bodyPartGroups.Contains(y)))
			select x;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (BodyPartRecord item in source.Distinct())
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				flag = false;
				stringBuilder.Append(item.def.label);
			}
			return stringBuilder.ToString().CapitalizeFirst();
		}

		public BodyPartGroupDef[] GetInterferingBodyPartGroups(BodyDef body)
		{
			if (this.interferingBodyPartGroups == null)
			{
				this.interferingBodyPartGroups = new BodyPartGroupDef[DefDatabase<BodyPartGroupDef>.DefCount][];
			}
			if (this.interferingBodyPartGroups[body.index] == null)
			{
				if (ApparelProperties.apparelRelevantGroups == null)
				{
					ApparelProperties.apparelRelevantGroups = (from td in DefDatabase<ThingDef>.AllDefs
					where td.IsApparel
					select td).SelectMany((Func<ThingDef, IEnumerable<BodyPartGroupDef>>)((ThingDef td) => td.apparel.bodyPartGroups)).Distinct().ToArray();
				}
				BodyPartRecord[] source = (from part in body.AllParts
				where part.groups.Any((Predicate<BodyPartGroupDef>)((BodyPartGroupDef @group) => this.bodyPartGroups.Contains(@group)))
				select part).ToArray();
				IEnumerable<BodyPartGroupDef> source2 = source.SelectMany((Func<BodyPartRecord, IEnumerable<BodyPartGroupDef>>)((BodyPartRecord bpr) => bpr.groups)).Distinct();
				BodyPartGroupDef[] array = this.interferingBodyPartGroups[body.index] = (from bpgd in source2
				where ApparelProperties.apparelRelevantGroups.Contains(bpgd)
				select bpgd).ToArray();
			}
			return this.interferingBodyPartGroups[body.index];
		}
	}
}
