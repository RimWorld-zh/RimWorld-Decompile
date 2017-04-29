using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class ConceptDef : Def
	{
		public float priority = 3.40282347E+38f;

		public bool noteTeaches;

		public bool needsOpportunity;

		public bool opportunityDecays = true;

		public ProgramState gameMode = ProgramState.Playing;

		private string helpText;

		public List<string> highlightTags;

		public bool TriggeredDirect
		{
			get
			{
				return this.priority <= 0f;
			}
		}

		public string HelpTextAdjusted
		{
			get
			{
				return this.helpText.AdjustedForKeys();
			}
		}

		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = this.defName.ToString();
			}
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			ConceptDef.<ConfigErrors>c__Iterator9E <ConfigErrors>c__Iterator9E = new ConceptDef.<ConfigErrors>c__Iterator9E();
			<ConfigErrors>c__Iterator9E.<>f__this = this;
			ConceptDef.<ConfigErrors>c__Iterator9E expr_0E = <ConfigErrors>c__Iterator9E;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public static ConceptDef Named(string defName)
		{
			return DefDatabase<ConceptDef>.GetNamed(defName, true);
		}

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
