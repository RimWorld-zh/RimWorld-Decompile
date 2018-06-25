using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000876 RID: 2166
	public class MainTabWindow_Restrict : MainTabWindow_PawnTable
	{
		// Token: 0x04001AC2 RID: 6850
		private const int TimeAssignmentSelectorWidth = 191;

		// Token: 0x04001AC3 RID: 6851
		private const int TimeAssignmentSelectorHeight = 65;

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600316B RID: 12651 RVA: 0x001AD740 File Offset: 0x001ABB40
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x001AD75A File Offset: 0x001ABB5A
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x001AD774 File Offset: 0x001ABB74
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			Rect rect = new Rect(0f, 0f, 191f, 65f);
			TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(rect);
		}
	}
}
