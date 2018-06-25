using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000876 RID: 2166
	public class MainTabWindow_Restrict : MainTabWindow_PawnTable
	{
		// Token: 0x04001ABE RID: 6846
		private const int TimeAssignmentSelectorWidth = 191;

		// Token: 0x04001ABF RID: 6847
		private const int TimeAssignmentSelectorHeight = 65;

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600316C RID: 12652 RVA: 0x001AD4D8 File Offset: 0x001AB8D8
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x001AD4F2 File Offset: 0x001AB8F2
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x001AD50C File Offset: 0x001AB90C
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			Rect rect = new Rect(0f, 0f, 191f, 65f);
			TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(rect);
		}
	}
}
