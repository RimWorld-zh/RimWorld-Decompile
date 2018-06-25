using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class MainTabWindow_Wildlife : MainTabWindow_PawnTable
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		public MainTabWindow_Wildlife()
		{
		}

		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Wildlife;
			}
		}

		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.AllPawns
				where p.Spawned && p.Faction == null && p.AnimalOrWildMan() && !p.Position.Fogged(p.Map)
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
			return p.Spawned && p.Faction == null && p.AnimalOrWildMan() && !p.Position.Fogged(p.Map);
		}
	}
}
