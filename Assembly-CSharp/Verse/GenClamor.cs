using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public static class GenClamor
	{
		[CompilerGenerated]
		private static RegionEntryPredicate <>f__am$cache0;

		public static void DoClamor(Thing source, float radius, ClamorDef type)
		{
			IntVec3 root = source.Position;
			Region region = source.GetRegion(RegionType.Set_Passable);
			if (region != null)
			{
				RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.portal == null || r.portal.Open, delegate(Region r)
				{
					List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
					for (int i = 0; i < list.Count; i++)
					{
						Pawn pawn = list[i] as Pawn;
						float num = Mathf.Clamp01(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Hearing));
						if (num > 0f && pawn.Position.InHorDistOf(root, radius * num))
						{
							pawn.HearClamor(source, type);
						}
					}
					return false;
				}, 15, RegionType.Set_Passable);
			}
		}

		[CompilerGenerated]
		private static bool <DoClamor>m__0(Region from, Region r)
		{
			return r.portal == null || r.portal.Open;
		}

		[CompilerGenerated]
		private sealed class <DoClamor>c__AnonStorey0
		{
			internal IntVec3 root;

			internal float radius;

			internal Thing source;

			internal ClamorDef type;

			public <DoClamor>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i] as Pawn;
					float num = Mathf.Clamp01(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Hearing));
					if (num > 0f && pawn.Position.InHorDistOf(this.root, this.radius * num))
					{
						pawn.HearClamor(this.source, this.type);
					}
				}
				return false;
			}
		}
	}
}
