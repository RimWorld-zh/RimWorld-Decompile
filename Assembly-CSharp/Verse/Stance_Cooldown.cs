using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D5F RID: 3423
	public class Stance_Cooldown : Stance_Busy
	{
		// Token: 0x04003332 RID: 13106
		private const float RadiusPerTick = 0.002f;

		// Token: 0x04003333 RID: 13107
		private const float MaxRadius = 0.5f;

		// Token: 0x06004CC8 RID: 19656 RVA: 0x00280337 File Offset: 0x0027E737
		public Stance_Cooldown()
		{
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x00280340 File Offset: 0x0027E740
		public Stance_Cooldown(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x0028034C File Offset: 0x0027E74C
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				float radius = Mathf.Min(0.5f, (float)this.ticksLeft * 0.002f);
				GenDraw.DrawCooldownCircle(this.stanceTracker.pawn.Drawer.DrawPos + new Vector3(0f, 0.2f, 0f), radius);
			}
		}
	}
}
