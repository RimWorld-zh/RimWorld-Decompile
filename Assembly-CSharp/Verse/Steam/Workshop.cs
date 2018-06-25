using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000FC4 RID: 4036
	[HasDebugOutput]
	public static class Workshop
	{
		// Token: 0x04003FC9 RID: 16329
		private static WorkshopItemHook uploadingHook;

		// Token: 0x04003FCA RID: 16330
		private static UGCUpdateHandle_t curUpdateHandle;

		// Token: 0x04003FCB RID: 16331
		private static WorkshopInteractStage curStage = WorkshopInteractStage.None;

		// Token: 0x04003FCC RID: 16332
		private static Callback<RemoteStoragePublishedFileSubscribed_t> subscribedCallback;

		// Token: 0x04003FCD RID: 16333
		private static Callback<RemoteStoragePublishedFileUnsubscribed_t> unsubscribedCallback;

		// Token: 0x04003FCE RID: 16334
		private static Callback<ItemInstalled_t> installedCallback;

		// Token: 0x04003FCF RID: 16335
		private static CallResult<SubmitItemUpdateResult_t> submitResult;

		// Token: 0x04003FD0 RID: 16336
		private static CallResult<CreateItemResult_t> createResult;

		// Token: 0x04003FD1 RID: 16337
		private static CallResult<SteamUGCRequestUGCDetailsResult_t> requestDetailsResult;

		// Token: 0x04003FD2 RID: 16338
		private static UGCQueryHandle_t detailsQueryHandle;

		// Token: 0x04003FD3 RID: 16339
		private static int detailsQueryCount = -1;

		// Token: 0x04003FD4 RID: 16340
		public const uint InstallInfoFolderNameMaxLength = 257u;

		// Token: 0x04003FD5 RID: 16341
		[CompilerGenerated]
		private static Callback<RemoteStoragePublishedFileSubscribed_t>.DispatchDelegate <>f__mg$cache0;

		// Token: 0x04003FD6 RID: 16342
		[CompilerGenerated]
		private static Callback<ItemInstalled_t>.DispatchDelegate <>f__mg$cache1;

		// Token: 0x04003FD7 RID: 16343
		[CompilerGenerated]
		private static Callback<RemoteStoragePublishedFileUnsubscribed_t>.DispatchDelegate <>f__mg$cache2;

		// Token: 0x04003FD8 RID: 16344
		[CompilerGenerated]
		private static CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate <>f__mg$cache3;

		// Token: 0x04003FD9 RID: 16345
		[CompilerGenerated]
		private static CallResult<CreateItemResult_t>.APIDispatchDelegate <>f__mg$cache4;

		// Token: 0x04003FDA RID: 16346
		[CompilerGenerated]
		private static CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate <>f__mg$cache5;

		// Token: 0x04003FDB RID: 16347
		[CompilerGenerated]
		private static CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate <>f__mg$cache6;

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06006183 RID: 24963 RVA: 0x00313FEC File Offset: 0x003123EC
		public static WorkshopInteractStage CurStage
		{
			get
			{
				return Workshop.curStage;
			}
		}

		// Token: 0x06006184 RID: 24964 RVA: 0x00314008 File Offset: 0x00312408
		internal static void Init()
		{
			if (Workshop.<>f__mg$cache0 == null)
			{
				Workshop.<>f__mg$cache0 = new Callback<RemoteStoragePublishedFileSubscribed_t>.DispatchDelegate(Workshop.OnItemSubscribed);
			}
			Workshop.subscribedCallback = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(Workshop.<>f__mg$cache0);
			if (Workshop.<>f__mg$cache1 == null)
			{
				Workshop.<>f__mg$cache1 = new Callback<ItemInstalled_t>.DispatchDelegate(Workshop.OnItemInstalled);
			}
			Workshop.installedCallback = Callback<ItemInstalled_t>.Create(Workshop.<>f__mg$cache1);
			if (Workshop.<>f__mg$cache2 == null)
			{
				Workshop.<>f__mg$cache2 = new Callback<RemoteStoragePublishedFileUnsubscribed_t>.DispatchDelegate(Workshop.OnItemUnsubscribed);
			}
			Workshop.unsubscribedCallback = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(Workshop.<>f__mg$cache2);
		}

		// Token: 0x06006185 RID: 24965 RVA: 0x0031408C File Offset: 0x0031248C
		internal static void Upload(WorkshopUploadable item)
		{
			if (Workshop.curStage != WorkshopInteractStage.None)
			{
				Messages.Message("UploadAlreadyInProgress".Translate(), MessageTypeDefOf.RejectInput, false);
			}
			else
			{
				Workshop.uploadingHook = item.GetWorkshopItemHook();
				if (Workshop.uploadingHook.PublishedFileId != PublishedFileId_t.Invalid)
				{
					if (Prefs.LogVerbose)
					{
						Log.Message(string.Concat(new object[]
						{
							"Workshop: Starting item update for mod '",
							Workshop.uploadingHook.Name,
							"' with PublishedFileId ",
							Workshop.uploadingHook.PublishedFileId
						}), false);
					}
					Workshop.curStage = WorkshopInteractStage.SubmittingItem;
					Workshop.curUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), Workshop.uploadingHook.PublishedFileId);
					Workshop.SetWorkshopItemDataFrom(Workshop.curUpdateHandle, Workshop.uploadingHook, false);
					SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(Workshop.curUpdateHandle, "[Auto-generated text]: Update on " + DateTime.Now.ToString() + ".");
					if (Workshop.<>f__mg$cache3 == null)
					{
						Workshop.<>f__mg$cache3 = new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(Workshop.OnItemSubmitted);
					}
					Workshop.submitResult = CallResult<SubmitItemUpdateResult_t>.Create(Workshop.<>f__mg$cache3);
					Workshop.submitResult.Set(hAPICall, null);
				}
				else
				{
					if (Prefs.LogVerbose)
					{
						Log.Message("Workshop: Starting item creation for mod '" + Workshop.uploadingHook.Name + "'.", false);
					}
					Workshop.curStage = WorkshopInteractStage.CreatingItem;
					SteamAPICall_t hAPICall2 = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeFirst);
					if (Workshop.<>f__mg$cache4 == null)
					{
						Workshop.<>f__mg$cache4 = new CallResult<CreateItemResult_t>.APIDispatchDelegate(Workshop.OnItemCreated);
					}
					Workshop.createResult = CallResult<CreateItemResult_t>.Create(Workshop.<>f__mg$cache4);
					Workshop.createResult.Set(hAPICall2, null);
				}
				Find.WindowStack.Add(new Dialog_WorkshopOperationInProgress());
			}
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x00314242 File Offset: 0x00312642
		internal static void Unsubscribe(WorkshopUploadable item)
		{
			SteamUGC.UnsubscribeItem(item.GetPublishedFileId());
		}

		// Token: 0x06006187 RID: 24967 RVA: 0x00314254 File Offset: 0x00312654
		internal static void RequestItemsDetails(PublishedFileId_t[] publishedFileIds)
		{
			if (Workshop.detailsQueryCount >= 0)
			{
				Log.Error("Requested Workshop item details while a details request was already pending.", false);
			}
			else
			{
				Workshop.detailsQueryCount = publishedFileIds.Length;
				Workshop.detailsQueryHandle = SteamUGC.CreateQueryUGCDetailsRequest(publishedFileIds, (uint)Workshop.detailsQueryCount);
				SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(Workshop.detailsQueryHandle);
				if (Workshop.<>f__mg$cache5 == null)
				{
					Workshop.<>f__mg$cache5 = new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(Workshop.OnGotItemDetails);
				}
				Workshop.requestDetailsResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(Workshop.<>f__mg$cache5);
				Workshop.requestDetailsResult.Set(hAPICall, null);
			}
		}

		// Token: 0x06006188 RID: 24968 RVA: 0x003142D4 File Offset: 0x003126D4
		internal static void OnItemSubscribed(RemoteStoragePublishedFileSubscribed_t result)
		{
			if (Workshop.IsOurAppId(result.m_nAppID))
			{
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Item subscribed: " + result.m_nPublishedFileId, false);
				}
				WorkshopItems.Notify_Subscribed(result.m_nPublishedFileId);
			}
		}

		// Token: 0x06006189 RID: 24969 RVA: 0x0031432C File Offset: 0x0031272C
		internal static void OnItemInstalled(ItemInstalled_t result)
		{
			if (Workshop.IsOurAppId(result.m_unAppID))
			{
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Item installed: " + result.m_nPublishedFileId, false);
				}
				WorkshopItems.Notify_Installed(result.m_nPublishedFileId);
			}
		}

		// Token: 0x0600618A RID: 24970 RVA: 0x00314384 File Offset: 0x00312784
		internal static void OnItemUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t result)
		{
			if (Workshop.IsOurAppId(result.m_nAppID))
			{
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Item unsubscribed: " + result.m_nPublishedFileId, false);
				}
				Page_ModsConfig page_ModsConfig = Find.WindowStack.WindowOfType<Page_ModsConfig>();
				if (page_ModsConfig != null)
				{
					page_ModsConfig.Notify_SteamItemUnsubscribed(result.m_nPublishedFileId);
				}
				Page_SelectScenario page_SelectScenario = Find.WindowStack.WindowOfType<Page_SelectScenario>();
				if (page_SelectScenario != null)
				{
					page_SelectScenario.Notify_SteamItemUnsubscribed(result.m_nPublishedFileId);
				}
				WorkshopItems.Notify_Unsubscribed(result.m_nPublishedFileId);
			}
		}

		// Token: 0x0600618B RID: 24971 RVA: 0x00314418 File Offset: 0x00312818
		private static void OnItemCreated(CreateItemResult_t result, bool IOFailure)
		{
			if (IOFailure || result.m_eResult != EResult.k_EResultOK)
			{
				Workshop.uploadingHook = null;
				Dialog_WorkshopOperationInProgress.CloseAll();
				Log.Error("Workshop: OnItemCreated failure. Result: " + result.m_eResult.GetLabel(), false);
				Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(new object[]
				{
					GenText.SplitCamelCase(result.m_eResult.GetLabel())
				}), null, null, null, null, null, false, null, null));
			}
			else
			{
				Workshop.uploadingHook.PublishedFileId = result.m_nPublishedFileId;
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Item created. PublishedFileId: " + Workshop.uploadingHook.PublishedFileId, false);
				}
				Workshop.curUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), Workshop.uploadingHook.PublishedFileId);
				Workshop.SetWorkshopItemDataFrom(Workshop.curUpdateHandle, Workshop.uploadingHook, true);
				Workshop.curStage = WorkshopInteractStage.SubmittingItem;
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Submitting item.", false);
				}
				SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(Workshop.curUpdateHandle, "[Auto-generated text]: Initial upload.");
				if (Workshop.<>f__mg$cache6 == null)
				{
					Workshop.<>f__mg$cache6 = new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(Workshop.OnItemSubmitted);
				}
				Workshop.submitResult = CallResult<SubmitItemUpdateResult_t>.Create(Workshop.<>f__mg$cache6);
				Workshop.submitResult.Set(hAPICall, null);
				Workshop.createResult = null;
			}
		}

		// Token: 0x0600618C RID: 24972 RVA: 0x00314568 File Offset: 0x00312968
		private static void OnItemSubmitted(SubmitItemUpdateResult_t result, bool IOFailure)
		{
			if (IOFailure || result.m_eResult != EResult.k_EResultOK)
			{
				Workshop.uploadingHook = null;
				Dialog_WorkshopOperationInProgress.CloseAll();
				Log.Error("Workshop: OnItemSubmitted failure. Result: " + result.m_eResult.GetLabel(), false);
				Find.WindowStack.Add(new Dialog_MessageBox("WorkshopSubmissionFailed".Translate(new object[]
				{
					GenText.SplitCamelCase(result.m_eResult.GetLabel())
				}), null, null, null, null, null, false, null, null));
			}
			else
			{
				SteamUtility.OpenWorkshopPage(Workshop.uploadingHook.PublishedFileId);
				Messages.Message("WorkshopUploadSucceeded".Translate(new object[]
				{
					Workshop.uploadingHook.Name
				}), MessageTypeDefOf.TaskCompletion, false);
				if (Prefs.LogVerbose)
				{
					Log.Message("Workshop: Item submit result: " + result.m_eResult, false);
				}
			}
			Workshop.curStage = WorkshopInteractStage.None;
			Workshop.submitResult = null;
		}

		// Token: 0x0600618D RID: 24973 RVA: 0x00314660 File Offset: 0x00312A60
		private static void OnGotItemDetails(SteamUGCRequestUGCDetailsResult_t result, bool IOFailure)
		{
			if (IOFailure)
			{
				Log.Error("Workshop: OnGotItemDetails IOFailure.", false);
				Workshop.detailsQueryCount = -1;
			}
			else
			{
				if (Workshop.detailsQueryCount < 0)
				{
					Log.Warning("Got unexpected Steam Workshop item details response.", false);
				}
				string text = "Steam Workshop Item details received:";
				for (int i = 0; i < Workshop.detailsQueryCount; i++)
				{
					SteamUGCDetails_t steamUGCDetails_t;
					SteamUGC.GetQueryUGCResult(Workshop.detailsQueryHandle, (uint)i, out steamUGCDetails_t);
					if (steamUGCDetails_t.m_eResult != EResult.k_EResultOK)
					{
						text = text + "\n  Query result: " + steamUGCDetails_t.m_eResult;
					}
					else
					{
						text = text + "\n  Title: " + steamUGCDetails_t.m_rgchTitle;
						text = text + "\n  PublishedFileId: " + steamUGCDetails_t.m_nPublishedFileId;
						text = text + "\n  Created: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeCreated)).ToString();
						text = text + "\n  Updated: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeUpdated)).ToString();
						text = text + "\n  Added to list: " + DateTime.FromFileTimeUtc((long)((ulong)steamUGCDetails_t.m_rtimeAddedToUserList)).ToString();
						text = text + "\n  File size: " + steamUGCDetails_t.m_nFileSize.ToStringKilobytes("F2");
						text = text + "\n  Preview size: " + steamUGCDetails_t.m_nPreviewFileSize.ToStringKilobytes("F2");
						text = text + "\n  File name: " + steamUGCDetails_t.m_pchFileName;
						text = text + "\n  CreatorAppID: " + steamUGCDetails_t.m_nCreatorAppID;
						text = text + "\n  ConsumerAppID: " + steamUGCDetails_t.m_nConsumerAppID;
						text = text + "\n  Visibiliy: " + steamUGCDetails_t.m_eVisibility;
						text = text + "\n  FileType: " + steamUGCDetails_t.m_eFileType;
						text = text + "\n  Owner: " + steamUGCDetails_t.m_ulSteamIDOwner;
					}
					text += "\n";
				}
				Log.Message(text.TrimEndNewlines(), false);
				Workshop.detailsQueryCount = -1;
			}
		}

		// Token: 0x0600618E RID: 24974 RVA: 0x00314880 File Offset: 0x00312C80
		public static void GetUpdateStatus(out EItemUpdateStatus updateStatus, out float progPercent)
		{
			ulong num;
			ulong num2;
			updateStatus = SteamUGC.GetItemUpdateProgress(Workshop.curUpdateHandle, out num, out num2);
			progPercent = num / num2;
		}

		// Token: 0x0600618F RID: 24975 RVA: 0x003148A8 File Offset: 0x00312CA8
		public static string UploadButtonLabel(PublishedFileId_t pfid)
		{
			return (!(pfid != PublishedFileId_t.Invalid)) ? "UploadToSteamWorkshop".Translate() : "UpdateOnSteamWorkshop".Translate();
		}

		// Token: 0x06006190 RID: 24976 RVA: 0x003148E8 File Offset: 0x00312CE8
		private static void SetWorkshopItemDataFrom(UGCUpdateHandle_t updateHandle, WorkshopItemHook hook, bool creating)
		{
			hook.PrepareForWorkshopUpload();
			SteamUGC.SetItemTitle(updateHandle, hook.Name);
			if (creating)
			{
				SteamUGC.SetItemDescription(updateHandle, hook.Description);
			}
			if (!File.Exists(hook.PreviewImagePath))
			{
				Log.Warning("Missing preview file at " + hook.PreviewImagePath, false);
			}
			else
			{
				SteamUGC.SetItemPreview(updateHandle, hook.PreviewImagePath);
			}
			IList<string> tags = hook.Tags;
			tags.Add(VersionControl.CurrentMajor + "." + VersionControl.CurrentMinor);
			SteamUGC.SetItemTags(updateHandle, tags);
			SteamUGC.SetItemContent(updateHandle, hook.Directory.FullName);
		}

		// Token: 0x06006191 RID: 24977 RVA: 0x0031499C File Offset: 0x00312D9C
		internal static IEnumerable<PublishedFileId_t> AllSubscribedItems()
		{
			uint numSub = SteamUGC.GetNumSubscribedItems();
			PublishedFileId_t[] subbedItems = new PublishedFileId_t[numSub];
			uint count = SteamUGC.GetSubscribedItems(subbedItems, numSub);
			int i = 0;
			while ((long)i < (long)((ulong)count))
			{
				PublishedFileId_t pfid = subbedItems[i];
				yield return pfid;
				i++;
			}
			yield break;
		}

		// Token: 0x06006192 RID: 24978 RVA: 0x003149C0 File Offset: 0x00312DC0
		[DebugOutput]
		[Category("System")]
		internal static void SteamWorkshopStatus()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("All subscribed items (" + SteamUGC.GetNumSubscribedItems() + " total):");
			List<PublishedFileId_t> list = Workshop.AllSubscribedItems().ToList<PublishedFileId_t>();
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.AppendLine("   " + Workshop.ItemStatusString(list[i]));
			}
			stringBuilder.AppendLine("All installed mods:");
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				stringBuilder.AppendLine("   " + modMetaData.Identifier + ": " + Workshop.ItemStatusString(modMetaData.GetPublishedFileId()));
			}
			Log.Message(stringBuilder.ToString(), false);
			List<PublishedFileId_t> list2 = Workshop.AllSubscribedItems().ToList<PublishedFileId_t>();
			PublishedFileId_t[] array = new PublishedFileId_t[list2.Count];
			for (int j = 0; j < list2.Count; j++)
			{
				array[j] = (PublishedFileId_t)list2[j].m_PublishedFileId;
			}
			Workshop.RequestItemsDetails(array);
		}

		// Token: 0x06006193 RID: 24979 RVA: 0x00314B2C File Offset: 0x00312F2C
		private static string ItemStatusString(PublishedFileId_t pfid)
		{
			string result;
			if (pfid == PublishedFileId_t.Invalid)
			{
				result = "[unpublished]";
			}
			else
			{
				string text = "[" + pfid + "] ";
				ulong num;
				string str;
				uint num2;
				if (SteamUGC.GetItemInstallInfo(pfid, out num, out str, 257u, out num2))
				{
					text += "\n      installed";
					text = text + "\n      folder=" + str;
					text = text + "\n      sizeOnDisk=" + (num / 1024f).ToString("F2") + "Kb";
				}
				else
				{
					text += "\n      not installed";
				}
				result = text;
			}
			return result;
		}

		// Token: 0x06006194 RID: 24980 RVA: 0x00314BE0 File Offset: 0x00312FE0
		private static bool IsOurAppId(AppId_t appId)
		{
			return !(appId != SteamUtils.GetAppID());
		}
	}
}
