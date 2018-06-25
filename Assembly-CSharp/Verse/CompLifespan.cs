using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E06 RID: 3590
	public class CompLifespan : ThingComp
	{
		// Token: 0x04003553 RID: 13651
		public int age = -1;

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x0600515D RID: 20829 RVA: 0x0029C1C0 File Offset: 0x0029A5C0
		public CompProperties_Lifespan Props
		{
			get
			{
				return (CompProperties_Lifespan)this.props;
			}
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x0029C1E0 File Offset: 0x0029A5E0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x0029C1FB File Offset: 0x0029A5FB
		public override void CompTick()
		{
			this.age++;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x0029C22E File Offset: 0x0029A62E
		public override void CompTickRare()
		{
			this.age += 250;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x0029C268 File Offset: 0x0029A668
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
