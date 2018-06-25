using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000877 RID: 2167
	public class MainTabWindow_Wildlife : MainTabWindow_PawnTable
	{
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06003170 RID: 12656 RVA: 0x001AD54C File Offset: 0x001AB94C
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Wildlife;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x001AD568 File Offset: 0x001AB968
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.AllPawns
				where p.Spawned && p.Faction == null && p.AnimalOrWildMan() && !p.Position.Fogged(p.Map)
				select p;
			}
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x001AD5AE File Offset: 0x001AB9AE
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
