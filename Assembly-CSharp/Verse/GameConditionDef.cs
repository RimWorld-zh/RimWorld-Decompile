using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse
{
	public class GameConditionDef : Def
	{
		public Type conditionClass = typeof(GameCondition);

		private List<GameConditionDef> exclusiveConditions = null;

		[MustTranslate]
		public string endMessage = (string)null;

		public bool canBePermanent = false;

		public PsychicDroneLevel droneLevel = PsychicDroneLevel.BadMedium;

		public bool preventRain = false;

		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

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
			if (this.conditionClass != null)
				yield break;
			yield return "conditionClass is null";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_00f0:
			/*Error near IL_00f1: Unexpected return in MoveNext()*/;
		}
	}
}
