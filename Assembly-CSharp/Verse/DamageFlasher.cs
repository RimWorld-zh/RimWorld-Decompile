using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CED RID: 3309
	public class DamageFlasher
	{
		// Token: 0x060048C6 RID: 18630 RVA: 0x00262725 File Offset: 0x00260B25
		public DamageFlasher(Pawn pawn)
		{
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x060048C7 RID: 18631 RVA: 0x0026273C File Offset: 0x00260B3C
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060048C8 RID: 18632 RVA: 0x00262768 File Offset: 0x00260B68
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x0026278C File Offset: 0x00260B8C
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x002627B4 File Offset: 0x00260BB4
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04003148 RID: 12616
		private int lastDamageTick = -9999;

		// Token: 0x04003149 RID: 12617
		private const int DamagedMatTicksTotal = 16;
	}
}
