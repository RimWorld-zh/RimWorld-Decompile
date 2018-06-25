using System;

namespace RimWorld
{
	// Token: 0x02000944 RID: 2372
	[DefOf]
	public static class InteractionDefOf
	{
		// Token: 0x040021B8 RID: 8632
		public static InteractionDef Chitchat;

		// Token: 0x040021B9 RID: 8633
		public static InteractionDef DeepTalk;

		// Token: 0x040021BA RID: 8634
		public static InteractionDef Insult;

		// Token: 0x040021BB RID: 8635
		public static InteractionDef RomanceAttempt;

		// Token: 0x040021BC RID: 8636
		public static InteractionDef MarriageProposal;

		// Token: 0x040021BD RID: 8637
		public static InteractionDef BuildRapport;

		// Token: 0x040021BE RID: 8638
		public static InteractionDef RecruitAttempt;

		// Token: 0x040021BF RID: 8639
		public static InteractionDef SparkJailbreak;

		// Token: 0x040021C0 RID: 8640
		public static InteractionDef AnimalChat;

		// Token: 0x040021C1 RID: 8641
		public static InteractionDef TrainAttempt;

		// Token: 0x040021C2 RID: 8642
		public static InteractionDef TameAttempt;

		// Token: 0x040021C3 RID: 8643
		public static InteractionDef Nuzzle;

		// Token: 0x0600364E RID: 13902 RVA: 0x001D1005 File Offset: 0x001CF405
		static InteractionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InteractionDefOf));
		}
	}
}
