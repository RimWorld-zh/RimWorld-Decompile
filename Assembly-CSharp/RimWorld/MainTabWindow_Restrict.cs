using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000874 RID: 2164
	public class MainTabWindow_Restrict : MainTabWindow_PawnTable
	{
		// Token: 0x04001ABE RID: 6846
		private const int TimeAssignmentSelectorWidth = 191;

		// Token: 0x04001ABF RID: 6847
		private const int TimeAssignmentSelectorHeight = 65;

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06003168 RID: 12648 RVA: 0x001AD388 File Offset: 0x001AB788
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x001AD3A2 File Offset: 0x001AB7A2
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x001AD3BC File Offset: 0x001AB7BC
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			Rect rect = new Rect(0f, 0f, 191f, 65f);
			TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(rect);
		}
	}
}
