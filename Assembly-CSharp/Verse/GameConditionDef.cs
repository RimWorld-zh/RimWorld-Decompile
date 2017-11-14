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
			using (IEnumerator<string> enumerator = base.ConfigErrors().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.conditionClass != null)
				yield break;
			yield return "conditionClass is null";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_00ec:
			/*Error near IL_00ed: Unexpected return in MoveNext()*/;
		}
	}
}
