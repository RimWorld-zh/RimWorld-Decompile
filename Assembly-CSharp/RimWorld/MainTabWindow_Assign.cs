using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000870 RID: 2160
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06003115 RID: 12565 RVA: 0x001AA378 File Offset: 0x001A8778
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Assign;
			}
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x001AA392 File Offset: 0x001A8792
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
