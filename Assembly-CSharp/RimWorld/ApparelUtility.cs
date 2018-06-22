using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050E RID: 1294
	public static class ApparelUtility
	{
		// Token: 0x06001767 RID: 5991 RVA: 0x000CD44C File Offset: 0x000CB84C
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

		// Token: 0x06001768 RID: 5992 RVA: 0x000CD5A0 File Offset: 0x000CB9A0
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

		// Token: 0x06001769 RID: 5993 RVA: 0x000CD618 File Offset: 0x000CBA18
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

		// Token: 0x0200050F RID: 1295
		public struct LayerGroupPair
		{
			// Token: 0x0600176A RID: 5994 RVA: 0x000CD715 File Offset: 0x000CBB15
			public LayerGroupPair(ApparelLayerDef layer, BodyPartGroupDef group)
			{
				this.layer = layer;
				this.group = group;
			}

			// Token: 0x0600176B RID: 5995 RVA: 0x000CD728 File Offset: 0x000CBB28
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

			// Token: 0x0600176C RID: 5996 RVA: 0x000CD77C File Offset: 0x000CBB7C
			public override int GetHashCode()
			{
				int num = 17;
				num = num * 23 + this.layer.GetHashCode();
				return num * 23 + this.group.GetHashCode();
			}

			// Token: 0x04000DD7 RID: 3543
			private readonly ApparelLayerDef layer;

			// Token: 0x04000DD8 RID: 3544
			private readonly BodyPartGroupDef group;
		}
	}
}
