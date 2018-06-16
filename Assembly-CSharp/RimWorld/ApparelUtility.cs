using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000512 RID: 1298
	public static class ApparelUtility
	{
		// Token: 0x0600176F RID: 5999 RVA: 0x000CD400 File Offset: 0x000CB800
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

		// Token: 0x06001770 RID: 6000 RVA: 0x000CD554 File Offset: 0x000CB954
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

		// Token: 0x06001771 RID: 6001 RVA: 0x000CD5CC File Offset: 0x000CB9CC
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

		// Token: 0x02000513 RID: 1299
		public struct LayerGroupPair
		{
			// Token: 0x06001772 RID: 6002 RVA: 0x000CD6C9 File Offset: 0x000CBAC9
			public LayerGroupPair(ApparelLayerDef layer, BodyPartGroupDef group)
			{
				this.layer = layer;
				this.group = group;
			}

			// Token: 0x06001773 RID: 6003 RVA: 0x000CD6DC File Offset: 0x000CBADC
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

			// Token: 0x06001774 RID: 6004 RVA: 0x000CD730 File Offset: 0x000CBB30
			public override int GetHashCode()
			{
				int num = 17;
				num = num * 23 + this.layer.GetHashCode();
				return num * 23 + this.group.GetHashCode();
			}

			// Token: 0x04000DDA RID: 3546
			private readonly ApparelLayerDef layer;

			// Token: 0x04000DDB RID: 3547
			private readonly BodyPartGroupDef group;
		}
	}
}
