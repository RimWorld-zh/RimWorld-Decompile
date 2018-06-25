using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000809 RID: 2057
	public static class NamePlayerFactionBaseDialogUtility
	{
		// Token: 0x06002DEE RID: 11758 RVA: 0x00182AB4 File Offset: 0x00180EB4
		public static bool IsValidName(string s)
		{
			return s.Length != 0;
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x00182ADC File Offset: 0x00180EDC
		public static void Named(FactionBase factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}
	}
}
