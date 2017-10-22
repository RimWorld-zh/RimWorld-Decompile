using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class FactionUtility
	{
		public static bool HostileTo(this Faction fac, Faction other)
		{
			return fac != null && other != null && other != fac && fac.RelationWith(other, false).hostile;
		}

		public static Faction DefaultFactionFrom(FactionDef ft)
		{
			Faction faction = default(Faction);
			return (ft != null) ? ((!ft.isPlayer) ? ((!(from fac in Find.FactionManager.AllFactions
			where fac.def == ft
			select fac).TryRandomElement<Faction>(out faction)) ? null : faction) : Faction.OfPlayer) : null;
		}

		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			return (byte)((thing.Faction == null) ? 1 : ((pawn.Faction == null) ? 1 : ((thing.Faction == pawn.Faction) ? 1 : ((thing.Faction == pawn.HostFaction) ? 1 : 0)))) != 0;
		}
	}
}
