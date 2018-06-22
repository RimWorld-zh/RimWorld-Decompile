using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000875 RID: 2165
	public class MainTabWindow_Wildlife : MainTabWindow_PawnTable
	{
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x0600316C RID: 12652 RVA: 0x001AD3FC File Offset: 0x001AB7FC
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Wildlife;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x0600316D RID: 12653 RVA: 0x001AD418 File Offset: 0x001AB818
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.AllPawns
				where p.Spawned && p.Faction == null && p.RaceProps.Animal && !p.Position.Fogged(p.Map)
				select p;
			}
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x001AD45E File Offset: 0x001AB85E
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
