using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E08 RID: 3592
	public class CompLifespan : ThingComp
	{
		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06005147 RID: 20807 RVA: 0x0029AAD8 File Offset: 0x00298ED8
		public CompProperties_Lifespan Props
		{
			get
			{
				return (CompProperties_Lifespan)this.props;
			}
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x0029AAF8 File Offset: 0x00298EF8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x0029AB13 File Offset: 0x00298F13
		public override void CompTick()
		{
			this.age++;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x0029AB46 File Offset: 0x00298F46
		public override void CompTickRare()
		{
			this.age += 250;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x0029AB80 File Offset: 0x00298F80
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			string result = "";
			int num = this.Props.lifespanTicks - this.age;
			if (num > 0)
			{
				result = "LifespanExpiry".Translate() + " " + num.ToStringTicksToPeriod();
				if (!text.NullOrEmpty())
				{
					result = "\n" + text;
				}
			}
			return result;
		}

		// Token: 0x0400354E RID: 13646
		public int age = -1;
	}
}
