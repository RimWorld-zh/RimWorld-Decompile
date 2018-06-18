using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D62 RID: 3426
	public class Stance_Cooldown : Stance_Busy
	{
		// Token: 0x06004CB3 RID: 19635 RVA: 0x0027ED87 File Offset: 0x0027D187
		public Stance_Cooldown()
		{
		}

		// Token: 0x06004CB4 RID: 19636 RVA: 0x0027ED90 File Offset: 0x0027D190
		public Stance_Cooldown(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x06004CB5 RID: 19637 RVA: 0x0027ED9C File Offset: 0x0027D19C
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				float radius = Mathf.Min(0.5f, (float)this.ticksLeft * 0.002f);
				GenDraw.DrawCooldownCircle(this.stanceTracker.pawn.Drawer.DrawPos + new Vector3(0f, 0.2f, 0f), radius);
			}
		}

		// Token: 0x04003327 RID: 13095
		private const float RadiusPerTick = 0.002f;

		// Token: 0x04003328 RID: 13096
		private const float MaxRadius = 0.5f;
	}
}
