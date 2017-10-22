using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		public List<VerbProperties> verbs = null;

		public List<Tool> tools = null;

		public HediffCompProperties_VerbGiver()
		{
			base.compClass = typeof(HediffComp_VerbGiver);
		}

		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0(parentDef).GetEnumerator())
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
			Tool dupeTool = this.tools.SelectMany((Func<Tool, IEnumerable<Tool>>)delegate(Tool lhs)
			{
				HediffCompProperties_VerbGiver _0024this = ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_00db: stateMachine*/)._0024this;
				return from rhs in ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_00db: stateMachine*/)._0024this.tools
				where lhs != rhs && lhs.label == rhs.label
				select rhs;
			}).FirstOrDefault();
			if (dupeTool == null)
				yield break;
			yield return string.Format("duplicate hediff tool id {0}", dupeTool.Id);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_013a:
			/*Error near IL_013b: Unexpected return in MoveNext()*/;
		}
	}
}
