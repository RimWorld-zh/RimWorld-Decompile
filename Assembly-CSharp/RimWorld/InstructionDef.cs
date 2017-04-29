using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			InstructionDef.<ConfigErrors>c__Iterator9F <ConfigErrors>c__Iterator9F = new InstructionDef.<ConfigErrors>c__Iterator9F();
			<ConfigErrors>c__Iterator9F.<>f__this = this;
			InstructionDef.<ConfigErrors>c__Iterator9F expr_0E = <ConfigErrors>c__Iterator9F;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
