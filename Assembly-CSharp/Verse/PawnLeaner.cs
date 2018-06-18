using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE3 RID: 3299
	public class PawnLeaner
	{
		// Token: 0x0600489C RID: 18588 RVA: 0x002612AE File Offset: 0x0025F6AE
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x0600489D RID: 18589 RVA: 0x002612D8 File Offset: 0x0025F6D8
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x00261310 File Offset: 0x0025F710
		public void LeanerTick()
		{
			if (this.ShouldLean())
			{
				this.leanOffsetCurPct += 0.075f;
				if (this.leanOffsetCurPct > 1f)
				{
					this.leanOffsetCurPct = 1f;
				}
			}
			else
			{
				this.leanOffsetCurPct -= 0.075f;
				if (this.leanOffsetCurPct < 0f)
				{
					this.leanOffsetCurPct = 0f;
				}
			}
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x0026138C File Offset: 0x0025F78C
		public bool ShouldLean()
		{
			Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
			return stance_Busy != null && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x002613E5 File Offset: 0x0025F7E5
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}

		// Token: 0x0400311E RID: 12574
		private Pawn pawn;

		// Token: 0x0400311F RID: 12575
		private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

		// Token: 0x04003120 RID: 12576
		private float leanOffsetCurPct = 0f;

		// Token: 0x04003121 RID: 12577
		private const float LeanOffsetPctChangeRate = 0.075f;

		// Token: 0x04003122 RID: 12578
		private const float LeanOffsetDistanceMultiplier = 0.5f;
	}
}
