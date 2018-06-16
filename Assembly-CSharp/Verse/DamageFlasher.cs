using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CEE RID: 3310
	public class DamageFlasher
	{
		// Token: 0x060048C8 RID: 18632 RVA: 0x0026274D File Offset: 0x00260B4D
		public DamageFlasher(Pawn pawn)
		{
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060048C9 RID: 18633 RVA: 0x00262764 File Offset: 0x00260B64
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060048CA RID: 18634 RVA: 0x00262790 File Offset: 0x00260B90
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x002627B4 File Offset: 0x00260BB4
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x002627DC File Offset: 0x00260BDC
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0400314A RID: 12618
		private int lastDamageTick = -9999;

		// Token: 0x0400314B RID: 12619
		private const int DamagedMatTicksTotal = 16;
	}
}
