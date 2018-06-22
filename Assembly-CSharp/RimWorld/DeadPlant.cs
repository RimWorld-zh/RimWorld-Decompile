using System;

namespace RimWorld
{
	// Token: 0x020006D1 RID: 1745
	public class DeadPlant : Plant
	{
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060025E9 RID: 9705 RVA: 0x00146898 File Offset: 0x00144C98
		protected override bool Resting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060025EA RID: 9706 RVA: 0x001468B0 File Offset: 0x00144CB0
		public override float GrowthRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060025EB RID: 9707 RVA: 0x001468CC File Offset: 0x00144CCC
		public override float CurrentDyingDamagePerTick
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060025EC RID: 9708 RVA: 0x001468E8 File Offset: 0x00144CE8
		public override string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x00146904 File Offset: 0x00144D04
		public override PlantLifeStage LifeStage
		{
			get
			{
				return PlantLifeStage.Mature;
			}
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x0014691C File Offset: 0x00144D1C
		public override string GetInspectStringLowPriority()
		{
			return null;
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x00146934 File Offset: 0x00144D34
		public override string GetInspectString()
		{
			return "";
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x0014694E File Offset: 0x00144D4E
		public override void CropBlighted()
		{
		}
	}
}
