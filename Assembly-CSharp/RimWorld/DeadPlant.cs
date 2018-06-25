using System;

namespace RimWorld
{
	// Token: 0x020006D3 RID: 1747
	public class DeadPlant : Plant
	{
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x001469E8 File Offset: 0x00144DE8
		protected override bool Resting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060025EE RID: 9710 RVA: 0x00146A00 File Offset: 0x00144E00
		public override float GrowthRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060025EF RID: 9711 RVA: 0x00146A1C File Offset: 0x00144E1C
		public override float CurrentDyingDamagePerTick
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060025F0 RID: 9712 RVA: 0x00146A38 File Offset: 0x00144E38
		public override string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060025F1 RID: 9713 RVA: 0x00146A54 File Offset: 0x00144E54
		public override PlantLifeStage LifeStage
		{
			get
			{
				return PlantLifeStage.Mature;
			}
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x00146A6C File Offset: 0x00144E6C
		public override string GetInspectStringLowPriority()
		{
			return null;
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x00146A84 File Offset: 0x00144E84
		public override string GetInspectString()
		{
			return "";
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x00146A9E File Offset: 0x00144E9E
		public override void CropBlighted()
		{
		}
	}
}
