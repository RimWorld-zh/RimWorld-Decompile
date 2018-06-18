using System;

namespace RimWorld
{
	// Token: 0x020006D5 RID: 1749
	public class DeadPlant : Plant
	{
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060025F1 RID: 9713 RVA: 0x0014674C File Offset: 0x00144B4C
		protected override bool Resting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060025F2 RID: 9714 RVA: 0x00146764 File Offset: 0x00144B64
		public override float GrowthRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060025F3 RID: 9715 RVA: 0x00146780 File Offset: 0x00144B80
		public override float CurrentDyingDamagePerTick
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060025F4 RID: 9716 RVA: 0x0014679C File Offset: 0x00144B9C
		public override string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060025F5 RID: 9717 RVA: 0x001467B8 File Offset: 0x00144BB8
		public override PlantLifeStage LifeStage
		{
			get
			{
				return PlantLifeStage.Mature;
			}
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x001467D0 File Offset: 0x00144BD0
		public override string GetInspectStringLowPriority()
		{
			return null;
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x001467E8 File Offset: 0x00144BE8
		public override string GetInspectString()
		{
			return "";
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x00146802 File Offset: 0x00144C02
		public override void CropBlighted()
		{
		}
	}
}
