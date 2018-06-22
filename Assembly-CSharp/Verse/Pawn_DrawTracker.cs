using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE7 RID: 3303
	public class Pawn_DrawTracker
	{
		// Token: 0x060048CB RID: 18635 RVA: 0x00263764 File Offset: 0x00261B64
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

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060048CC RID: 18636 RVA: 0x002637D4 File Offset: 0x00261BD4
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

		// Token: 0x060048CD RID: 18637 RVA: 0x0026383C File Offset: 0x00261C3C
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

		// Token: 0x060048CE RID: 18638 RVA: 0x002638D2 File Offset: 0x00261CD2
		public void DrawAt(Vector3 loc)
		{
			this.renderer.RenderPawnAt(loc);
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x002638E1 File Offset: 0x00261CE1
		public void Notify_Spawned()
		{
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x002638EF File Offset: 0x00261CEF
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.leaner.Notify_WarmingCastAlongLine(newShootLine, ShootPosition);
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x002638FF File Offset: 0x00261CFF
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (!this.pawn.Destroyed)
			{
				this.jitterer.Notify_DamageApplied(dinfo);
				this.renderer.Notify_DamageApplied(dinfo);
			}
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x0026392F File Offset: 0x00261D2F
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (!this.pawn.Destroyed)
			{
				this.jitterer.Notify_DamageDeflected(dinfo);
			}
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x00263954 File Offset: 0x00261D54
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

		// Token: 0x060048D4 RID: 18644 RVA: 0x002639F8 File Offset: 0x00261DF8
		public void Notify_DebugAffected()
		{
			for (int i = 0; i < 10; i++)
			{
				MoteMaker.ThrowAirPuffUp(this.pawn.DrawPos, this.pawn.Map);
			}
			this.jitterer.AddOffset(0.05f, (float)Rand.Range(0, 360));
		}

		// Token: 0x04003148 RID: 12616
		private Pawn pawn;

		// Token: 0x04003149 RID: 12617
		public PawnTweener tweener;

		// Token: 0x0400314A RID: 12618
		private JitterHandler jitterer;

		// Token: 0x0400314B RID: 12619
		public PawnLeaner leaner;

		// Token: 0x0400314C RID: 12620
		public PawnRenderer renderer;

		// Token: 0x0400314D RID: 12621
		public PawnUIOverlay ui;

		// Token: 0x0400314E RID: 12622
		private PawnFootprintMaker footprintMaker;

		// Token: 0x0400314F RID: 12623
		private PawnBreathMoteMaker breathMoteMaker;

		// Token: 0x04003150 RID: 12624
		private const float MeleeJitterDistance = 0.5f;
	}
}
