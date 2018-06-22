using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000318 RID: 792
	public static class IncidentParmsUtility
	{
		// Token: 0x06000D6D RID: 3437 RVA: 0x000736E0 File Offset: 0x00071AE0
		public static PawnGroupMakerParms GetDefaultPawnGroupMakerParms(PawnGroupKindDef groupKind, IncidentParms parms, bool ensureCanGenerateAtLeastOnePawn = false)
		{
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.groupKind = groupKind;
			pawnGroupMakerParms.tile = parms.target.Tile;
			pawnGroupMakerParms.points = parms.points;
			pawnGroupMakerParms.faction = parms.faction;
			pawnGroupMakerParms.traderKind = parms.traderKind;
			pawnGroupMakerParms.generateFightersOnly = parms.generateFightersOnly;
			pawnGroupMakerParms.raidStrategy = parms.raidStrategy;
			pawnGroupMakerParms.forceOneIncap = parms.raidForceOneIncap;
			if (ensureCanGenerateAtLeastOnePawn && parms.faction != null)
			{
				pawnGroupMakerParms.points = Mathf.Max(pawnGroupMakerParms.points, parms.faction.def.MinPointsToGeneratePawnGroup(groupKind));
			}
			return pawnGroupMakerParms;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00073790 File Offset: 0x00071B90
		public static List<List<Pawn>> SplitIntoGroups(List<Pawn> pawns, Dictionary<Pawn, int> groups)
		{
			List<List<Pawn>> list = new List<List<Pawn>>();
			List<Pawn> list2 = pawns.ToList<Pawn>();
			while (list2.Any<Pawn>())
			{
				List<Pawn> list3 = new List<Pawn>();
				Pawn pawn = list2.Last<Pawn>();
				list2.RemoveLast<Pawn>();
				list3.Add(pawn);
				for (int i = list2.Count - 1; i >= 0; i--)
				{
					if (IncidentParmsUtility.GetGroup(pawn, groups) == IncidentParmsUtility.GetGroup(list2[i], groups))
					{
						list3.Add(list2[i]);
						list2.RemoveAt(i);
					}
				}
				list.Add(list3);
			}
			return list;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0007383C File Offset: 0x00071C3C
		private static int GetGroup(Pawn pawn, Dictionary<Pawn, int> groups)
		{
			int num;
			int result;
			if (groups == null || !groups.TryGetValue(pawn, out num))
			{
				result = -1;
			}
			else
			{
				result = num;
			}
			return result;
		}
	}
}
