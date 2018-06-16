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
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x001AD14C File Offset: 0x001AB54C
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Wildlife;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06003172 RID: 12658 RVA: 0x001AD168 File Offset: 0x001AB568
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.AllPawns
				where p.Spawned && p.Faction == null && p.RaceProps.Animal && !p.Position.Fogged(p.Map)
				select p;
			}
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x001AD1AE File Offset: 0x001AB5AE
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
