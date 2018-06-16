using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000563 RID: 1379
	public static class FactionUtility
	{
		// Token: 0x06001A03 RID: 6659 RVA: 0x000E187C File Offset: 0x000DFC7C
		public static bool HostileTo(this Faction fac, Faction other)
		{
			return fac != null && other != null && other != fac && fac.RelationWith(other, false).kind == FactionRelationKind.Hostile;
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x000E18BC File Offset: 0x000DFCBC
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

		// Token: 0x06001A05 RID: 6661 RVA: 0x000E193C File Offset: 0x000DFD3C
		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			return thing.Faction == null || pawn.Faction == null || thing.Faction == pawn.Faction || thing.Faction == pawn.HostFaction;
		}
	}
}
