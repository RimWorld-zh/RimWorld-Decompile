using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE2 RID: 3298
	public class PawnLeaner
	{
		// Token: 0x04003129 RID: 12585
		private Pawn pawn;

		// Token: 0x0400312A RID: 12586
		private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

		// Token: 0x0400312B RID: 12587
		private float leanOffsetCurPct = 0f;

		// Token: 0x0400312C RID: 12588
		private const float LeanOffsetPctChangeRate = 0.075f;

		// Token: 0x0400312D RID: 12589
		private const float LeanOffsetDistanceMultiplier = 0.5f;

		// Token: 0x060048B0 RID: 18608 RVA: 0x002627A2 File Offset: 0x00260BA2
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x060048B1 RID: 18609 RVA: 0x002627CC File Offset: 0x00260BCC
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x00262804 File Offset: 0x00260C04
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

		// Token: 0x060048B3 RID: 18611 RVA: 0x00262880 File Offset: 0x00260C80
		public bool ShouldLean()
		{
			Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
			return stance_Busy != null && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		// Token: 0x060048B4 RID: 18612 RVA: 0x002628D9 File Offset: 0x00260CD9
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}
	}
}
