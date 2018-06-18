using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F30 RID: 3888
	public static class GenClamor
	{
		// Token: 0x06005D80 RID: 23936 RVA: 0x002F7010 File Offset: 0x002F5410
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
	}
}
