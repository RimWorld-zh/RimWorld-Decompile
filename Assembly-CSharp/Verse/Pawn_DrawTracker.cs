using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE9 RID: 3305
	public class Pawn_DrawTracker
	{
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

		// Token: 0x060048CE RID: 18638 RVA: 0x00263840 File Offset: 0x00261C40
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

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x060048CF RID: 18639 RVA: 0x002638B0 File Offset: 0x00261CB0
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

		// Token: 0x060048D0 RID: 18640 RVA: 0x00263918 File Offset: 0x00261D18
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

		// Token: 0x060048D1 RID: 18641 RVA: 0x002639AE File Offset: 0x00261DAE
		public void DrawAt(Vector3 loc)
		{
			this.renderer.RenderPawnAt(loc);
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x002639BD File Offset: 0x00261DBD
		public void Notify_Spawned()
		{
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x002639CB File Offset: 0x00261DCB
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.leaner.Notify_WarmingCastAlongLine(newShootLine, ShootPosition);
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x002639DB File Offset: 0x00261DDB
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (!this.pawn.Destroyed)
			{
				this.jitterer.Notify_DamageApplied(dinfo);
				this.renderer.Notify_DamageApplied(dinfo);
			}
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x00263A0B File Offset: 0x00261E0B
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (!this.pawn.Destroyed)
			{
				this.jitterer.Notify_DamageDeflected(dinfo);
			}
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x00263A30 File Offset: 0x00261E30
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

		// Token: 0x060048D7 RID: 18647 RVA: 0x00263AD4 File Offset: 0x00261ED4
		public void Notify_DebugAffected()
		{
			for (int i = 0; i < 10; i++)
			{
				MoteMaker.ThrowAirPuffUp(this.pawn.DrawPos, this.pawn.Map);
			}
			this.jitterer.AddOffset(0.05f, (float)Rand.Range(0, 360));
		}
	}
}
