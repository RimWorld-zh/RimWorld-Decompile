using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE7 RID: 3303
	public class PawnTweener
	{
		// Token: 0x060048AD RID: 18605 RVA: 0x002619F7 File Offset: 0x0025FDF7
		public PawnTweener(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x060048AE RID: 18606 RVA: 0x00261A28 File Offset: 0x0025FE28
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x060048AF RID: 18607 RVA: 0x00261A44 File Offset: 0x0025FE44
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x060048B0 RID: 18608 RVA: 0x00261A6C File Offset: 0x0025FE6C
		public void PreDrawPosCalculation()
		{
			if (this.lastDrawFrame != RealTime.frameCount)
			{
				if (this.lastDrawFrame < RealTime.frameCount - 1)
				{
					this.ResetTweenedPosToRoot();
				}
				else
				{
					this.lastTickSpringPos = this.tweenedPos;
					float tickRateMultiplier = Find.TickManager.TickRateMultiplier;
					if (tickRateMultiplier < 5f)
					{
						Vector3 a = this.TweenedPosRoot() - this.tweenedPos;
						float num = 0.09f * (RealTime.deltaTime * 60f * tickRateMultiplier);
						if (RealTime.deltaTime > 0.05f)
						{
							num = Mathf.Min(num, 1f);
						}
						this.tweenedPos += a * num;
					}
					else
					{
						this.tweenedPos = this.TweenedPosRoot();
					}
				}
				this.lastDrawFrame = RealTime.frameCount;
			}
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x00261B4B File Offset: 0x0025FF4B
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot();
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x00261B68 File Offset: 0x0025FF68
		private Vector3 TweenedPosRoot()
		{
			Vector3 result;
			if (!this.pawn.Spawned)
			{
				result = this.pawn.Position.ToVector3Shifted();
			}
			else
			{
				float num = this.MovedPercent();
				result = this.pawn.pather.nextCell.ToVector3Shifted() * num + this.pawn.Position.ToVector3Shifted() * (1f - num) + PawnCollisionTweenerUtility.PawnCollisionPosOffsetFor(this.pawn);
			}
			return result;
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x00261BFC File Offset: 0x0025FFFC
		private float MovedPercent()
		{
			float result;
			if (!this.pawn.pather.Moving)
			{
				result = 0f;
			}
			else if (this.pawn.stances.FullBodyBusy)
			{
				result = 0f;
			}
			else if (this.pawn.pather.BuildingBlockingNextPathCell() != null)
			{
				result = 0f;
			}
			else if (this.pawn.pather.NextCellDoorToManuallyOpen() != null)
			{
				result = 0f;
			}
			else if (this.pawn.pather.WillCollideWithPawnOnNextPathCell())
			{
				result = 0f;
			}
			else
			{
				result = 1f - this.pawn.pather.nextCellCostLeft / this.pawn.pather.nextCellCostTotal;
			}
			return result;
		}

		// Token: 0x04003132 RID: 12594
		private Pawn pawn;

		// Token: 0x04003133 RID: 12595
		private Vector3 tweenedPos = new Vector3(0f, 0f, 0f);

		// Token: 0x04003134 RID: 12596
		private int lastDrawFrame = -1;

		// Token: 0x04003135 RID: 12597
		private Vector3 lastTickSpringPos;

		// Token: 0x04003136 RID: 12598
		private const float SpringTightness = 0.09f;
	}
}
