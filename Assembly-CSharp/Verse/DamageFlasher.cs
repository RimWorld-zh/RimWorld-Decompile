using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CEA RID: 3306
	public class DamageFlasher
	{
		// Token: 0x04003153 RID: 12627
		private int lastDamageTick = -9999;

		// Token: 0x04003154 RID: 12628
		private const int DamagedMatTicksTotal = 16;

		// Token: 0x060048D7 RID: 18647 RVA: 0x00263B3D File Offset: 0x00261F3D
		public DamageFlasher(Pawn pawn)
		{
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060048D8 RID: 18648 RVA: 0x00263B54 File Offset: 0x00261F54
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x060048D9 RID: 18649 RVA: 0x00263B80 File Offset: 0x00261F80
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x00263BA4 File Offset: 0x00261FA4
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x00263BCC File Offset: 0x00261FCC
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}
	}
}
