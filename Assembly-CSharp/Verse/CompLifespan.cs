using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E04 RID: 3588
	public class CompLifespan : ThingComp
	{
		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06005159 RID: 20825 RVA: 0x0029C094 File Offset: 0x0029A494
		public CompProperties_Lifespan Props
		{
			get
			{
				return (CompProperties_Lifespan)this.props;
			}
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x0029C0B4 File Offset: 0x0029A4B4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x0029C0CF File Offset: 0x0029A4CF
		public override void CompTick()
		{
			this.age++;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x0029C102 File Offset: 0x0029A502
		public override void CompTickRare()
		{
			this.age += 250;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x0029C13C File Offset: 0x0029A53C
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

		// Token: 0x04003553 RID: 13651
		public int age = -1;
	}
}
