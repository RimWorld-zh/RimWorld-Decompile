using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	public class Page_ModsConfig : Page
	{
		public ModMetaData selectedMod;

		private Vector2 modListScrollPosition = Vector2.zero;

		private Vector2 modDescriptionScrollPosition = Vector2.zero;

		private int activeModsWhenOpenedHash = -1;

		private Dictionary<string, string> truncatedModNamesCache = new Dictionary<string, string>();

		private const float ModListAreaWidth = 350f;

		private const float ModsListButtonHeight = 30f;

		private const float ModsFolderButHeight = 30f;

		private const float ButtonsGap = 4f;

		private const float UploadRowHeight = 40f;

		private const float PreviewMaxHeight = 300f;

		private const float VersionWidth = 30f;

		private const float ModRowHeight = 26f;

		public Page_ModsConfig()
		{
			base.doCloseButton = true;
		}

		public override void PreOpen()
		{
			base.PreOpen();
			ModLister.RebuildModList();
			this.selectedMod = this.ModsInListOrder().FirstOrDefault();
			this.activeModsWhenOpenedHash = ModLister.InstalledModsListHash(true);
		}

		private IEnumerable<ModMetaData> ModsInListOrder()
		{
			using (IEnumerator<ModMetaData> enumerator = ModsConfig.ActiveModsInLoadOrder.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					ModMetaData mod2 = enumerator.Current;
					yield return mod2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			using (IEnumerator<ModMetaData> enumerator2 = (from x in ModLister.AllInstalledMods
			where !x.Active
			select x into m
			orderby m.VersionCompatible descending
			select m).GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					ModMetaData mod = enumerator2.Current;
					yield return mod;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_0182:
			/*Error near IL_0183: Unexpected return in MoveNext()*/;
		}

		public override void DoWindowContents(Rect rect)
		{
			Rect mainRect = base.GetMainRect(rect, 0f, true);
			GUI.BeginGroup(mainRect);
			Text.Font = GameFont.Small;
			float num = 0f;
			Rect rect2 = new Rect(17f, num, 316f, 30f);
			if (Widgets.ButtonText(rect2, "OpenSteamWorkshop".Translate(), true, false, true))
			{
				SteamUtility.OpenSteamWorkshopPage();
			}
			num = (float)(num + 30.0);
			Rect rect3 = new Rect(17f, num, 316f, 30f);
			if (Widgets.ButtonText(rect3, "GetModsFromForum".Translate(), true, false, true))
			{
				Application.OpenURL("http://rimworldgame.com/getmods");
			}
			num = (float)(num + 30.0);
			num = (float)(num + 17.0);
			Rect rect4 = new Rect(0f, num, 350f, mainRect.height - num);
			Widgets.DrawMenuSection(rect4);
			float height = (float)((float)ModLister.AllInstalledMods.Count() * 26.0 + 8.0);
			Rect rect5 = new Rect(0f, 0f, (float)(rect4.width - 16.0), height);
			Widgets.BeginScrollView(rect4, ref this.modListScrollPosition, rect5, true);
			Rect rect6 = rect5.ContractedBy(4f);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = rect6.width;
			listing_Standard.Begin(rect6);
			int reorderableGroup = ReorderableWidget.NewGroup(delegate(int from, int to)
			{
				ModsConfig.Reorder(from, to);
			});
			int num2 = 0;
			foreach (ModMetaData item in this.ModsInListOrder())
			{
				this.DoModRow(listing_Standard, item, num2, reorderableGroup);
				num2++;
			}
			int downloadingItemsCount = WorkshopItems.DownloadingItemsCount;
			for (int i = 0; i < downloadingItemsCount; i++)
			{
				this.DoModRowDownloading(listing_Standard, num2);
				num2++;
			}
			listing_Standard.End();
			Widgets.EndScrollView();
			Rect position = new Rect((float)(rect4.xMax + 17.0), 0f, (float)(mainRect.width - rect4.width - 17.0), mainRect.height);
			GUI.BeginGroup(position);
			if (this.selectedMod != null)
			{
				Text.Font = GameFont.Medium;
				Rect rect7 = new Rect(0f, 0f, position.width, 40f);
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(rect7, this.selectedMod.Name.Truncate(rect7.width, null));
				Text.Anchor = TextAnchor.UpperLeft;
				if (!this.selectedMod.IsCoreMod)
				{
					Rect rect8 = rect7;
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.LowerRight;
					if (!this.selectedMod.VersionCompatible)
					{
						GUI.color = Color.red;
					}
					Widgets.Label(rect8, "ModTargetVersion".Translate(this.selectedMod.TargetVersion));
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
					Text.Font = GameFont.Small;
				}
				Rect position2 = new Rect(0f, rect7.yMax, 0f, 20f);
				if ((UnityEngine.Object)this.selectedMod.previewImage != (UnityEngine.Object)null)
				{
					position2.width = Mathf.Min((float)this.selectedMod.previewImage.width, position.width);
					position2.height = (float)this.selectedMod.previewImage.height * (position2.width / (float)this.selectedMod.previewImage.width);
					if (position2.height > 300.0)
					{
						position2.width *= (float)(300.0 / position2.height);
						position2.height = 300f;
					}
					position2.x = (float)(position.width / 2.0 - position2.width / 2.0);
					GUI.DrawTexture(position2, this.selectedMod.previewImage, ScaleMode.ScaleToFit);
				}
				Text.Font = GameFont.Small;
				float num3 = (float)(position2.yMax + 10.0);
				if (!this.selectedMod.Author.NullOrEmpty())
				{
					Rect rect9 = new Rect(0f, num3, (float)(position.width / 2.0), 25f);
					Widgets.Label(rect9, "Author".Translate() + ": " + this.selectedMod.Author);
				}
				if (!this.selectedMod.Url.NullOrEmpty())
				{
					double a = position.width / 2.0;
					Vector2 vector = Text.CalcSize(this.selectedMod.Url);
					float num4 = Mathf.Min((float)a, vector.x);
					Rect rect10 = new Rect(position.width - num4, num3, num4, 25f);
					Text.WordWrap = false;
					if (Widgets.ButtonText(rect10, this.selectedMod.Url, false, false, true))
					{
						Application.OpenURL(this.selectedMod.Url);
					}
					Text.WordWrap = true;
				}
				WidgetRow widgetRow = new WidgetRow(position.width, (float)(num3 + 25.0), UIDirection.LeftThenUp, 99999f, 4f);
				if (SteamManager.Initialized && this.selectedMod.OnSteamWorkshop)
				{
					if (widgetRow.ButtonText("Unsubscribe", null, true, false))
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmUnsubscribe".Translate(this.selectedMod.Name), delegate
						{
							this.selectedMod.enabled = false;
							Workshop.Unsubscribe(this.selectedMod);
							this.Notify_SteamItemUnsubscribed(this.selectedMod.GetPublishedFileId());
						}, true, null));
					}
					if (widgetRow.ButtonText("WorkshopPage".Translate(), null, true, false))
					{
						SteamUtility.OpenWorkshopPage(this.selectedMod.GetPublishedFileId());
					}
				}
				float num5 = (float)(num3 + 25.0 + 24.0);
				Rect outRect = new Rect(0f, num5, position.width, (float)(position.height - num5 - 40.0));
				float width = (float)(outRect.width - 16.0);
				Rect rect11 = new Rect(0f, 0f, width, Text.CalcHeight(this.selectedMod.Description, width));
				Widgets.BeginScrollView(outRect, ref this.modDescriptionScrollPosition, rect11, true);
				Widgets.Label(rect11, this.selectedMod.Description);
				Widgets.EndScrollView();
				if (Prefs.DevMode && SteamManager.Initialized && this.selectedMod.CanToUploadToWorkshop())
				{
					Rect rect12 = new Rect(0f, (float)(position.yMax - 40.0), 200f, 40f);
					if (Widgets.ButtonText(rect12, Workshop.UploadButtonLabel(this.selectedMod.GetPublishedFileId()), true, false, true))
					{
						if (!VersionControl.IsWellFormattedVersionString(this.selectedMod.TargetVersion))
						{
							Messages.Message("MessageModNeedsWellFormattedTargetVersion".Translate(VersionControl.CurrentVersionString), MessageTypeDefOf.RejectInput);
						}
						else
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSteamWorkshopUpload".Translate(), delegate
							{
								SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
								Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation("ConfirmContentAuthor".Translate(), delegate
								{
									SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
									Workshop.Upload(this.selectedMod);
								}, true, null);
								dialog_MessageBox.buttonAText = "Yes".Translate();
								dialog_MessageBox.buttonBText = "No".Translate();
								dialog_MessageBox.interactionDelay = 6f;
								Find.WindowStack.Add(dialog_MessageBox);
							}, true, null));
						}
					}
				}
			}
			GUI.EndGroup();
			GUI.EndGroup();
		}

		private void DoModRow(Listing_Standard listing, ModMetaData mod, int index, int reorderableGroup)
		{
			Rect rect = listing.GetRect(26f);
			if (mod.Active)
			{
				ReorderableWidget.Reorderable(reorderableGroup, rect);
			}
			Action clickAction = null;
			if (mod.Source == ContentSource.SteamWorkshop)
			{
				clickAction = delegate
				{
					SteamUtility.OpenWorkshopPage(mod.GetPublishedFileId());
				};
			}
			ContentSourceUtility.DrawContentSource(rect, mod.Source, clickAction);
			rect.xMin += 28f;
			bool flag = mod == this.selectedMod;
			bool active = mod.Active;
			Rect rect2 = rect;
			if (mod.enabled)
			{
				string text = string.Empty;
				if (mod.Active)
				{
					text = text + "DragToReorder".Translate() + ".\n\n";
				}
				if (!mod.VersionCompatible)
				{
					GUI.color = Color.red;
					text += "ModNotMadeForThisVersion".Translate();
				}
				if (!text.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect2, new TipSignal(text, mod.GetHashCode() * 3311));
				}
				float num = (float)(rect2.width - 24.0);
				if (mod.Active)
				{
					Rect position = new Rect((float)(rect2.xMax - 48.0 + 2.0), rect2.y, 24f, 24f);
					GUI.DrawTexture(position, TexButton.DragHash);
					num = (float)(num - 24.0);
				}
				Text.Font = GameFont.Small;
				string label = mod.Name.Truncate(num, this.truncatedModNamesCache);
				if (Widgets.CheckboxLabeledSelectable(rect2, label, ref flag, ref active))
				{
					this.selectedMod = mod;
				}
				if (mod.Active && !active && mod.IsCoreMod)
				{
					ModMetaData coreMod = mod;
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDisableCoreMod".Translate(), delegate
					{
						coreMod.Active = false;
						this.truncatedModNamesCache.Clear();
					}, false, null));
				}
				else
				{
					mod.Active = active;
					this.truncatedModNamesCache.Clear();
				}
			}
			else
			{
				GUI.color = Color.gray;
				Widgets.Label(rect2, mod.Name);
			}
			GUI.color = Color.white;
		}

		private void DoModRowDownloading(Listing_Standard listing, int index)
		{
			Rect rect = listing.GetRect(26f);
			ContentSourceUtility.DrawContentSource(rect, ContentSource.SteamWorkshop, null);
			rect.xMin += 28f;
			Widgets.Label(rect, "Downloading".Translate() + GenText.MarchingEllipsis(0f));
		}

		public void Notify_ModsListChanged()
		{
			string selModId = this.selectedMod.Identifier;
			this.selectedMod = ModLister.AllInstalledMods.FirstOrDefault((ModMetaData m) => m.Identifier == selModId);
		}

		internal void Notify_SteamItemUnsubscribed(PublishedFileId_t pfid)
		{
			if (this.selectedMod != null && this.selectedMod.Identifier == pfid.ToString())
			{
				this.selectedMod = null;
			}
		}

		public override void PostClose()
		{
			ModsConfig.Save();
			if (this.activeModsWhenOpenedHash != ModLister.InstalledModsListHash(true))
			{
				ModsConfig.RestartFromChangedMods();
			}
		}
	}
}
