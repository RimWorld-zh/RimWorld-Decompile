using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DFF RID: 3583
	public class CompAffectsSky : ThingComp
	{
		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06005116 RID: 20758 RVA: 0x0029A038 File Offset: 0x00298438
		public CompProperties_AffectsSky Props
		{
			get
			{
				return (CompProperties_AffectsSky)this.props;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06005117 RID: 20759 RVA: 0x0029A058 File Offset: 0x00298458
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

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06005118 RID: 20760 RVA: 0x0029A114 File Offset: 0x00298514
		public bool HasAutoAnimation
		{
			get
			{
				return Find.TickManager.TicksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration + this.fadeOutDuration;
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06005119 RID: 20761 RVA: 0x0029A150 File Offset: 0x00298550
		public virtual SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(this.Props.glow, this.Props.skyColors, this.Props.lightsourceShineSize, this.Props.lightsourceShineIntensity);
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x0600511A RID: 20762 RVA: 0x0029A198 File Offset: 0x00298598
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x0029A1B8 File Offset: 0x002985B8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.autoAnimationStartTick, "autoAnimationStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.fadeInDuration, "fadeInDuration", 0, false);
			Scribe_Values.Look<int>(ref this.holdDuration, "holdDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.autoAnimationTarget, "autoAnimationTarget", 0f, false);
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x0029A22A File Offset: 0x0029862A
		public void StartFadeInHoldFadeOut(int fadeInDuration, int holdDuration, int fadeOutDuration, float target = 1f)
		{
			this.autoAnimationStartTick = Find.TickManager.TicksGame;
			this.fadeInDuration = fadeInDuration;
			this.holdDuration = holdDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.autoAnimationTarget = target;
		}

		// Token: 0x0400353F RID: 13631
		private int autoAnimationStartTick;

		// Token: 0x04003540 RID: 13632
		private int fadeInDuration;

		// Token: 0x04003541 RID: 13633
		private int holdDuration;

		// Token: 0x04003542 RID: 13634
		private int fadeOutDuration;

		// Token: 0x04003543 RID: 13635
		private float autoAnimationTarget;
	}
}
