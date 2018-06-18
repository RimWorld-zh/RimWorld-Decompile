using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E07 RID: 3591
	public class CompLifespan : ThingComp
	{
		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06005145 RID: 20805 RVA: 0x0029AAB8 File Offset: 0x00298EB8
		public CompProperties_Lifespan Props
		{
			get
			{
				return (CompProperties_Lifespan)this.props;
			}
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x0029AAD8 File Offset: 0x00298ED8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x0029AAF3 File Offset: 0x00298EF3
		public override void CompTick()
		{
			this.age++;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x0029AB26 File Offset: 0x00298F26
		public override void CompTickRare()
		{
			this.age += 250;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x0029AB60 File Offset: 0x00298F60
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

		// Token: 0x0400354C RID: 13644
		public int age = -1;
	}
}
