using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002CF RID: 719
	public class ScenarioDef : Def
	{
		// Token: 0x04000720 RID: 1824
		public Scenario scenario;

		// Token: 0x06000BDC RID: 3036 RVA: 0x00069A98 File Offset: 0x00067E98
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.scenario.name.NullOrEmpty())
			{
				this.scenario.name = this.label;
			}
			if (this.scenario.description.NullOrEmpty())
			{
				this.scenario.description = this.description;
			}
			this.scenario.Category = ScenarioCategory.FromDef;
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00069B04 File Offset: 0x00067F04
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.scenario == null)
			{
				yield return "null scenario";
			}
			foreach (string se in this.scenario.ConfigErrors())
			{
				yield return se;
			}
			yield break;
		}
	}
}
