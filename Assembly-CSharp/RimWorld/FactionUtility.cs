using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class FactionUtility
	{
		public static bool HostileTo(this Faction fac, Faction other)
		{
			return fac != null && other != null && other != fac && fac.RelationWith(other, false).kind == FactionRelationKind.Hostile;
		}

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

		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			return thing.Faction == null || pawn.Faction == null || thing.Faction == pawn.Faction || thing.Faction == pawn.HostFaction;
		}

		[CompilerGenerated]
		private sealed class <DefaultFactionFrom>c__AnonStorey0
		{
			internal FactionDef ft;

			public <DefaultFactionFrom>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Faction fac)
			{
				return fac.def == this.ft;
			}
		}
	}
}
