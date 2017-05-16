using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse
{
	public class GameConditionDef : Def
	{
		public Type conditionClass = typeof(GameCondition);

		private List<GameConditionDef> exclusiveConditions;

		[MustTranslate]
		public string endMessage;

		public bool canBePermanent;

		public PsychicDroneLevel droneLevel = PsychicDroneLevel.BadMedium;

		public bool preventRain;

		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			GameConditionDef.<ConfigErrors>c__Iterator1CE <ConfigErrors>c__Iterator1CE = new GameConditionDef.<ConfigErrors>c__Iterator1CE();
			<ConfigErrors>c__Iterator1CE.<>f__this = this;
			GameConditionDef.<ConfigErrors>c__Iterator1CE expr_0E = <ConfigErrors>c__Iterator1CE;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
