using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CEA RID: 3306
	public class Pawn_DrawTracker
	{
		// Token: 0x0400314F RID: 12623
		private Pawn pawn;

		// Token: 0x04003150 RID: 12624
		public PawnTweener tweener;

		// Token: 0x04003151 RID: 12625
		private JitterHandler jitterer;

		// Token: 0x04003152 RID: 12626
		public PawnLeaner leaner;

		// Token: 0x04003153 RID: 12627
		public PawnRenderer renderer;

		// Token: 0x04003154 RID: 12628
		public PawnUIOverlay ui;

		// Token: 0x04003155 RID: 12629
		private PawnFootprintMaker footprintMaker;

		// Token: 0x04003156 RID: 12630
		private PawnBreathMoteMaker breathMoteMaker;

		// Token: 0x04003157 RID: 12631
		private const float MeleeJitterDistance = 0.5f;

		// Token: 0x060048CE RID: 18638 RVA: 0x00263B20 File Offset: 0x00261F20
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
		// (get) Token: 0x060048CF RID: 18639 RVA: 0x00263B90 File Offset: 0x00261F90
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

		// Token: 0x060048D0 RID: 18640 RVA: 0x00263BF8 File Offset: 0x00261FF8
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

		// Token: 0x060048D1 RID: 18641 RVA: 0x00263C8E File Offset: 0x0026208E
		public void DrawAt(Vector3 loc)
		{
			this.renderer.RenderPawnAt(loc);
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x00263C9D File Offset: 0x0026209D
		public void Notify_Spawned()
		{
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x00263CAB File Offset: 0x002620AB
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.leaner.Notify_WarmingCastAlongLine(newShootLine, ShootPosition);
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x00263CBB File Offset: 0x002620BB
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (!this.pawn.Destroyed)
			{
				this.jitterer.Notify_DamageApplied(dinfo);
				this.renderer.Notify_DamageApplied(dinfo);
			}
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x00263CEB File Offset: 0x002620EB
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (!this.pawn.Destroyed)
			{
				this.jitterer.Notify_DamageDeflected(dinfo);
			}
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x00263D10 File Offset: 0x00262110
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

		// Token: 0x060048D7 RID: 18647 RVA: 0x00263DB4 File Offset: 0x002621B4
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
