using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE0 RID: 3296
	public class PawnLeaner
	{
		// Token: 0x060048AD RID: 18605 RVA: 0x002626C6 File Offset: 0x00260AC6
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x060048AE RID: 18606 RVA: 0x002626F0 File Offset: 0x00260AF0
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x00262728 File Offset: 0x00260B28
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

		// Token: 0x060048B0 RID: 18608 RVA: 0x002627A4 File Offset: 0x00260BA4
		public bool ShouldLean()
		{
			Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
			return stance_Busy != null && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x002627FD File Offset: 0x00260BFD
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}

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
	}
}
