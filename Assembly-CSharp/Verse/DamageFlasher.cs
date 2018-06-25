using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CED RID: 3309
	public class DamageFlasher
	{
		// Token: 0x0400315A RID: 12634
		private int lastDamageTick = -9999;

		// Token: 0x0400315B RID: 12635
		private const int DamagedMatTicksTotal = 16;

		// Token: 0x060048DA RID: 18650 RVA: 0x00263EF9 File Offset: 0x002622F9
		public DamageFlasher(Pawn pawn)
		{
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060048DB RID: 18651 RVA: 0x00263F10 File Offset: 0x00262310
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060048DC RID: 18652 RVA: 0x00263F3C File Offset: 0x0026233C
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x00263F60 File Offset: 0x00262360
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x00263F88 File Offset: 0x00262388
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}
	}
}
