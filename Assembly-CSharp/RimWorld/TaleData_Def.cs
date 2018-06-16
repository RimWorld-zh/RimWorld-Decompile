using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200065D RID: 1629
	public class TaleData_Def : TaleData
	{
		// Token: 0x060021FD RID: 8701 RVA: 0x001200F0 File Offset: 0x0011E4F0
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpDefName = ((this.def == null) ? null : this.def.defName);
				this.tmpDefType = ((this.def == null) ? null : this.def.GetType());
			}
			Scribe_Values.Look<string>(ref this.tmpDefName, "defName", null, false);
			Scribe_Values.Look<Type>(ref this.tmpDefType, "defType", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.tmpDefName != null)
			{
				this.def = GenDefDatabase.GetDef(this.tmpDefType, this.tmpDefName, true);
			}
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x001201A4 File Offset: 0x0011E5A4
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			if (this.def != null)
			{
				yield return new Rule_String(prefix + "_label", this.def.label);
				yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.def.label));
				yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.def.label));
			}
			yield break;
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x001201D8 File Offset: 0x0011E5D8
		public static TaleData_Def GenerateFrom(Def def)
		{
			return new TaleData_Def
			{
				def = def
			};
		}

		// Token: 0x04001348 RID: 4936
		public Def def;

		// Token: 0x04001349 RID: 4937
		private string tmpDefName;

		// Token: 0x0400134A RID: 4938
		private Type tmpDefType;
	}
}
