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

		public PawnTweener(Pawn pawn)
		{
			this.pawn = pawn;
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
					if (tickRateMultiplier < 5.0)
					{
						Vector3 a = this.TweenedPosRoot() - this.tweenedPos;
						this.tweenedPos += a * 0.09f * (float)(RealTime.deltaTime * 60.0 * tickRateMultiplier);
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
				result = this.pawn.pather.nextCell.ToVector3Shifted() * num + this.pawn.Position.ToVector3Shifted() * (float)(1.0 - num) + PawnCollisionTweenerUtility.PawnCollisionPosOffsetFor(this.pawn);
			}
			return result;
		}

		private float MovedPercent()
		{
			return (float)(this.pawn.pather.Moving ? ((!this.pawn.stances.FullBodyBusy) ? ((this.pawn.pather.BuildingBlockingNextPathCell() == null) ? ((this.pawn.pather.NextCellDoorToManuallyOpen() == null) ? ((!this.pawn.pather.WillCollideWithPawnOnNextPathCell()) ? (1.0 - this.pawn.pather.nextCellCostLeft / this.pawn.pather.nextCellCostTotal) : 0.0) : 0.0) : 0.0) : 0.0) : 0.0);
		}
	}
}
