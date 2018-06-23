using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE4 RID: 3300
	public class PawnTweener
	{
		// Token: 0x0400313D RID: 12605
		private Pawn pawn;

		// Token: 0x0400313E RID: 12606
		private Vector3 tweenedPos = new Vector3(0f, 0f, 0f);

		// Token: 0x0400313F RID: 12607
		private int lastDrawFrame = -1;

		// Token: 0x04003140 RID: 12608
		private Vector3 lastTickSpringPos;

		// Token: 0x04003141 RID: 12609
		private const float SpringTightness = 0.09f;

		// Token: 0x060048BE RID: 18622 RVA: 0x00262E0F File Offset: 0x0026120F
		public PawnTweener(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x060048BF RID: 18623 RVA: 0x00262E40 File Offset: 0x00261240
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x060048C0 RID: 18624 RVA: 0x00262E5C File Offset: 0x0026125C
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x00262E84 File Offset: 0x00261284
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

		// Token: 0x060048C2 RID: 18626 RVA: 0x00262F63 File Offset: 0x00261363
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot();
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x00262F80 File Offset: 0x00261380
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

		// Token: 0x060048C4 RID: 18628 RVA: 0x00263014 File Offset: 0x00261414
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
	}
}
