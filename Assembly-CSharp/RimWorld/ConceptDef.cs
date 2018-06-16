using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002EC RID: 748
	public class ConceptDef : Def
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0006D8C0 File Offset: 0x0006BCC0
		public bool TriggeredDirect
		{
			get
			{
				return this.priority <= 0f;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x0006D8E8 File Offset: 0x0006BCE8
		public string HelpTextAdjusted
		{
			get
			{
				return this.helpText.AdjustedForKeys(null, true);
			}
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0006D90A File Offset: 0x0006BD0A
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = this.defName.ToString();
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0006D93C File Offset: 0x0006BD3C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string str in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return str;
			}
			if (this.priority > 9999999f)
			{
				yield return "priority isn't set";
			}
			if (this.helpText.NullOrEmpty())
			{
				yield return "no help text";
			}
			if (this.TriggeredDirect && this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			ConceptDef.tmpParseErrors.Clear();
			this.helpText.AdjustedForKeys(ConceptDef.tmpParseErrors, false);
			for (int i = 0; i < ConceptDef.tmpParseErrors.Count; i++)
			{
				yield return "helpText error: " + ConceptDef.tmpParseErrors[i];
			}
			yield break;
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0006D968 File Offset: 0x0006BD68
		public static ConceptDef Named(string defName)
		{
			return DefDatabase<ConceptDef>.GetNamed(defName, true);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0006D984 File Offset: 0x0006BD84
		public void HighlightAllTags()
		{
			if (this.highlightTags != null)
			{
				for (int i = 0; i < this.highlightTags.Count; i++)
				{
					UIHighlighter.HighlightTag(this.highlightTags[i]);
				}
			}
		}

		// Token: 0x040007F6 RID: 2038
		public float priority = float.MaxValue;

		// Token: 0x040007F7 RID: 2039
		public bool noteTeaches = false;

		// Token: 0x040007F8 RID: 2040
		public bool needsOpportunity = false;

		// Token: 0x040007F9 RID: 2041
		public bool opportunityDecays = true;

		// Token: 0x040007FA RID: 2042
		public ProgramState gameMode = ProgramState.Playing;

		// Token: 0x040007FB RID: 2043
		[MustTranslate]
		private string helpText = null;

		// Token: 0x040007FC RID: 2044
		[NoTranslate]
		public List<string> highlightTags = null;

		// Token: 0x040007FD RID: 2045
		private static List<string> tmpParseErrors = new List<string>();
	}
}
