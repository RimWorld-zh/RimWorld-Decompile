using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002CD RID: 717
	public class ScenarioDef : Def
	{
		// Token: 0x06000BDB RID: 3035 RVA: 0x000698E4 File Offset: 0x00067CE4
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

		// Token: 0x06000BDC RID: 3036 RVA: 0x00069950 File Offset: 0x00067D50
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

		// Token: 0x0400071F RID: 1823
		public Scenario scenario;
	}
}
