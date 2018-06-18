using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000D1E RID: 3358
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		// Token: 0x060049E6 RID: 18918 RVA: 0x00269EAE File Offset: 0x002682AE
		public HediffCompProperties_VerbGiver()
		{
			this.compClass = typeof(HediffComp_VerbGiver);
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x00269ED8 File Offset: 0x002682D8
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

		// Token: 0x0400321F RID: 12831
		public List<VerbProperties> verbs = null;

		// Token: 0x04003220 RID: 12832
		public List<Tool> tools = null;
	}
}
