using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC5 RID: 4037
	public static class WorkshopItems
	{
		// Token: 0x06006181 RID: 24961 RVA: 0x003123DD File Offset: 0x003107DD
		static WorkshopItems()
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x06006182 RID: 24962 RVA: 0x003123F0 File Offset: 0x003107F0
		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06006183 RID: 24963 RVA: 0x0031240C File Offset: 0x0031080C
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

		// Token: 0x06006184 RID: 24964 RVA: 0x0031245C File Offset: 0x0031085C
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

		// Token: 0x06006185 RID: 24965 RVA: 0x003124BC File Offset: 0x003108BC
		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x003124E0 File Offset: 0x003108E0
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

		// Token: 0x06006187 RID: 24967 RVA: 0x00312568 File Offset: 0x00310968
		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x06006188 RID: 24968 RVA: 0x00312570 File Offset: 0x00310970
		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x06006189 RID: 24969 RVA: 0x00312578 File Offset: 0x00310978
		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x0600618A RID: 24970 RVA: 0x00312580 File Offset: 0x00310980
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

		// Token: 0x04003FB6 RID: 16310
		private static List<WorkshopItem> subbedItems = new List<WorkshopItem>();
	}
}
