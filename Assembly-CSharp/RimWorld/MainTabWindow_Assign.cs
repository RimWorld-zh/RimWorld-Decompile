using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086C RID: 2156
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06003110 RID: 12560 RVA: 0x001AA628 File Offset: 0x001A8A28
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Assign;
			}
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x001AA642 File Offset: 0x001A8A42
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
