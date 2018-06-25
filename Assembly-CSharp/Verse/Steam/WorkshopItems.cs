using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FCB RID: 4043
	public static class WorkshopItems
	{
		// Token: 0x04003FE3 RID: 16355
		private static List<WorkshopItem> subbedItems = new List<WorkshopItem>();

		// Token: 0x060061BA RID: 25018 RVA: 0x003151D5 File Offset: 0x003135D5
		static WorkshopItems()
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x060061BB RID: 25019 RVA: 0x003151E8 File Offset: 0x003135E8
		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x060061BC RID: 25020 RVA: 0x00315204 File Offset: 0x00313604
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

		// Token: 0x060061BD RID: 25021 RVA: 0x00315254 File Offset: 0x00313654
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

		// Token: 0x060061BE RID: 25022 RVA: 0x003152B4 File Offset: 0x003136B4
		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x003152D8 File Offset: 0x003136D8
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

		// Token: 0x060061C0 RID: 25024 RVA: 0x00315360 File Offset: 0x00313760
		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060061C1 RID: 25025 RVA: 0x00315368 File Offset: 0x00313768
		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x00315370 File Offset: 0x00313770
		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060061C3 RID: 25027 RVA: 0x00315378 File Offset: 0x00313778
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
