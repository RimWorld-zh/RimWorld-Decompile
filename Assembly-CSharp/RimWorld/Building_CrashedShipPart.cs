using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020006A1 RID: 1697
	public class Building_CrashedShipPart : Building
	{
		// Token: 0x06002426 RID: 9254 RVA: 0x00136869 File Offset: 0x00134C69
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x00136884 File Offset: 0x00134C84
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

		// Token: 0x06002428 RID: 9256 RVA: 0x001368F6 File Offset: 0x00134CF6
		public override void Tick()
		{
			base.Tick();
			this.age++;
		}

		// Token: 0x04001417 RID: 5143
		protected int age;
	}
}
