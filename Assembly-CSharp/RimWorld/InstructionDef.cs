using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class InstructionDef : Def
	{
		public Type instructionClass = typeof(Instruction_Basic);

		public string text;

		public bool startCentered;

		public bool tutorialModeOnly = true;

		public string eventTagInitiate;

		public InstructionDef eventTagInitiateSource;

		public List<string> eventTagsEnd;

		public List<string> actionTagsAllowed;

		public string rejectInputMessage;

		public ConceptDef concept;

		public List<string> highlightTags;

		public string onMapInstruction;

		public int targetCount;

		public ThingDef thingDef;

		public RecipeDef recipeDef;

		public int recipeTargetCount = 1;

		public ThingDef giveOnActivateDef;

		public int giveOnActivateCount;

		public bool endTutorial;

		public bool resetBuildDesignatorStuffs;

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = base.ConfigErrors().GetEnumerator())
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
			IL_015c:
			/*Error near IL_015d: Unexpected return in MoveNext()*/;
		}
	}
}
