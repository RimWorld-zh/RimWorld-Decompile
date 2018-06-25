using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FCA RID: 4042
	public static class WorkshopItems
	{
		// Token: 0x04003FDB RID: 16347
		private static List<WorkshopItem> subbedItems = new List<WorkshopItem>();

		// Token: 0x060061BA RID: 25018 RVA: 0x00314F91 File Offset: 0x00313391
		static WorkshopItems()
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x060061BB RID: 25019 RVA: 0x00314FA4 File Offset: 0x003133A4
		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x060061BC RID: 25020 RVA: 0x00314FC0 File Offset: 0x003133C0
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

		// Token: 0x060061BD RID: 25021 RVA: 0x00315010 File Offset: 0x00313410
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

		// Token: 0x060061BE RID: 25022 RVA: 0x00315070 File Offset: 0x00313470
		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x00315094 File Offset: 0x00313494
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

		// Token: 0x060061C0 RID: 25024 RVA: 0x0031511C File Offset: 0x0031351C
		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060061C1 RID: 25025 RVA: 0x00315124 File Offset: 0x00313524
		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x0031512C File Offset: 0x0031352C
		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060061C3 RID: 25027 RVA: 0x00315134 File Offset: 0x00313534
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
	}
}
