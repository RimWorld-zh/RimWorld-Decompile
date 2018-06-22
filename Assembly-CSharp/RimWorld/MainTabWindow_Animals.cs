using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086A RID: 2154
	public class MainTabWindow_Animals : MainTabWindow_PawnTable
	{
		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06003100 RID: 12544 RVA: 0x001AA250 File Offset: 0x001A8650
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Animals;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x001AA26C File Offset: 0x001A866C
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.RaceProps.Animal
				select p;
			}
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x001AA2B7 File Offset: 0x001A86B7
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
