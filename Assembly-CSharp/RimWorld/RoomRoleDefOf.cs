using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000941 RID: 2369
	[DefOf]
	public static class RoomRoleDefOf
	{
		// Token: 0x04002167 RID: 8551
		public static RoomRoleDef None;

		// Token: 0x04002168 RID: 8552
		public static RoomRoleDef Bedroom;

		// Token: 0x04002169 RID: 8553
		public static RoomRoleDef Barracks;

		// Token: 0x0400216A RID: 8554
		public static RoomRoleDef PrisonCell;

		// Token: 0x0400216B RID: 8555
		public static RoomRoleDef PrisonBarracks;

		// Token: 0x0400216C RID: 8556
		public static RoomRoleDef DiningRoom;

		// Token: 0x0400216D RID: 8557
		public static RoomRoleDef RecRoom;

		// Token: 0x0400216E RID: 8558
		public static RoomRoleDef Hospital;

		// Token: 0x0400216F RID: 8559
		public static RoomRoleDef Laboratory;

		// Token: 0x0600364B RID: 13899 RVA: 0x001D0CFB File Offset: 0x001CF0FB
		static RoomRoleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomRoleDefOf));
		}
	}
}
