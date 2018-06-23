using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002CD RID: 717
	public class ScenarioDef : Def
	{
		// Token: 0x0400071E RID: 1822
		public Scenario scenario;

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0006994C File Offset: 0x00067D4C
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

		// Token: 0x06000BDA RID: 3034 RVA: 0x000699B8 File Offset: 0x00067DB8
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
