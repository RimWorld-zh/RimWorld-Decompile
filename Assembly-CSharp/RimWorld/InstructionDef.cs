using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class InstructionDef : Def
	{
		public Type instructionClass = typeof(Instruction_Basic);

		public string text;

		public bool startCentered = false;

		public bool tutorialModeOnly = true;

		public string eventTagInitiate;

		public List<string> eventTagsEnd;

		public List<string> actionTagsAllowed = null;

		public string rejectInputMessage = (string)null;

		public ConceptDef concept = null;

		public List<string> highlightTags;

		public string onMapInstruction;

		public int targetCount;

		public ThingDef thingDef;

		public RecipeDef recipeDef;

		public int recipeTargetCount = 1;

		public ThingDef giveOnActivateDef;

		public int giveOnActivateCount;

		public bool endTutorial = false;

		public bool resetBuildDesignatorStuffs = false;

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.instructionClass == null)
			{
				yield return "no instruction class";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.text.NullOrEmpty())
			{
				yield return "no text";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!this.eventTagInitiate.NullOrEmpty())
				yield break;
			yield return "no eventTagInitiate";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0160:
			/*Error near IL_0161: Unexpected return in MoveNext()*/;
		}
	}
}
