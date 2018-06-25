using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086C RID: 2156
	public class MainTabWindow_Animals : MainTabWindow_PawnTable
	{
		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06003104 RID: 12548 RVA: 0x001AA3A0 File Offset: 0x001A87A0
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Animals;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x001AA3BC File Offset: 0x001A87BC
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.RaceProps.Animal
				select p;
			}
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x001AA407 File Offset: 0x001A8807
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
