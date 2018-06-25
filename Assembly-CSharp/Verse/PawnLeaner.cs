using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE3 RID: 3299
	public class PawnLeaner
	{
		// Token: 0x04003130 RID: 12592
		private Pawn pawn;

		// Token: 0x04003131 RID: 12593
		private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

		// Token: 0x04003132 RID: 12594
		private float leanOffsetCurPct = 0f;

		// Token: 0x04003133 RID: 12595
		private const float LeanOffsetPctChangeRate = 0.075f;

		// Token: 0x04003134 RID: 12596
		private const float LeanOffsetDistanceMultiplier = 0.5f;

		// Token: 0x060048B0 RID: 18608 RVA: 0x00262A82 File Offset: 0x00260E82
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x060048B1 RID: 18609 RVA: 0x00262AAC File Offset: 0x00260EAC
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x00262AE4 File Offset: 0x00260EE4
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

		// Token: 0x060048B3 RID: 18611 RVA: 0x00262B60 File Offset: 0x00260F60
		public bool ShouldLean()
		{
			Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
			return stance_Busy != null && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		// Token: 0x060048B4 RID: 18612 RVA: 0x00262BB9 File Offset: 0x00260FB9
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}
	}
}
