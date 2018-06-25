using System;

namespace Verse
{
	// Token: 0x02000B50 RID: 2896
	public class ManeuverDef : Def
	{
		// Token: 0x040029EF RID: 10735
		public ToolCapacityDef requiredCapacity = null;

		// Token: 0x040029F0 RID: 10736
		public VerbProperties verb;

		// Token: 0x040029F1 RID: 10737
		public RulePackDef combatLogRulesHit;

		// Token: 0x040029F2 RID: 10738
		public RulePackDef combatLogRulesDeflect;

		// Token: 0x040029F3 RID: 10739
		public RulePackDef combatLogRulesMiss;

		// Token: 0x040029F4 RID: 10740
		public RulePackDef combatLogRulesDodge;

		// Token: 0x040029F5 RID: 10741
		public LogEntryDef logEntryDef;
	}
}
