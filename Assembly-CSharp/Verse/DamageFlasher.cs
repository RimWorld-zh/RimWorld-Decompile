using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CEC RID: 3308
	public class DamageFlasher
	{
		// Token: 0x04003153 RID: 12627
		private int lastDamageTick = -9999;

		// Token: 0x04003154 RID: 12628
		private const int DamagedMatTicksTotal = 16;

		// Token: 0x060048DA RID: 18650 RVA: 0x00263C19 File Offset: 0x00262019
		public DamageFlasher(Pawn pawn)
		{
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060048DB RID: 18651 RVA: 0x00263C30 File Offset: 0x00262030
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060048DC RID: 18652 RVA: 0x00263C5C File Offset: 0x0026205C
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x00263C80 File Offset: 0x00262080
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x00263CA8 File Offset: 0x002620A8
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}
	}
}
