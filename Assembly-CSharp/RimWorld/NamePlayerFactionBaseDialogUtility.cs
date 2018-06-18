using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200080B RID: 2059
	public static class NamePlayerFactionBaseDialogUtility
	{
		// Token: 0x06002DF1 RID: 11761 RVA: 0x0018278C File Offset: 0x00180B8C
		public static bool IsValidName(string s)
		{
			return s.Length != 0;
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x001827B4 File Offset: 0x00180BB4
		public static void Named(FactionBase factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}
	}
}
