using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B2 RID: 690
	public static class MainTabWindowUtility
	{
		// Token: 0x06000B85 RID: 2949 RVA: 0x0006808C File Offset: 0x0006648C
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
