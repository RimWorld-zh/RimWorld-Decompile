using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000810 RID: 2064
	public abstract class Dialog_FileList : Window
	{
		// Token: 0x06002E0F RID: 11791 RVA: 0x001842CC File Offset: 0x001826CC
		public Dialog_FileList()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.ReloadFiles();
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002E10 RID: 11792 RVA: 0x00184348 File Offset: 0x00182748
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 700f);
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002E11 RID: 11793 RVA: 0x0018436C File Offset: 0x0018276C
		protected virtual bool ShouldDoTypeInField
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x00184384 File Offset: 0x00182784
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

		// Token: 0x06002E13 RID: 11795
		protected abstract void DoFileInteraction(string fileName);

		// Token: 0x06002E14 RID: 11796
		protected abstract void ReloadFiles();

		// Token: 0x06002E15 RID: 11797 RVA: 0x00184718 File Offset: 0x00182B18
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

		// Token: 0x06002E16 RID: 11798 RVA: 0x00184854 File Offset: 0x00182C54
		protected virtual Color FileNameColor(SaveFileInfo sfi)
		{
			return Dialog_FileList.DefaultFileTextColor;
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x00184870 File Offset: 0x00182C70
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

		// Token: 0x04001874 RID: 6260
		protected string interactButLabel = "Error";

		// Token: 0x04001875 RID: 6261
		protected float bottomAreaHeight = 0f;

		// Token: 0x04001876 RID: 6262
		protected List<SaveFileInfo> files = new List<SaveFileInfo>();

		// Token: 0x04001877 RID: 6263
		protected Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001878 RID: 6264
		protected string typingName = "";

		// Token: 0x04001879 RID: 6265
		private bool focusedNameArea = false;

		// Token: 0x0400187A RID: 6266
		protected const float BoxMargin = 20f;

		// Token: 0x0400187B RID: 6267
		protected const float EntrySpacing = 3f;

		// Token: 0x0400187C RID: 6268
		protected const float EntryMargin = 1f;

		// Token: 0x0400187D RID: 6269
		protected const float NameExtraLeftMargin = 15f;

		// Token: 0x0400187E RID: 6270
		protected const float InfoExtraLeftMargin = 270f;

		// Token: 0x0400187F RID: 6271
		protected const float DeleteButtonSpace = 5f;

		// Token: 0x04001880 RID: 6272
		protected const float EntryHeight = 36f;

		// Token: 0x04001881 RID: 6273
		private static readonly Color DefaultFileTextColor = new Color(1f, 1f, 0.6f);

		// Token: 0x04001882 RID: 6274
		protected const float NameTextFieldWidth = 400f;

		// Token: 0x04001883 RID: 6275
		protected const float NameTextFieldHeight = 35f;

		// Token: 0x04001884 RID: 6276
		protected const float NameTextFieldButtonSpace = 20f;
	}
}
