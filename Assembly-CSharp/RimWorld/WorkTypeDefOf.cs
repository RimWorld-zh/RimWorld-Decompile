using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200091F RID: 2335
	[DefOf]
	public static class WorkTypeDefOf
	{
		// Token: 0x06003628 RID: 13864 RVA: 0x001D0683 File Offset: 0x001CEA83
		static WorkTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkTypeDefOf));
		}

		// Token: 0x04001EC8 RID: 7880
		public static WorkTypeDef Mining;

		// Token: 0x04001EC9 RID: 7881
		public static WorkTypeDef Growing;

		// Token: 0x04001ECA RID: 7882
		public static WorkTypeDef Construction;

		// Token: 0x04001ECB RID: 7883
		public static WorkTypeDef Warden;

		// Token: 0x04001ECC RID: 7884
		public static WorkTypeDef Doctor;

		// Token: 0x04001ECD RID: 7885
		public static WorkTypeDef Firefighter;

		// Token: 0x04001ECE RID: 7886
		public static WorkTypeDef Hunting;

		// Token: 0x04001ECF RID: 7887
		public static WorkTypeDef Handling;

		// Token: 0x04001ED0 RID: 7888
		public static WorkTypeDef Crafting;

		// Token: 0x04001ED1 RID: 7889
		public static WorkTypeDef Hauling;
	}
}
