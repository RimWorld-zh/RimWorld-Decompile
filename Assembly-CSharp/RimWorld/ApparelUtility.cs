using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000510 RID: 1296
	public static class ApparelUtility
	{
		// Token: 0x0600176B RID: 5995 RVA: 0x000CD59C File Offset: 0x000CB99C
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

		// Token: 0x0600176C RID: 5996 RVA: 0x000CD6F0 File Offset: 0x000CBAF0
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

		// Token: 0x0600176D RID: 5997 RVA: 0x000CD768 File Offset: 0x000CBB68
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

		// Token: 0x02000511 RID: 1297
		public struct LayerGroupPair
		{
			// Token: 0x04000DD7 RID: 3543
			private readonly ApparelLayerDef layer;

			// Token: 0x04000DD8 RID: 3544
			private readonly BodyPartGroupDef group;

			// Token: 0x0600176E RID: 5998 RVA: 0x000CD865 File Offset: 0x000CBC65
			public LayerGroupPair(ApparelLayerDef layer, BodyPartGroupDef group)
			{
				this.layer = layer;
				this.group = group;
			}

			// Token: 0x0600176F RID: 5999 RVA: 0x000CD878 File Offset: 0x000CBC78
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

			// Token: 0x06001770 RID: 6000 RVA: 0x000CD8CC File Offset: 0x000CBCCC
			public override int GetHashCode()
			{
				int num = 17;
				num = num * 23 + this.layer.GetHashCode();
				return num * 23 + this.group.GetHashCode();
			}
		}
	}
}
