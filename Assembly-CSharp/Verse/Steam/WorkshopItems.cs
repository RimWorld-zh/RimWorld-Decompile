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
		// Token: 0x060061AA RID: 25002 RVA: 0x003144B1 File Offset: 0x003128B1
		static WorkshopItems()
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x060061AB RID: 25003 RVA: 0x003144C4 File Offset: 0x003128C4
		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x060061AC RID: 25004 RVA: 0x003144E0 File Offset: 0x003128E0
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

		// Token: 0x060061AD RID: 25005 RVA: 0x00314530 File Offset: 0x00312930
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

		// Token: 0x060061AE RID: 25006 RVA: 0x00314590 File Offset: 0x00312990
		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x003145B4 File Offset: 0x003129B4
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

		// Token: 0x060061B0 RID: 25008 RVA: 0x0031463C File Offset: 0x00312A3C
		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x00314644 File Offset: 0x00312A44
		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x0031464C File Offset: 0x00312A4C
		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x00314654 File Offset: 0x00312A54
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

		// Token: 0x04003FD3 RID: 16339
		private static List<WorkshopItem> subbedItems = new List<WorkshopItem>();
	}
}
