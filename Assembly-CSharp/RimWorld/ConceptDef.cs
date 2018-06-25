using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002EE RID: 750
	public class ConceptDef : Def
	{
		// Token: 0x040007F8 RID: 2040
		public float priority = float.MaxValue;

		// Token: 0x040007F9 RID: 2041
		public bool noteTeaches = false;

		// Token: 0x040007FA RID: 2042
		public bool needsOpportunity = false;

		// Token: 0x040007FB RID: 2043
		public bool opportunityDecays = true;

		// Token: 0x040007FC RID: 2044
		public ProgramState gameMode = ProgramState.Playing;

		// Token: 0x040007FD RID: 2045
		[MustTranslate]
		private string helpText = null;

		// Token: 0x040007FE RID: 2046
		[NoTranslate]
		public List<string> highlightTags = null;

		// Token: 0x040007FF RID: 2047
		private static List<string> tmpParseErrors = new List<string>();

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x0006DAC4 File Offset: 0x0006BEC4
		public bool TriggeredDirect
		{
			get
			{
				return this.priority <= 0f;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x0006DAEC File Offset: 0x0006BEEC
		public string HelpTextAdjusted
		{
			get
			{
				return this.helpText.AdjustedForKeys(null, true);
			}
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0006DB0E File Offset: 0x0006BF0E
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = this.defName.ToString();
			}
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0006DB40 File Offset: 0x0006BF40
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

		// Token: 0x06000C60 RID: 3168 RVA: 0x0006DB6C File Offset: 0x0006BF6C
		public static ConceptDef Named(string defName)
		{
			return DefDatabase<ConceptDef>.GetNamed(defName, true);
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0006DB88 File Offset: 0x0006BF88
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
	}
}
