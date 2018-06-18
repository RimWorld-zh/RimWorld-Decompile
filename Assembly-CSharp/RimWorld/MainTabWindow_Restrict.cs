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
		// (get) Token: 0x0600316F RID: 12655 RVA: 0x001AD1A0 File Offset: 0x001AB5A0
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x001AD1BA File Offset: 0x001AB5BA
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x001AD1D4 File Offset: 0x001AB5D4
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
