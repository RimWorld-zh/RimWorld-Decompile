using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200031A RID: 794
	public static class IncidentParmsUtility
	{
		// Token: 0x06000D70 RID: 3440 RVA: 0x00073838 File Offset: 0x00071C38
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

		// Token: 0x06000D71 RID: 3441 RVA: 0x000738E8 File Offset: 0x00071CE8
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

		// Token: 0x06000D72 RID: 3442 RVA: 0x00073994 File Offset: 0x00071D94
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
