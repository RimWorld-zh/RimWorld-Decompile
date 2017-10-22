using RimWorld;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Verse.Steam
{
	public static class WorkshopItems
	{
		private static List<WorkshopItem> subbedItems;

		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

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

		static WorkshopItems()
		{
			WorkshopItems.subbedItems = new List<WorkshopItem>();
			WorkshopItems.RebuildItemsList();
		}

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

		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		private static void RebuildItemsList()
		{
			if (SteamManager.Initialized)
			{
				WorkshopItems.subbedItems.Clear();
				foreach (PublishedFileId_t item in Workshop.AllSubscribedItems())
				{
					WorkshopItems.subbedItems.Add(WorkshopItem.MakeFrom(item));
				}
				ModLister.RebuildModList();
				ScenarioLister.MarkDirty();
			}
		}

		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		public static string DebugOutput()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Subscribed items:");
			List<WorkshopItem>.Enumerator enumerator = WorkshopItems.subbedItems.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					WorkshopItem current = enumerator.Current;
					stringBuilder.AppendLine("  " + current.ToString());
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return stringBuilder.ToString();
		}
	}
}
