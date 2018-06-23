using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000D1B RID: 3355
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		// Token: 0x0400322A RID: 12842
		public List<VerbProperties> verbs = null;

		// Token: 0x0400322B RID: 12843
		public List<Tool> tools = null;

		// Token: 0x060049F7 RID: 18935 RVA: 0x0026B2E2 File Offset: 0x002696E2
		public HediffCompProperties_VerbGiver()
		{
			this.compClass = typeof(HediffComp_VerbGiver);
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x0026B30C File Offset: 0x0026970C
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return err;
			}
			if (this.tools != null)
			{
				Tool dupeTool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.label == rhs.label
				select rhs).FirstOrDefault<Tool>();
				if (dupeTool != null)
				{
					yield return string.Format("duplicate hediff tool id {0}", dupeTool.Id);
				}
			}
			yield break;
		}
	}
}
