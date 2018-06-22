using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093F RID: 2367
	[DefOf]
	public static class RoomRoleDefOf
	{
		// Token: 0x06003647 RID: 13895 RVA: 0x001D0BBB File Offset: 0x001CEFBB
		static RoomRoleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomRoleDefOf));
		}

		// Token: 0x04002166 RID: 8550
		public static RoomRoleDef None;

		// Token: 0x04002167 RID: 8551
		public static RoomRoleDef Bedroom;

		// Token: 0x04002168 RID: 8552
		public static RoomRoleDef Barracks;

		// Token: 0x04002169 RID: 8553
		public static RoomRoleDef PrisonCell;

		// Token: 0x0400216A RID: 8554
		public static RoomRoleDef PrisonBarracks;

		// Token: 0x0400216B RID: 8555
		public static RoomRoleDef DiningRoom;

		// Token: 0x0400216C RID: 8556
		public static RoomRoleDef RecRoom;

		// Token: 0x0400216D RID: 8557
		public static RoomRoleDef Hospital;

		// Token: 0x0400216E RID: 8558
		public static RoomRoleDef Laboratory;
	}
}
