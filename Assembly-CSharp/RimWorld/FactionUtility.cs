using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200055F RID: 1375
	public static class FactionUtility
	{
		// Token: 0x060019FB RID: 6651 RVA: 0x000E1924 File Offset: 0x000DFD24
		public static bool HostileTo(this Faction fac, Faction other)
		{
			return fac != null && other != null && other != fac && fac.RelationWith(other, false).kind == FactionRelationKind.Hostile;
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x000E1964 File Offset: 0x000DFD64
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

		// Token: 0x060019FD RID: 6653 RVA: 0x000E19E4 File Offset: 0x000DFDE4
		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			return thing.Faction == null || pawn.Faction == null || thing.Faction == pawn.Faction || thing.Faction == pawn.HostFaction;
		}
	}
}
