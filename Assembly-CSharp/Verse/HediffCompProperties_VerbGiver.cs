using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000D1E RID: 3358
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		// Token: 0x04003231 RID: 12849
		public List<VerbProperties> verbs = null;

		// Token: 0x04003232 RID: 12850
		public List<Tool> tools = null;

		// Token: 0x060049FA RID: 18938 RVA: 0x0026B69E File Offset: 0x00269A9E
		public HediffCompProperties_VerbGiver()
		{
			this.compClass = typeof(HediffComp_VerbGiver);
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x0026B6C8 File Offset: 0x00269AC8
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
