using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000809 RID: 2057
	public static class NamePlayerFactionBaseDialogUtility
	{
		// Token: 0x06002DED RID: 11757 RVA: 0x00182D18 File Offset: 0x00181118
		public static bool IsValidName(string s)
		{
			return s.Length != 0;
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x00182D40 File Offset: 0x00181140
		public static void Named(FactionBase factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}
	}
}
