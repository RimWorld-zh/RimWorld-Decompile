using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000961 RID: 2401
	[DefOf]
	public static class MessageTypeDefOf
	{
		// Token: 0x0600366C RID: 13932 RVA: 0x001D0BEF File Offset: 0x001CEFEF
		static MessageTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MessageTypeDefOf));
		}

		// Token: 0x040022B9 RID: 8889
		public static MessageTypeDef ThreatBig;

		// Token: 0x040022BA RID: 8890
		public static MessageTypeDef ThreatSmall;

		// Token: 0x040022BB RID: 8891
		public static MessageTypeDef PawnDeath;

		// Token: 0x040022BC RID: 8892
		public static MessageTypeDef NegativeHealthEvent;

		// Token: 0x040022BD RID: 8893
		public static MessageTypeDef NegativeEvent;

		// Token: 0x040022BE RID: 8894
		public static MessageTypeDef NeutralEvent;

		// Token: 0x040022BF RID: 8895
		public static MessageTypeDef TaskCompletion;

		// Token: 0x040022C0 RID: 8896
		public static MessageTypeDef PositiveEvent;

		// Token: 0x040022C1 RID: 8897
		public static MessageTypeDef SituationResolved;

		// Token: 0x040022C2 RID: 8898
		public static MessageTypeDef RejectInput;

		// Token: 0x040022C3 RID: 8899
		public static MessageTypeDef CautionInput;

		// Token: 0x040022C4 RID: 8900
		public static MessageTypeDef SilentInput;
	}
}
