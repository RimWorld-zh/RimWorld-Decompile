using System;

namespace RimWorld
{
	// Token: 0x020006D3 RID: 1747
	public class DeadPlant : Plant
	{
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060025EC RID: 9708 RVA: 0x00146C48 File Offset: 0x00145048
		protected override bool Resting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x00146C60 File Offset: 0x00145060
		public override float GrowthRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060025EE RID: 9710 RVA: 0x00146C7C File Offset: 0x0014507C
		public override float CurrentDyingDamagePerTick
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060025EF RID: 9711 RVA: 0x00146C98 File Offset: 0x00145098
		public override string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060025F0 RID: 9712 RVA: 0x00146CB4 File Offset: 0x001450B4
		public override PlantLifeStage LifeStage
		{
			get
			{
				return PlantLifeStage.Mature;
			}
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x00146CCC File Offset: 0x001450CC
		public override string GetInspectStringLowPriority()
		{
			return null;
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x00146CE4 File Offset: 0x001450E4
		public override string GetInspectString()
		{
			return "";
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x00146CFE File Offset: 0x001450FE
		public override void CropBlighted()
		{
		}
	}
}
