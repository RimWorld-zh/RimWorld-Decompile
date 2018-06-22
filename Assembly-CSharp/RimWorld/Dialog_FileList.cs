using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080C RID: 2060
	public abstract class Dialog_FileList : Window
	{
		// Token: 0x06002E0A RID: 11786 RVA: 0x00184538 File Offset: 0x00182938
		public Dialog_FileList()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.ReloadFiles();
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002E0B RID: 11787 RVA: 0x001845B4 File Offset: 0x001829B4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 700f);
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002E0C RID: 11788 RVA: 0x001845D8 File Offset: 0x001829D8
		protected virtual bool ShouldDoTypeInField
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x001845F0 File Offset: 0x001829F0
		public override void DoWindowContents(Rect inRect)
		{
			Vector2 vector = new Vector2(inRect.width - 16f, 36f);
			Vector2 vector2 = new Vector2(100f, vector.y - 2f);
			inRect.height -= 45f;
			float num = vector.y + 3f;
			float height = (float)this.files.Count * num;
			Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, height);
			Rect outRect = new Rect(inRect.AtZero());
			outRect.height -= this.bottomAreaHeight;
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num2 = 0f;
			int num3 = 0;
			foreach (SaveFileInfo sfi in this.files)
			{
				if (num2 + vector.y >= this.scrollPosition.y && num2 <= this.scrollPosition.y + outRect.height)
				{
					Rect rect = new Rect(0f, num2, vector.x, vector.y);
					if (num3 % 2 == 0)
					{
						Widgets.DrawAltRect(rect);
					}
					Rect position = rect.ContractedBy(1f);
					GUI.BeginGroup(position);
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sfi.FileInfo.Name);
					GUI.color = this.FileNameColor(sfi);
					Rect rect2 = new Rect(15f, 0f, position.width, position.height);
					Text.Anchor = TextAnchor.MiddleLeft;
					Text.Font = GameFont.Small;
					Widgets.Label(rect2, fileNameWithoutExtension);
					GUI.color = Color.white;
					Rect rect3 = new Rect(270f, 0f, 200f, position.height);
					Dialog_FileList.DrawDateAndVersion(sfi, rect3);
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
					Text.Font = GameFont.Small;
					float num4 = vector.x - 2f - vector2.x - vector2.y;
					Rect rect4 = new Rect(num4, 0f, vector2.x, vector2.y);
					if (Widgets.ButtonText(rect4, this.interactButLabel, true, false, true))
					{
						this.DoFileInteraction(Path.GetFileNameWithoutExtension(sfi.FileInfo.Name));
					}
					Rect rect5 = new Rect(num4 + vector2.x + 5f, 0f, vector2.y, vector2.y);
					if (Widgets.ButtonImage(rect5, TexButton.DeleteX, Color.white, GenUI.SubtleMouseoverColor))
					{
						FileInfo localFile = sfi.FileInfo;
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDelete".Translate(new object[]
						{
							localFile.Name
						}), delegate
						{
							localFile.Delete();
							this.ReloadFiles();
						}, true, null));
					}
					TooltipHandler.TipRegion(rect5, "DeleteThisSavegame".Translate());
					GUI.EndGroup();
				}
				num2 += vector.y + 3f;
				num3++;
			}
			Widgets.EndScrollView();
			if (this.ShouldDoTypeInField)
			{
				this.DoTypeInField(inRect.AtZero());
			}
		}

		// Token: 0x06002E0E RID: 11790
		protected abstract void DoFileInteraction(string fileName);

		// Token: 0x06002E0F RID: 11791
		protected abstract void ReloadFiles();

		// Token: 0x06002E10 RID: 11792 RVA: 0x00184984 File Offset: 0x00182D84
		protected virtual void DoTypeInField(Rect rect)
		{
			GUI.BeginGroup(rect);
			bool flag = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return;
			float y = rect.height - 52f;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.SetNextControlName("MapNameField");
			Rect rect2 = new Rect(5f, y, 400f, 35f);
			string str = Widgets.TextField(rect2, this.typingName);
			if (GenText.IsValidFilename(str))
			{
				this.typingName = str;
			}
			if (!this.focusedNameArea)
			{
				UI.FocusControl("MapNameField", this);
				this.focusedNameArea = true;
			}
			Rect rect3 = new Rect(420f, y, rect.width - 400f - 20f, 35f);
			if (Widgets.ButtonText(rect3, "SaveGameButton".Translate(), true, false, true) || flag)
			{
				if (this.typingName.NullOrEmpty())
				{
					Messages.Message("NeedAName".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					this.DoFileInteraction(this.typingName);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x00184AC0 File Offset: 0x00182EC0
		protected virtual Color FileNameColor(SaveFileInfo sfi)
		{
			return Dialog_FileList.DefaultFileTextColor;
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x00184ADC File Offset: 0x00182EDC
		public static void DrawDateAndVersion(SaveFileInfo sfi, Rect rect)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(0f, 2f, rect.width, rect.height / 2f);
			GUI.color = SaveFileInfo.UnimportantTextColor;
			Widgets.Label(rect2, sfi.FileInfo.LastWriteTime.ToString("g"));
			Rect rect3 = new Rect(0f, rect2.yMax, rect.width, rect.height / 2f);
			GUI.color = sfi.VersionColor;
			Widgets.Label(rect3, sfi.GameVersion);
			TooltipHandler.TipRegion(rect3, sfi.CompatibilityTip);
			GUI.EndGroup();
		}

		// Token: 0x04001872 RID: 6258
		protected string interactButLabel = "Error";

		// Token: 0x04001873 RID: 6259
		protected float bottomAreaHeight = 0f;

		// Token: 0x04001874 RID: 6260
		protected List<SaveFileInfo> files = new List<SaveFileInfo>();

		// Token: 0x04001875 RID: 6261
		protected Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001876 RID: 6262
		protected string typingName = "";

		// Token: 0x04001877 RID: 6263
		private bool focusedNameArea = false;

		// Token: 0x04001878 RID: 6264
		protected const float BoxMargin = 20f;

		// Token: 0x04001879 RID: 6265
		protected const float EntrySpacing = 3f;

		// Token: 0x0400187A RID: 6266
		protected const float EntryMargin = 1f;

		// Token: 0x0400187B RID: 6267
		protected const float NameExtraLeftMargin = 15f;

		// Token: 0x0400187C RID: 6268
		protected const float InfoExtraLeftMargin = 270f;

		// Token: 0x0400187D RID: 6269
		protected const float DeleteButtonSpace = 5f;

		// Token: 0x0400187E RID: 6270
		protected const float EntryHeight = 36f;

		// Token: 0x0400187F RID: 6271
		private static readonly Color DefaultFileTextColor = new Color(1f, 1f, 0.6f);

		// Token: 0x04001880 RID: 6272
		protected const float NameTextFieldWidth = 400f;

		// Token: 0x04001881 RID: 6273
		protected const float NameTextFieldHeight = 35f;

		// Token: 0x04001882 RID: 6274
		protected const float NameTextFieldButtonSpace = 20f;
	}
}
