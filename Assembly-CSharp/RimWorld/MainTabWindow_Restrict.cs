using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000878 RID: 2168
	public class MainTabWindow_Restrict : MainTabWindow_PawnTable
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x0600316D RID: 12653 RVA: 0x001AD0D8 File Offset: 0x001AB4D8
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x001AD0F2 File Offset: 0x001AB4F2
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x001AD10C File Offset: 0x001AB50C
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			Rect rect = new Rect(0f, 0f, 191f, 65f);
			TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(rect);
		}

		// Token: 0x04001AC0 RID: 6848
		private const int TimeAssignmentSelectorWidth = 191;

		// Token: 0x04001AC1 RID: 6849
		private const int TimeAssignmentSelectorHeight = 65;
	}
}
