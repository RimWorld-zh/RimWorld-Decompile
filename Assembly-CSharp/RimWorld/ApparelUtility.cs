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
				if (!(rhs is LayerGroupPair))
				{
					return false;
				}
				LayerGroupPair layerGroupPair = (LayerGroupPair)rhs;
				return layerGroupPair.layer == this.layer && layerGroupPair.group == this.group;
			}

			public override int GetHashCode()
			{
				int num = 17;
				num = num * 23 + ((Enum)(object)this.layer).GetHashCode();
				return num * 23 + this.group.GetHashCode();
			}
		}

		public static bool CanWearTogether(ThingDef A, ThingDef B)
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
			if (!flag)
			{
				return true;
			}
			for (int i = 0; i < A.apparel.bodyPartGroups.Count; i++)
			{
				for (int j = 0; j < B.apparel.bodyPartGroups.Count; j++)
				{
					BodyPartGroupDef item = A.apparel.bodyPartGroups[i];
					BodyPartGroupDef item2 = B.apparel.bodyPartGroups[j];
					for (int k = 0; k < BodyDefOf.Human.AllParts.Count; k++)
					{
						BodyPartRecord bodyPartRecord = BodyDefOf.Human.AllParts[k];
						if (bodyPartRecord.groups.Contains(item) && bodyPartRecord.groups.Contains(item2))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		public static void GenerateLayerGroupPairs(ThingDef td, Action<LayerGroupPair> callback)
		{
			for (int i = 0; i < td.apparel.layers.Count; i++)
			{
				for (int j = 0; j < td.apparel.bodyPartGroups.Count; j++)
				{
					callback(new LayerGroupPair(td.apparel.layers[i], td.apparel.bodyPartGroups[j]));
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
			if (!flag)
			{
				return true;
			}
			IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined);
			List<BodyPartGroupDef> groups = apparel.apparel.bodyPartGroups;
			int i;
			for (i = 0; i < groups.Count; i++)
			{
				if (notMissingParts.Any((Func<BodyPartRecord, bool>)((BodyPartRecord x) => x.IsInGroup(groups[i]))))
				{
					return true;
				}
			}
			return false;
		}
	}
}
