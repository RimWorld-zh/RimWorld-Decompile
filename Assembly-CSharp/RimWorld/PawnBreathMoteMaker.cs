using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045B RID: 1115
	public class PawnBreathMoteMaker
	{
		// Token: 0x06001393 RID: 5011 RVA: 0x000A91E1 File Offset: 0x000A75E1
		public PawnBreathMoteMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x000A91F8 File Offset: 0x000A75F8
		public void BreathMoteMakerTick()
		{
			if (this.pawn.RaceProps.Humanlike && !this.pawn.RaceProps.IsMechanoid)
			{
				int num = (Find.TickManager.TicksGame + this.pawn.HashOffset()) % 320;
				if (num == 0)
				{
					this.doThisBreath = (this.pawn.AmbientTemperature < 0f && this.pawn.GetPosture() != PawnPosture.Standing);
				}
				if (this.doThisBreath && num < 80 && num % 8 == 0)
				{
					this.TryMakeBreathMote();
				}
			}
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x000A92AC File Offset: 0x000A76AC
		private void TryMakeBreathMote()
		{
			Vector3 loc = this.pawn.Drawer.DrawPos + this.pawn.Drawer.renderer.BaseHeadOffsetAt(this.pawn.Rotation) + this.pawn.Rotation.FacingCell.ToVector3() * 0.21f + PawnBreathMoteMaker.BreathOffset;
			Vector3 lastTickTweenedVelocity = this.pawn.Drawer.tweener.LastTickTweenedVelocity;
			MoteMaker.ThrowBreathPuff(loc, this.pawn.Map, this.pawn.Rotation.AsAngle, lastTickTweenedVelocity);
		}

		// Token: 0x04000BDA RID: 3034
		private Pawn pawn;

		// Token: 0x04000BDB RID: 3035
		private bool doThisBreath = false;

		// Token: 0x04000BDC RID: 3036
		private const int BreathDuration = 80;

		// Token: 0x04000BDD RID: 3037
		private const int BreathInterval = 320;

		// Token: 0x04000BDE RID: 3038
		private const int MoteInterval = 8;

		// Token: 0x04000BDF RID: 3039
		private const float MaxBreathTemperature = 0f;

		// Token: 0x04000BE0 RID: 3040
		private static readonly Vector3 BreathOffset = new Vector3(0f, 0f, -0.04f);

		// Token: 0x04000BE1 RID: 3041
		private const float BreathRotationOffsetDist = 0.21f;
	}
}
