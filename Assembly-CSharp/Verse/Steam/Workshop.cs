using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	[HasDebugOutput]
	public static class Workshop
	{
		private static WorkshopItemHook uploadingHook;

		private static UGCUpdateHandle_t curUpdateHandle;

		private static WorkshopInteractStage curStage = WorkshopInteractStage.None;

		private static Callback<RemoteStoragePublishedFileSubscribed_t> subscribedCallback;

		private static Callback<RemoteStoragePublishedFileUnsubscribed_t> unsubscribedCallback;

		private static Callback<ItemInstalled_t> installedCallback;

		private static CallResult<SubmitItemUpdateResult_t> submitResult;

		private static CallResult<CreateItemResult_t> createResult;

		private static CallResult<SteamUGCRequestUGCDetailsResult_t> requestDetailsResult;

		private static UGCQueryHandle_t detailsQueryHandle;

		private static int detailsQueryCount = -1;

		public const uint InstallInfoFolderNameMaxLength = 257u;

		[CompilerGenerated]
		private static Callback<RemoteStoragePublishedFileSubscribed_t>.DispatchDelegate <>f__mg$cache0;

		[CompilerGenerated]
		private static Callback<ItemInstalled_t>.DispatchDelegate <>f__mg$cache1;

		[CompilerGenerated]
		private static Callback<RemoteStoragePublishedFileUnsubscribed_t>.DispatchDelegate <>f__mg$cache2;

		[CompilerGenerated]
		private static CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate <>f__mg$cache3;

		[CompilerGenerated]
		private static CallResult<CreateItemResult_t>.APIDispatchDelegate <>f__mg$cache4;

		[CompilerGenerated]
		private static CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate <>f__mg$cache5;

		[CompilerGenerated]
		private static CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate <>f__mg$cache6;

		public static WorkshopInteractStage CurStage
		{
			get
			{
				return Workshop.curStage;
			}
		}

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

		internal static void Unsubscribe(WorkshopUploadable item)
		{
			SteamUGC.UnsubscribeItem(item.GetPublishedFileId());
		}

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

		public static void GetUpdateStatus(out EItemUpdateStatus updateStatus, out float progPercent)
		{
			ulong num;
			ulong num2;
			updateStatus = SteamUGC.GetItemUpdateProgress(Workshop.curUpdateHandle, out num, out num2);
			progPercent = num / num2;
		}

		public static string UploadButtonLabel(PublishedFileId_t pfid)
		{
			return (!(pfid != PublishedFileId_t.Invalid)) ? "UploadToSteamWorkshop".Translate() : "UpdateOnSteamWorkshop".Translate();
		}

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

		[Category("System")]
		[DebugOutput]
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

		private static bool IsOurAppId(AppId_t appId)
		{
			return !(appId != SteamUtils.GetAppID());
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Workshop()
		{
		}

		[CompilerGenerated]
		private sealed class <AllSubscribedItems>c__Iterator0 : IEnumerable, IEnumerable<PublishedFileId_t>, IEnumerator, IDisposable, IEnumerator<PublishedFileId_t>
		{
			internal uint <numSub>__0;

			internal PublishedFileId_t[] <subbedItems>__0;

			internal uint <count>__0;

			internal int <i>__1;

			internal PublishedFileId_t <pfid>__2;

			internal PublishedFileId_t $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllSubscribedItems>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					numSub = SteamUGC.GetNumSubscribedItems();
					subbedItems = new PublishedFileId_t[numSub];
					count = SteamUGC.GetSubscribedItems(subbedItems, numSub);
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if ((long)i < (long)((ulong)count))
				{
					pfid = subbedItems[i];
					this.$current = pfid;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			PublishedFileId_t IEnumerator<PublishedFileId_t>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Steamworks.PublishedFileId_t>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<PublishedFileId_t> IEnumerable<PublishedFileId_t>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new Workshop.<AllSubscribedItems>c__Iterator0();
			}
		}
	}
}
