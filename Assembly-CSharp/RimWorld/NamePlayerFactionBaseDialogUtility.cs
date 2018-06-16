using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200080B RID: 2059
	public static class NamePlayerFactionBaseDialogUtility
	{
		// Token: 0x06002DEF RID: 11759 RVA: 0x001826F8 File Offset: 0x00180AF8
		public static bool IsValidName(string s)
		{
			return s.Length != 0;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x00182720 File Offset: 0x00180B20
		public static void Named(FactionBase factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}
	}
}
