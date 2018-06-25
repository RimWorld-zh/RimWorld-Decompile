using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E07 RID: 3591
	public class CompLifespan : ThingComp
	{
		// Token: 0x0400355A RID: 13658
		public int age = -1;

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x0600515D RID: 20829 RVA: 0x0029C4A0 File Offset: 0x0029A8A0
		public CompProperties_Lifespan Props
		{
			get
			{
				return (CompProperties_Lifespan)this.props;
			}
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x0029C4C0 File Offset: 0x0029A8C0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x0029C4DB File Offset: 0x0029A8DB
		public override void CompTick()
		{
			this.age++;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x0029C50E File Offset: 0x0029A90E
		public override void CompTickRare()
		{
			this.age += 250;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x0029C548 File Offset: 0x0029A948
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
	}
}
