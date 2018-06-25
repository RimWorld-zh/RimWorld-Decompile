using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200091D RID: 2333
	[DefOf]
	public static class WorkTypeDefOf
	{
		// Token: 0x04001ECD RID: 7885
		public static WorkTypeDef Mining;

		// Token: 0x04001ECE RID: 7886
		public static WorkTypeDef Growing;

		// Token: 0x04001ECF RID: 7887
		public static WorkTypeDef Construction;

		// Token: 0x04001ED0 RID: 7888
		public static WorkTypeDef Warden;

		// Token: 0x04001ED1 RID: 7889
		public static WorkTypeDef Doctor;

		// Token: 0x04001ED2 RID: 7890
		public static WorkTypeDef Firefighter;

		// Token: 0x04001ED3 RID: 7891
		public static WorkTypeDef Hunting;

		// Token: 0x04001ED4 RID: 7892
		public static WorkTypeDef Handling;

		// Token: 0x04001ED5 RID: 7893
		public static WorkTypeDef Crafting;

		// Token: 0x04001ED6 RID: 7894
		public static WorkTypeDef Hauling;

		// Token: 0x06003627 RID: 13863 RVA: 0x001D0D47 File Offset: 0x001CF147
		static WorkTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkTypeDefOf));
		}
	}
}
