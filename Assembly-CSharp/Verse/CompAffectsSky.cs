using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DFF RID: 3583
	public class CompAffectsSky : ThingComp
	{
		// Token: 0x0400354D RID: 13645
		private int autoAnimationStartTick;

		// Token: 0x0400354E RID: 13646
		private int fadeInDuration;

		// Token: 0x0400354F RID: 13647
		private int holdDuration;

		// Token: 0x04003550 RID: 13648
		private int fadeOutDuration;

		// Token: 0x04003551 RID: 13649
		private float autoAnimationTarget;

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x0600512E RID: 20782 RVA: 0x0029BA20 File Offset: 0x00299E20
		public CompProperties_AffectsSky Props
		{
			get
			{
				return (CompProperties_AffectsSky)this.props;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x0600512F RID: 20783 RVA: 0x0029BA40 File Offset: 0x00299E40
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

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06005130 RID: 20784 RVA: 0x0029BAFC File Offset: 0x00299EFC
		public bool HasAutoAnimation
		{
			get
			{
				return Find.TickManager.TicksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration + this.fadeOutDuration;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06005131 RID: 20785 RVA: 0x0029BB38 File Offset: 0x00299F38
		public virtual SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(this.Props.glow, this.Props.skyColors, this.Props.lightsourceShineSize, this.Props.lightsourceShineIntensity);
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06005132 RID: 20786 RVA: 0x0029BB80 File Offset: 0x00299F80
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x0029BBA0 File Offset: 0x00299FA0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.autoAnimationStartTick, "autoAnimationStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.fadeInDuration, "fadeInDuration", 0, false);
			Scribe_Values.Look<int>(ref this.holdDuration, "holdDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.autoAnimationTarget, "autoAnimationTarget", 0f, false);
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x0029BC12 File Offset: 0x0029A012
		public void StartFadeInHoldFadeOut(int fadeInDuration, int holdDuration, int fadeOutDuration, float target = 1f)
		{
			this.autoAnimationStartTick = Find.TickManager.TicksGame;
			this.fadeInDuration = fadeInDuration;
			this.holdDuration = holdDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.autoAnimationTarget = target;
		}
	}
}
