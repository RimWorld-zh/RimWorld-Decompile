using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class ApparelUtility
	{
		public struct LayerGroupPair
		{
			private readonly ApparelLayer layer;

			private readonly BodyPartGroupDef group;

			public LayerGroupPair(ApparelLayer layer, BodyPartGroupDef group)
			{
				this.layer = layer;
				this.group = group;
			}

			public override bool Equals(object rhs)
			{
				bool result;
				if (!(rhs is LayerGroupPair))
				{
					result = false;
				}
				else
				{
					LayerGroupPair layerGroupPair = (LayerGroupPair)rhs;
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

		public static bool CanWearTogether(ThingDef A, ThingDef B, BodyDef body)
		{
			bool flag = false;
			int num = 0;
			while (num < A.apparel.layers.Count)
			{
				int num2 = 0;
				while (num2 < B.apparel.layers.Count)
				{
					if (A.apparel.layers[num] == B.apparel.layers[num2])
					{
						flag = true;
					}
					if (!flag)
					{
						num2++;
						continue;
					}
					break;
				}
				if (!flag)
				{
					num++;
					continue;
				}
				break;
			}
			bool result;
			if (!flag)
			{
				result = true;
			}
			else
			{
				BodyPartGroupDef[] interferingBodyPartGroups = A.apparel.GetInterferingBodyPartGroups(body);
				BodyPartGroupDef[] interferingBodyPartGroups2 = B.apparel.GetInterferingBodyPartGroups(body);
				for (int i = 0; i < interferingBodyPartGroups.Length; i++)
				{
					if (interferingBodyPartGroups2.Contains(interferingBodyPartGroups[i]))
						goto IL_00cb;
				}
				result = true;
			}
			goto IL_00eb;
			IL_00eb:
			return result;
			IL_00cb:
			result = false;
			goto IL_00eb;
		}

		public static void GenerateLayerGroupPairs(BodyDef body, ThingDef td, Action<LayerGroupPair> callback)
		{
			for (int i = 0; i < td.apparel.layers.Count; i++)
			{
				ApparelLayer layer = td.apparel.layers[i];
				BodyPartGroupDef[] interferingBodyPartGroups = td.apparel.GetInterferingBodyPartGroups(body);
				for (int j = 0; j < interferingBodyPartGroups.Length; j++)
				{
					callback(new LayerGroupPair(layer, interferingBodyPartGroups[j]));
				}
			}
		}

		public static bool HasPartsToWear(Pawn p, ThingDef apparel)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			bool flag = false;
			int num = 0;
			while (num < hediffs.Count)
			{
				if (!(hediffs[num] is Hediff_MissingPart))
				{
					num++;
					continue;
				}
				flag = true;
				break;
			}
			bool result;
			if (!flag)
			{
				result = true;
			}
			else
			{
				IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined);
				List<BodyPartGroupDef> groups = apparel.apparel.bodyPartGroups;
				int i;
				for (i = 0; i < groups.Count; i++)
				{
					if (notMissingParts.Any((Func<BodyPartRecord, bool>)((BodyPartRecord x) => x.IsInGroup(groups[i]))))
						goto IL_00b5;
				}
				result = false;
			}
			goto IL_00ed;
			IL_00b5:
			result = true;
			goto IL_00ed;
			IL_00ed:
			return result;
		}
	}
}
