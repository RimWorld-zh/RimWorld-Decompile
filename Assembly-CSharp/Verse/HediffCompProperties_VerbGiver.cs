using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		public List<VerbProperties> verbs;

		public List<Tool> tools;

		public HediffCompProperties_VerbGiver()
		{
			base.compClass = typeof(HediffComp_VerbGiver);
		}

		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			using (IEnumerator<string> enumerator = base.ConfigErrors(parentDef).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string err = enumerator.Current;
					yield return err;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.tools == null)
				yield break;
			Tool dupeTool = this.tools.SelectMany(delegate(Tool lhs)
			{
				HediffCompProperties_VerbGiver _0024this = ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_00d6: stateMachine*/)._0024this;
				return from rhs in ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_00d6: stateMachine*/)._0024this.tools
				where lhs != rhs && lhs.label == rhs.label
				select rhs;
			}).FirstOrDefault();
			if (dupeTool == null)
				yield break;
			yield return string.Format("duplicate hediff tool id {0}", dupeTool.Id);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0134:
			/*Error near IL_0135: Unexpected return in MoveNext()*/;
		}
	}
}
