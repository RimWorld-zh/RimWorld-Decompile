using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ConceptDef : Def
	{
		public float priority = 3.40282347E+38f;

		public bool noteTeaches = false;

		public bool needsOpportunity = false;

		public bool opportunityDecays = true;

		public ProgramState gameMode = ProgramState.Playing;

		private string helpText = (string)null;

		public List<string> highlightTags = null;

		public bool TriggeredDirect
		{
			get
			{
				return this.priority <= 0.0;
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
			if (base.defName == "UnnamedDef")
			{
				base.defName = base.defName.ToString();
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string str = enumerator.Current;
					yield return str;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.priority > 9999999.0)
			{
				yield return "priority isn't set";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.helpText.NullOrEmpty())
			{
				yield return "no help text";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!this.TriggeredDirect)
				yield break;
			if (!base.label.NullOrEmpty())
				yield break;
			yield return "no label";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0175:
			/*Error near IL_0176: Unexpected return in MoveNext()*/;
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
