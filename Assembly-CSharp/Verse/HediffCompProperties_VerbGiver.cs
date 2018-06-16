using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000D1F RID: 3359
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		// Token: 0x060049E8 RID: 18920 RVA: 0x00269ED6 File Offset: 0x002682D6
		public HediffCompProperties_VerbGiver()
		{
			this.compClass = typeof(HediffComp_VerbGiver);
		}

		// Token: 0x060049E9 RID: 18921 RVA: 0x00269F00 File Offset: 0x00268300
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

		// Token: 0x04003221 RID: 12833
		public List<VerbProperties> verbs = null;

		// Token: 0x04003222 RID: 12834
		public List<Tool> tools = null;
	}
}
