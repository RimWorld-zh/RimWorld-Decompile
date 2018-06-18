using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000879 RID: 2169
	public class MainTabWindow_Wildlife : MainTabWindow_PawnTable
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x001AD214 File Offset: 0x001AB614
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Wildlife;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06003174 RID: 12660 RVA: 0x001AD230 File Offset: 0x001AB630
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.AllPawns
				where p.Spawned && p.Faction == null && p.RaceProps.Animal && !p.Position.Fogged(p.Map)
				select p;
			}
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x001AD276 File Offset: 0x001AB676
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
