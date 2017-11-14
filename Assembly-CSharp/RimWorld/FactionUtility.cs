using System.Linq;
using Verse;

namespace RimWorld
{
	public static class FactionUtility
	{
		public static bool HostileTo(this Faction fac, Faction other)
		{
			if (fac != null && other != null && other != fac)
			{
				return fac.RelationWith(other, false).hostile;
			}
			return false;
		}

		public static Faction DefaultFactionFrom(FactionDef ft)
		{
			if (ft == null)
			{
				return null;
			}
			if (ft.isPlayer)
			{
				return Faction.OfPlayer;
			}
			Faction result = default(Faction);
			if ((from fac in Find.FactionManager.AllFactions
			where fac.def == ft
			select fac).TryRandomElement<Faction>(out result))
			{
				return result;
			}
			return null;
		}

		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			if (thing.Faction == null)
			{
				return true;
			}
			if (pawn.Faction == null)
			{
				return true;
			}
			if (thing.Faction == pawn.Faction)
			{
				return true;
			}
			if (thing.Faction == pawn.HostFaction)
			{
				return true;
			}
			return false;
		}
	}
}
