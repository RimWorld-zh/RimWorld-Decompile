using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070E RID: 1806
	[StaticConstructorOnStartup]
	public class CompFireOverlay : ThingComp
	{
		// Token: 0x040015D9 RID: 5593
		protected CompRefuelable refuelableComp;

		// Token: 0x040015DA RID: 5594
		public static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060027AF RID: 10159 RVA: 0x001542DC File Offset: 0x001526DC
		public CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)this.props;
			}
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x001542FC File Offset: 0x001526FC
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

		// Token: 0x060027B1 RID: 10161 RVA: 0x0015436A File Offset: 0x0015276A
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}
	}
}
