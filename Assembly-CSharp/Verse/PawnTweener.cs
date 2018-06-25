using System;
using UnityEngine;

namespace Verse
{
	public class PawnTweener
	{
		private Pawn pawn;

		private Vector3 tweenedPos = new Vector3(0f, 0f, 0f);

		private int lastDrawFrame = -1;

		private Vector3 lastTickSpringPos;

		private const float SpringTightness = 0.09f;

		public PawnTweener(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

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

		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot();
			this.lastTickSpringPos = this.tweenedPos;
		}

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
