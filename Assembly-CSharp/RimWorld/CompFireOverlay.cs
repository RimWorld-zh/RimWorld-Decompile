using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000710 RID: 1808
	[StaticConstructorOnStartup]
	public class CompFireOverlay : ThingComp
	{
		// Token: 0x040015DD RID: 5597
		protected CompRefuelable refuelableComp;

		// Token: 0x040015DE RID: 5598
		public static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060027B2 RID: 10162 RVA: 0x0015468C File Offset: 0x00152A8C
		public CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)this.props;
			}
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x001546AC File Offset: 0x00152AAC
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.refuelableComp == null || this.refuelableComp.HasFuel)
			{
				Vector3 drawPos = this.parent.DrawPos;
				drawPos.y += 0.046875f;
				CompFireOverlay.FireGraphic.Draw(drawPos, Rot4.North, this.parent, 0f);
			}
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x0015471A File Offset: 0x00152B1A
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}
	}
}
