using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE4 RID: 3300
	public class PawnLeaner
	{
		// Token: 0x0600489E RID: 18590 RVA: 0x002612D6 File Offset: 0x0025F6D6
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600489F RID: 18591 RVA: 0x00261300 File Offset: 0x0025F700
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x00261338 File Offset: 0x0025F738
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

		// Token: 0x060048A1 RID: 18593 RVA: 0x002613B4 File Offset: 0x0025F7B4
		public bool ShouldLean()
		{
			Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
			return stance_Busy != null && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x0026140D File Offset: 0x0025F80D
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}

		// Token: 0x04003120 RID: 12576
		private Pawn pawn;

		// Token: 0x04003121 RID: 12577
		private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

		// Token: 0x04003122 RID: 12578
		private float leanOffsetCurPct = 0f;

		// Token: 0x04003123 RID: 12579
		private const float LeanOffsetPctChangeRate = 0.075f;

		// Token: 0x04003124 RID: 12580
		private const float LeanOffsetDistanceMultiplier = 0.5f;
	}
}
