using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DFC RID: 3580
	public class CompAffectsSky : ThingComp
	{
		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x0600512A RID: 20778 RVA: 0x0029B614 File Offset: 0x00299A14
		public CompProperties_AffectsSky Props
		{
			get
			{
				return (CompProperties_AffectsSky)this.props;
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x0600512B RID: 20779 RVA: 0x0029B634 File Offset: 0x00299A34
		public virtual float LerpFactor
		{
			get
			{
				float result;
				if (this.HasAutoAnimation)
				{
					int ticksGame = Find.TickManager.TicksGame;
					float num;
					if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration)
					{
						num = (float)(ticksGame - this.autoAnimationStartTick) / (float)this.fadeInDuration;
					}
					else if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration)
					{
						num = 1f;
					}
					else
					{
						num = 1f - (float)(ticksGame - this.autoAnimationStartTick - this.fadeInDuration - this.holdDuration) / (float)this.fadeOutDuration;
					}
					result = Mathf.Clamp01(num * this.autoAnimationTarget);
				}
				else
				{
					result = 0f;
				}
				return result;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x0600512C RID: 20780 RVA: 0x0029B6F0 File Offset: 0x00299AF0
		public bool HasAutoAnimation
		{
			get
			{
				return Find.TickManager.TicksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration + this.fadeOutDuration;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x0600512D RID: 20781 RVA: 0x0029B72C File Offset: 0x00299B2C
		public virtual SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(this.Props.glow, this.Props.skyColors, this.Props.lightsourceShineSize, this.Props.lightsourceShineIntensity);
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x0600512E RID: 20782 RVA: 0x0029B774 File Offset: 0x00299B74
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x0029B794 File Offset: 0x00299B94
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.autoAnimationStartTick, "autoAnimationStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.fadeInDuration, "fadeInDuration", 0, false);
			Scribe_Values.Look<int>(ref this.holdDuration, "holdDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.autoAnimationTarget, "autoAnimationTarget", 0f, false);
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x0029B806 File Offset: 0x00299C06
		public void StartFadeInHoldFadeOut(int fadeInDuration, int holdDuration, int fadeOutDuration, float target = 1f)
		{
			this.autoAnimationStartTick = Find.TickManager.TicksGame;
			this.fadeInDuration = fadeInDuration;
			this.holdDuration = holdDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.autoAnimationTarget = target;
		}

		// Token: 0x04003546 RID: 13638
		private int autoAnimationStartTick;

		// Token: 0x04003547 RID: 13639
		private int fadeInDuration;

		// Token: 0x04003548 RID: 13640
		private int holdDuration;

		// Token: 0x04003549 RID: 13641
		private int fadeOutDuration;

		// Token: 0x0400354A RID: 13642
		private float autoAnimationTarget;
	}
}
