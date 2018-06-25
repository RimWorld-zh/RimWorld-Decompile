using System;

namespace Verse
{
	public class ManeuverDef : Def
	{
		public ToolCapacityDef requiredCapacity = null;

		public VerbProperties verb;

		public RulePackDef combatLogRulesHit;

		public RulePackDef combatLogRulesDeflect;

		public RulePackDef combatLogRulesMiss;

		public RulePackDef combatLogRulesDodge;

		public LogEntryDef logEntryDef;

		public ManeuverDef()
		{
		}
	}
}
