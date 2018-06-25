using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086E RID: 2158
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06003113 RID: 12563 RVA: 0x001AA9E0 File Offset: 0x001A8DE0
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Assign;
			}
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x001AA9FA File Offset: 0x001A8DFA
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
