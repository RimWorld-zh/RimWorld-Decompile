using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class MainTabWindow_Animals : MainTabWindow_PawnTable
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		public MainTabWindow_Animals()
		{
		}

		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Animals;
			}
		}

		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.RaceProps.Animal
				select p;
			}
		}

		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		[CompilerGenerated]
		private static bool <get_Pawns>m__0(Pawn p)
		{
			return p.RaceProps.Animal;
		}
	}
}
