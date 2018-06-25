using System;

namespace Verse
{
	// Token: 0x02000B4F RID: 2895
	public class ManeuverDef : Def
	{
		// Token: 0x040029E8 RID: 10728
		public ToolCapacityDef requiredCapacity = null;

		// Token: 0x040029E9 RID: 10729
		public VerbProperties verb;

		// Token: 0x040029EA RID: 10730
		public RulePackDef combatLogRulesHit;

		// Token: 0x040029EB RID: 10731
		public RulePackDef combatLogRulesDeflect;

		// Token: 0x040029EC RID: 10732
		public RulePackDef combatLogRulesMiss;

		// Token: 0x040029ED RID: 10733
		public RulePackDef combatLogRulesDodge;

		// Token: 0x040029EE RID: 10734
		public LogEntryDef logEntryDef;
	}
}
