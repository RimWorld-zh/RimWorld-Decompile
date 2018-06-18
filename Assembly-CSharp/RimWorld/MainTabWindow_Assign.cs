using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000870 RID: 2160
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06003117 RID: 12567 RVA: 0x001AA440 File Offset: 0x001A8840
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Assign;
			}
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x001AA45A File Offset: 0x001A885A
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
