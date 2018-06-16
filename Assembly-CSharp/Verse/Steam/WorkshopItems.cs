using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC6 RID: 4038
	public static class WorkshopItems
	{
		// Token: 0x06006183 RID: 24963 RVA: 0x00312301 File Offset: 0x00310701
		static WorkshopItems()
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06006184 RID: 24964 RVA: 0x00312314 File Offset: 0x00310714
		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06006185 RID: 24965 RVA: 0x00312330 File Offset: 0x00310730
		public static int DownloadingItemsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < WorkshopItems.subbedItems.Count; i++)
				{
					if (WorkshopItems.subbedItems[i] is WorkshopItem_NotInstalled)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x00312380 File Offset: 0x00310780
		public static WorkshopItem GetItem(PublishedFileId_t pfid)
		{
			for (int i = 0; i < WorkshopItems.subbedItems.Count; i++)
			{
				if (WorkshopItems.subbedItems[i].PublishedFileId == pfid)
				{
					return WorkshopItems.subbedItems[i];
				}
			}
			return null;
		}

		// Token: 0x06006187 RID: 24967 RVA: 0x003123E0 File Offset: 0x003107E0
		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		// Token: 0x06006188 RID: 24968 RVA: 0x00312404 File Offset: 0x00310804
		private static void RebuildItemsList()
		{
			if (SteamManager.Initialized)
			{
				WorkshopItems.subbedItems.Clear();
				foreach (PublishedFileId_t pfid in Workshop.AllSubscribedItems())
				{
					WorkshopItems.subbedItems.Add(WorkshopItem.MakeFrom(pfid));
				}
				ModLister.RebuildModList();
				ScenarioLister.MarkDirty();
			}
		}

		// Token: 0x06006189 RID: 24969 RVA: 0x0031248C File Offset: 0x0031088C
		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x0600618A RID: 24970 RVA: 0x00312494 File Offset: 0x00310894
		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x0600618B RID: 24971 RVA: 0x0031249C File Offset: 0x0031089C
		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x0600618C RID: 24972 RVA: 0x003124A4 File Offset: 0x003108A4
		public static string DebugOutput()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Subscribed items:");
			foreach (WorkshopItem workshopItem in WorkshopItems.subbedItems)
			{
				stringBuilder.AppendLine("  " + workshopItem.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003FB7 RID: 16311
		private static List<WorkshopItem> subbedItems = new List<WorkshopItem>();
	}
}
