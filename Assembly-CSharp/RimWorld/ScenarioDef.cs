using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ScenarioDef : Def
	{
		public Scenario scenario;

		public override void PostLoad()
		{
			base.PostLoad();
			if (this.scenario.name.NullOrEmpty())
			{
				this.scenario.name = base.label;
			}
			if (this.scenario.description.NullOrEmpty())
			{
				this.scenario.description = base.description;
			}
			this.scenario.Category = ScenarioCategory.FromDef;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			if (this.scenario == null)
			{
				yield return "null scenario";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			using (IEnumerator<string> enumerator = this.scenario.ConfigErrors().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string se = enumerator.Current;
					yield return se;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00f5:
			/*Error near IL_00f6: Unexpected return in MoveNext()*/;
		}
	}
}
