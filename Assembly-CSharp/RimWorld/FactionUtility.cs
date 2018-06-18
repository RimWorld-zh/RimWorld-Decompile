using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000563 RID: 1379
	public static class FactionUtility
	{
		// Token: 0x06001A04 RID: 6660 RVA: 0x000E18D0 File Offset: 0x000DFCD0
		public static bool HostileTo(this Faction fac, Faction other)
		{
			return fac != null && other != null && other != fac && fac.RelationWith(other, false).kind == FactionRelationKind.Hostile;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x000E1910 File Offset: 0x000DFD10
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

		// Token: 0x06001A06 RID: 6662 RVA: 0x000E1990 File Offset: 0x000DFD90
		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			return thing.Faction == null || pawn.Faction == null || thing.Faction == pawn.Faction || thing.Faction == pawn.HostFaction;
		}
	}
}
