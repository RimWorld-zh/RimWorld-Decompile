using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A3 RID: 1699
	public class Building_CrashedShipPart : Building
	{
		// Token: 0x0400141B RID: 5147
		protected int age;

		// Token: 0x06002429 RID: 9257 RVA: 0x00136C21 File Offset: 0x00135021
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x00136C3C File Offset: 0x0013503C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append("AwokeDaysAgo".Translate(new object[]
			{
				this.age.TicksToDays().ToString("F1")
			}));
			return stringBuilder.ToString();
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x00136CAE File Offset: 0x001350AE
		public override void Tick()
		{
			base.Tick();
			this.age++;
		}
	}
}
