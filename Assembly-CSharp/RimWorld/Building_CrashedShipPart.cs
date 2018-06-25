using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A3 RID: 1699
	public class Building_CrashedShipPart : Building
	{
		// Token: 0x04001417 RID: 5143
		protected int age;

		// Token: 0x0600242A RID: 9258 RVA: 0x001369B9 File Offset: 0x00134DB9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x001369D4 File Offset: 0x00134DD4
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

		// Token: 0x0600242C RID: 9260 RVA: 0x00136A46 File Offset: 0x00134E46
		public override void Tick()
		{
			base.Tick();
			this.age++;
		}
	}
}
