using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D62 RID: 3426
	public class Stance_Cooldown : Stance_Busy
	{
		// Token: 0x04003339 RID: 13113
		private const float RadiusPerTick = 0.002f;

		// Token: 0x0400333A RID: 13114
		private const float MaxRadius = 0.5f;

		// Token: 0x06004CCC RID: 19660 RVA: 0x00280743 File Offset: 0x0027EB43
		public Stance_Cooldown()
		{
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x0028074C File Offset: 0x0027EB4C
		public Stance_Cooldown(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x00280758 File Offset: 0x0027EB58
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
