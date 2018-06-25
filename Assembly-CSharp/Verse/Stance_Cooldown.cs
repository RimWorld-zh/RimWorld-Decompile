using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D61 RID: 3425
	public class Stance_Cooldown : Stance_Busy
	{
		// Token: 0x04003332 RID: 13106
		private const float RadiusPerTick = 0.002f;

		// Token: 0x04003333 RID: 13107
		private const float MaxRadius = 0.5f;

		// Token: 0x06004CCC RID: 19660 RVA: 0x00280463 File Offset: 0x0027E863
		public Stance_Cooldown()
		{
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x0028046C File Offset: 0x0027E86C
		public Stance_Cooldown(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x00280478 File Offset: 0x0027E878
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
