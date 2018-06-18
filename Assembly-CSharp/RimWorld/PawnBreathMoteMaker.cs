using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045F RID: 1119
	public class PawnBreathMoteMaker
	{
		// Token: 0x0600139C RID: 5020 RVA: 0x000A91D1 File Offset: 0x000A75D1
		public PawnBreathMoteMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x000A91E8 File Offset: 0x000A75E8
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

		// Token: 0x0600139E RID: 5022 RVA: 0x000A929C File Offset: 0x000A769C
		private void TryMakeBreathMote()
		{
			Vector3 loc = this.pawn.Drawer.DrawPos + this.pawn.Drawer.renderer.BaseHeadOffsetAt(this.pawn.Rotation) + this.pawn.Rotation.FacingCell.ToVector3() * 0.21f + PawnBreathMoteMaker.BreathOffset;
			Vector3 lastTickTweenedVelocity = this.pawn.Drawer.tweener.LastTickTweenedVelocity;
			MoteMaker.ThrowBreathPuff(loc, this.pawn.Map, this.pawn.Rotation.AsAngle, lastTickTweenedVelocity);
		}

		// Token: 0x04000BDD RID: 3037
		private Pawn pawn;

		// Token: 0x04000BDE RID: 3038
		private bool doThisBreath = false;

		// Token: 0x04000BDF RID: 3039
		private const int BreathDuration = 80;

		// Token: 0x04000BE0 RID: 3040
		private const int BreathInterval = 320;

		// Token: 0x04000BE1 RID: 3041
		private const int MoteInterval = 8;

		// Token: 0x04000BE2 RID: 3042
		private const float MaxBreathTemperature = 0f;

		// Token: 0x04000BE3 RID: 3043
		private static readonly Vector3 BreathOffset = new Vector3(0f, 0f, -0.04f);

		// Token: 0x04000BE4 RID: 3044
		private const float BreathRotationOffsetDist = 0.21f;
	}
}
