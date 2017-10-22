using RimWorld;
using System;
using System.Collections.Generic;

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
			if (this == other)
			{
				return false;
			}
			return this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other);
		}

		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.conditionClass == null)
			{
				yield return "conditionClass is null";
			}
		}
	}
}
