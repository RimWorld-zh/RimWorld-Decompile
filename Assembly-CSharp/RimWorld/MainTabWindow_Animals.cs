using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086E RID: 2158
	public class MainTabWindow_Animals : MainTabWindow_PawnTable
	{
		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x001AA068 File Offset: 0x001A8468
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Animals;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06003108 RID: 12552 RVA: 0x001AA084 File Offset: 0x001A8484
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.RaceProps.Animal
				select p;
			}
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x001AA0CF File Offset: 0x001A84CF
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
