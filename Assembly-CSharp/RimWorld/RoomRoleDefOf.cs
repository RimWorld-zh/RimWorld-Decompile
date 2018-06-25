using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000941 RID: 2369
	[DefOf]
	public static class RoomRoleDefOf
	{
		// Token: 0x0400216E RID: 8558
		public static RoomRoleDef None;

		// Token: 0x0400216F RID: 8559
		public static RoomRoleDef Bedroom;

		// Token: 0x04002170 RID: 8560
		public static RoomRoleDef Barracks;

		// Token: 0x04002171 RID: 8561
		public static RoomRoleDef PrisonCell;

		// Token: 0x04002172 RID: 8562
		public static RoomRoleDef PrisonBarracks;

		// Token: 0x04002173 RID: 8563
		public static RoomRoleDef DiningRoom;

		// Token: 0x04002174 RID: 8564
		public static RoomRoleDef RecRoom;

		// Token: 0x04002175 RID: 8565
		public static RoomRoleDef Hospital;

		// Token: 0x04002176 RID: 8566
		public static RoomRoleDef Laboratory;

		// Token: 0x0600364B RID: 13899 RVA: 0x001D0FCF File Offset: 0x001CF3CF
		static RoomRoleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomRoleDefOf));
		}
	}
}
