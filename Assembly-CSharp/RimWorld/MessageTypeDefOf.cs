using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095F RID: 2399
	[DefOf]
	public static class MessageTypeDefOf
	{
		// Token: 0x040022BF RID: 8895
		public static MessageTypeDef ThreatBig;

		// Token: 0x040022C0 RID: 8896
		public static MessageTypeDef ThreatSmall;

		// Token: 0x040022C1 RID: 8897
		public static MessageTypeDef PawnDeath;

		// Token: 0x040022C2 RID: 8898
		public static MessageTypeDef NegativeHealthEvent;

		// Token: 0x040022C3 RID: 8899
		public static MessageTypeDef NegativeEvent;

		// Token: 0x040022C4 RID: 8900
		public static MessageTypeDef NeutralEvent;

		// Token: 0x040022C5 RID: 8901
		public static MessageTypeDef TaskCompletion;

		// Token: 0x040022C6 RID: 8902
		public static MessageTypeDef PositiveEvent;

		// Token: 0x040022C7 RID: 8903
		public static MessageTypeDef SituationResolved;

		// Token: 0x040022C8 RID: 8904
		public static MessageTypeDef RejectInput;

		// Token: 0x040022C9 RID: 8905
		public static MessageTypeDef CautionInput;

		// Token: 0x040022CA RID: 8906
		public static MessageTypeDef SilentInput;

		// Token: 0x06003669 RID: 13929 RVA: 0x001D11EB File Offset: 0x001CF5EB
		static MessageTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MessageTypeDefOf));
		}
	}
}
