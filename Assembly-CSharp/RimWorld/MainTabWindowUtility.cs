using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B0 RID: 688
	public static class MainTabWindowUtility
	{
		// Token: 0x06000B82 RID: 2946 RVA: 0x00067F40 File Offset: 0x00066340
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
