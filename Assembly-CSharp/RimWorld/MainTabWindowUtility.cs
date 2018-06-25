using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B2 RID: 690
	public static class MainTabWindowUtility
	{
		// Token: 0x06000B86 RID: 2950 RVA: 0x00068090 File Offset: 0x00066490
		public static void NotifyAllPawnTables_PawnsChanged()
		{
			if (Find.WindowStack != null)
			{
				WindowStack windowStack = Find.WindowStack;
				for (int i = 0; i < windowStack.Count; i++)
				{
					MainTabWindow_PawnTable mainTabWindow_PawnTable = windowStack[i] as MainTabWindow_PawnTable;
					if (mainTabWindow_PawnTable != null)
					{
						mainTabWindow_PawnTable.Notify_PawnsChanged();
					}
				}
			}
		}
	}
}
