using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A5 RID: 1701
	public class Building_CrashedShipPart : Building
	{
		// Token: 0x0600242C RID: 9260 RVA: 0x001366A9 File Offset: 0x00134AA9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x001366C4 File Offset: 0x00134AC4
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

		// Token: 0x0600242E RID: 9262 RVA: 0x00136736 File Offset: 0x00134B36
		public override void Tick()
		{
			base.Tick();
			this.age++;
		}

		// Token: 0x04001419 RID: 5145
		protected int age;
	}
}
