using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000807 RID: 2055
	public static class NamePlayerFactionBaseDialogUtility
	{
		// Token: 0x06002DEA RID: 11754 RVA: 0x00182964 File Offset: 0x00180D64
		public static bool IsValidName(string s)
		{
			return s.Length != 0;
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x0018298C File Offset: 0x00180D8C
		public static void Named(FactionBase factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}
	}
}
