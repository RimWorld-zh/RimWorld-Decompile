using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000712 RID: 1810
	[StaticConstructorOnStartup]
	public class CompFireOverlay : ThingComp
	{
		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060027B7 RID: 10167 RVA: 0x00154134 File Offset: 0x00152534
		public CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)this.props;
			}
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x00154154 File Offset: 0x00152554
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

		// Token: 0x060027B9 RID: 10169 RVA: 0x001541C2 File Offset: 0x001525C2
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}

		// Token: 0x040015DB RID: 5595
		protected CompRefuelable refuelableComp;

		// Token: 0x040015DC RID: 5596
		public static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}
