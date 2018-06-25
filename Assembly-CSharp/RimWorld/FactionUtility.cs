using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000561 RID: 1377
	public static class FactionUtility
	{
		// Token: 0x060019FF RID: 6655 RVA: 0x000E1A74 File Offset: 0x000DFE74
		public static bool HostileTo(this Faction fac, Faction other)
		{
			return fac != null && other != null && other != fac && fac.RelationWith(other, false).kind == FactionRelationKind.Hostile;
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x000E1AB4 File Offset: 0x000DFEB4
		public static Faction DefaultFactionFrom(FactionDef ft)
		{
			Faction result;
			Faction faction;
			if (ft == null)
			{
				result = null;
			}
			else if (ft.isPlayer)
			{
				result = Faction.OfPlayer;
			}
			else if ((from fac in Find.FactionManager.AllFactions
			where fac.def == ft
			select fac).TryRandomElement(out faction))
			{
				result = faction;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x000E1B34 File Offset: 0x000DFF34
		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			return thing.Faction == null || pawn.Faction == null || thing.Faction == pawn.Faction || thing.Faction == pawn.HostFaction;
		}
	}
}
