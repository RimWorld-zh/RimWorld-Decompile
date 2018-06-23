using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000659 RID: 1625
	public class TaleData_Def : TaleData
	{
		// Token: 0x04001345 RID: 4933
		public Def def;

		// Token: 0x04001346 RID: 4934
		private string tmpDefName;

		// Token: 0x04001347 RID: 4935
		private Type tmpDefType;

		// Token: 0x060021F7 RID: 8695 RVA: 0x00120268 File Offset: 0x0011E668
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

		// Token: 0x060021F8 RID: 8696 RVA: 0x0012031C File Offset: 0x0011E71C
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

		// Token: 0x060021F9 RID: 8697 RVA: 0x00120350 File Offset: 0x0011E750
		public static TaleData_Def GenerateFrom(Def def)
		{
			return new TaleData_Def
			{
				def = def
			};
		}
	}
}
