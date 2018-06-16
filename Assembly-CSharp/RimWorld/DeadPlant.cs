using System;

namespace RimWorld
{
	// Token: 0x020006D5 RID: 1749
	public class DeadPlant : Plant
	{
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060025EF RID: 9711 RVA: 0x001466D4 File Offset: 0x00144AD4
		protected override bool Resting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060025F0 RID: 9712 RVA: 0x001466EC File Offset: 0x00144AEC
		public override float GrowthRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060025F1 RID: 9713 RVA: 0x00146708 File Offset: 0x00144B08
		public override float CurrentDyingDamagePerTick
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060025F2 RID: 9714 RVA: 0x00146724 File Offset: 0x00144B24
		public override string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060025F3 RID: 9715 RVA: 0x00146740 File Offset: 0x00144B40
		public override PlantLifeStage LifeStage
		{
			get
			{
				return PlantLifeStage.Mature;
			}
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x00146758 File Offset: 0x00144B58
		public override string GetInspectStringLowPriority()
		{
			return null;
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x00146770 File Offset: 0x00144B70
		public override string GetInspectString()
		{
			return "";
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x0014678A File Offset: 0x00144B8A
		public override void CropBlighted()
		{
		}
	}
}
