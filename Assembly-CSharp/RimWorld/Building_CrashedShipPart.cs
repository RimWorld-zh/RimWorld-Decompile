using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A5 RID: 1701
	public class Building_CrashedShipPart : Building
	{
		// Token: 0x0600242E RID: 9262 RVA: 0x00136721 File Offset: 0x00134B21
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x0013673C File Offset: 0x00134B3C
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

		// Token: 0x06002430 RID: 9264 RVA: 0x001367AE File Offset: 0x00134BAE
		public override void Tick()
		{
			base.Tick();
			this.age++;
		}

		// Token: 0x04001419 RID: 5145
		protected int age;
	}
}
