using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D63 RID: 3427
	public class Stance_Cooldown : Stance_Busy
	{
		// Token: 0x06004CB5 RID: 19637 RVA: 0x0027EDA7 File Offset: 0x0027D1A7
		public Stance_Cooldown()
		{
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x0027EDB0 File Offset: 0x0027D1B0
		public Stance_Cooldown(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x06004CB7 RID: 19639 RVA: 0x0027EDBC File Offset: 0x0027D1BC
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				float radius = Mathf.Min(0.5f, (float)this.ticksLeft * 0.002f);
				GenDraw.DrawCooldownCircle(this.stanceTracker.pawn.Drawer.DrawPos + new Vector3(0f, 0.2f, 0f), radius);
			}
		}

		// Token: 0x04003329 RID: 13097
		private const float RadiusPerTick = 0.002f;

		// Token: 0x0400332A RID: 13098
		private const float MaxRadius = 0.5f;
	}
}
