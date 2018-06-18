using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CEA RID: 3306
	public class Pawn_DrawTracker
	{
		// Token: 0x060048BA RID: 18618 RVA: 0x0026234C File Offset: 0x0026074C
		public Pawn_DrawTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.tweener = new PawnTweener(pawn);
			this.jitterer = new JitterHandler();
			this.leaner = new PawnLeaner(pawn);
			this.renderer = new PawnRenderer(pawn);
			this.ui = new PawnUIOverlay(pawn);
			this.footprintMaker = new PawnFootprintMaker(pawn);
			this.breathMoteMaker = new PawnBreathMoteMaker(pawn);
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x060048BB RID: 18619 RVA: 0x002623BC File Offset: 0x002607BC
		public Vector3 DrawPos
		{
			get
			{
				this.tweener.PreDrawPosCalculation();
				Vector3 vector = this.tweener.TweenedPos;
				vector += this.jitterer.CurrentOffset;
				vector += this.leaner.LeanOffset;
				vector.y = this.pawn.def.Altitude;
				return vector;
			}
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x00262424 File Offset: 0x00260824
		public void DrawTrackerTick()
		{
			if (this.pawn.Spawned)
			{
				if (Current.ProgramState != ProgramState.Playing || Find.CameraDriver.CurrentViewRect.ExpandedBy(3).Contains(this.pawn.Position))
				{
					this.jitterer.JitterHandlerTick();
					this.footprintMaker.FootprintMakerTick();
					this.breathMoteMaker.BreathMoteMakerTick();
					this.leaner.LeanerTick();
					this.renderer.RendererTick();
				}
			}
		}

		// Token: 0x060048BD RID: 18621 RVA: 0x002624BA File Offset: 0x002608BA
		public void DrawAt(Vector3 loc)
		{
			this.renderer.RenderPawnAt(loc);
		}

		// Token: 0x060048BE RID: 18622 RVA: 0x002624C9 File Offset: 0x002608C9
		public void Notify_Spawned()
		{
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x060048BF RID: 18623 RVA: 0x002624D7 File Offset: 0x002608D7
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.leaner.Notify_WarmingCastAlongLine(newShootLine, ShootPosition);
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x002624E7 File Offset: 0x002608E7
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (!this.pawn.Destroyed)
			{
				this.jitterer.Notify_DamageApplied(dinfo);
				this.renderer.Notify_DamageApplied(dinfo);
			}
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x00262517 File Offset: 0x00260917
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (!this.pawn.Destroyed)
			{
				this.jitterer.Notify_DamageDeflected(dinfo);
			}
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x0026253C File Offset: 0x0026093C
		public void Notify_MeleeAttackOn(Thing Target)
		{
			if (Target.Position != this.pawn.Position)
			{
				this.jitterer.AddOffset(0.5f, (Target.Position - this.pawn.Position).AngleFlat);
			}
			else if (Target.DrawPos != this.pawn.DrawPos)
			{
				this.jitterer.AddOffset(0.25f, (Target.DrawPos - this.pawn.DrawPos).AngleFlat());
			}
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x002625E0 File Offset: 0x002609E0
		public void Notify_DebugAffected()
		{
			for (int i = 0; i < 10; i++)
			{
				MoteMaker.ThrowAirPuffUp(this.pawn.DrawPos, this.pawn.Map);
			}
			this.jitterer.AddOffset(0.05f, (float)Rand.Range(0, 360));
		}

		// Token: 0x0400313D RID: 12605
		private Pawn pawn;

		// Token: 0x0400313E RID: 12606
		public PawnTweener tweener;

		// Token: 0x0400313F RID: 12607
		private JitterHandler jitterer;

		// Token: 0x04003140 RID: 12608
		public PawnLeaner leaner;

		// Token: 0x04003141 RID: 12609
		public PawnRenderer renderer;

		// Token: 0x04003142 RID: 12610
		public PawnUIOverlay ui;

		// Token: 0x04003143 RID: 12611
		private PawnFootprintMaker footprintMaker;

		// Token: 0x04003144 RID: 12612
		private PawnBreathMoteMaker breathMoteMaker;

		// Token: 0x04003145 RID: 12613
		private const float MeleeJitterDistance = 0.5f;
	}
}
