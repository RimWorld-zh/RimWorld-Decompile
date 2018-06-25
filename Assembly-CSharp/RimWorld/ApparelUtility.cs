using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class ApparelUtility
	{
		public static bool CanWearTogether(ThingDef A, ThingDef B, BodyDef body)
		{
			bool flag = false;
			for (int i = 0; i < A.apparel.layers.Count; i++)
			{
				for (int j = 0; j < B.apparel.layers.Count; j++)
				{
					if (A.apparel.layers[i] == B.apparel.layers[j])
					{
						flag = true;
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			bool result;
			if (!flag)
			{
				result = true;
			}
			else
			{
				List<BodyPartGroupDef> bodyPartGroups = A.apparel.bodyPartGroups;
				List<BodyPartGroupDef> bodyPartGroups2 = B.apparel.bodyPartGroups;
				BodyPartGroupDef[] interferingBodyPartGroups = A.apparel.GetInterferingBodyPartGroups(body);
				BodyPartGroupDef[] interferingBodyPartGroups2 = B.apparel.GetInterferingBodyPartGroups(body);
				for (int k = 0; k < bodyPartGroups.Count; k++)
				{
					if (interferingBodyPartGroups2.Contains(bodyPartGroups[k]))
					{
						return false;
					}
				}
				for (int l = 0; l < bodyPartGroups2.Count; l++)
				{
					if (interferingBodyPartGroups.Contains(bodyPartGroups2[l]))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		public static void GenerateLayerGroupPairs(BodyDef body, ThingDef td, Action<ApparelUtility.LayerGroupPair> callback)
		{
			for (int i = 0; i < td.apparel.layers.Count; i++)
			{
				ApparelLayerDef layer = td.apparel.layers[i];
				BodyPartGroupDef[] interferingBodyPartGroups = td.apparel.GetInterferingBodyPartGroups(body);
				for (int j = 0; j < interferingBodyPartGroups.Length; j++)
				{
					callback(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j]));
				}
			}
		}

		public static bool HasPartsToWear(Pawn p, ThingDef apparel)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			bool flag = false;
			for (int j = 0; j < hediffs.Count; j++)
			{
				if (hediffs[j] is Hediff_MissingPart)
				{
					flag = true;
					break;
				}
			}
			bool result;
			if (!flag)
			{
				result = true;
			}
			else
			{
				IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null);
				List<BodyPartGroupDef> groups = apparel.apparel.bodyPartGroups;
				int i;
				for (i = 0; i < groups.Count; i++)
				{
					if (notMissingParts.Any((BodyPartRecord x) => x.IsInGroup(groups[i])))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		public struct LayerGroupPair
		{
			private readonly ApparelLayerDef layer;

			private readonly BodyPartGroupDef group;

			public LayerGroupPair(ApparelLayerDef layer, BodyPartGroupDef group)
			{
				this.layer = layer;
				this.group = group;
			}

			public override bool Equals(object rhs)
			{
				bool result;
				if (!(rhs is ApparelUtility.LayerGroupPair))
				{
					result = false;
				}
				else
				{
					ApparelUtility.LayerGroupPair layerGroupPair = (ApparelUtility.LayerGroupPair)rhs;
					result = (layerGroupPair.layer == this.layer && layerGroupPair.group == this.group);
				}
				return result;
			}

			public override int GetHashCode()
			{
				int num = 17;
				num = num * 23 + this.layer.GetHashCode();
				return num * 23 + this.group.GetHashCode();
			}
		}

		[CompilerGenerated]
		private sealed class <HasPartsToWear>c__AnonStorey0
		{
			internal List<BodyPartGroupDef> groups;

			public <HasPartsToWear>c__AnonStorey0()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <HasPartsToWear>c__AnonStorey1
		{
			internal int i;

			internal ApparelUtility.<HasPartsToWear>c__AnonStorey0 <>f__ref$0;

			public <HasPartsToWear>c__AnonStorey1()
			{
			}

			internal bool <>m__0(BodyPartRecord x)
			{
				return x.IsInGroup(this.<>f__ref$0.groups[this.i]);
			}
		}
	}
}
