using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class Dialog_FileList : Window
	{
		protected string interactButLabel = "Error";

		protected float bottomAreaHeight = 0f;

		protected List<SaveFileInfo> files = new List<SaveFileInfo>();

		protected Vector2 scrollPosition = Vector2.zero;

		protected string typingName = "";

		private bool focusedNameArea = false;

		protected const float BoxMargin = 20f;

		protected const float EntrySpacing = 3f;

		protected const float EntryMargin = 1f;

		protected const float NameExtraLeftMargin = 15f;

		protected const float InfoExtraLeftMargin = 270f;

		protected const float DeleteButtonSpace = 5f;

		protected const float EntryHeight = 36f;

		private static readonly Color DefaultFileTextColor = new Color(1f, 1f, 0.6f);

		protected const float NameTextFieldWidth = 400f;

		protected const float NameTextFieldHeight = 35f;

		protected const float NameTextFieldButtonSpace = 20f;

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 700f);
			}
		}

		protected virtual bool ShouldDoTypeInField
		{
			get
			{
				return false;
			}
		}

		public Dialog_FileList()
		{
			base.closeOnEscapeKey = true;
			base.doCloseButton = true;
			base.doCloseX = true;
			base.forcePause = true;
			base.absorbInputAroundWindow = true;
			this.ReloadFiles();
		}

		public override void DoWindowContents(Rect inRect)
		{
			Vector2 vector = new Vector2((float)(inRect.width - 16.0), 36f);
			Vector2 vector2 = new Vector2(100f, (float)(vector.y - 2.0));
			inRect.height -= 45f;
			float num = (float)(vector.y + 3.0);
			float height = (float)this.files.Count * num;
			Rect viewRect = new Rect(0f, 0f, (float)(inRect.width - 16.0), height);
			Rect outRect = new Rect(inRect.AtZero());
			outRect.height -= this.bottomAreaHeight;
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num2 = 0f;
			int num3 = 0;
			foreach (SaveFileInfo file in this.files)
			{
				Rect rect = new Rect(0f, num2, vector.x, vector.y);
				if (num3 % 2 == 0)
				{
					Widgets.DrawAltRect(rect);
				}
				Rect position = rect.ContractedBy(1f);
				GUI.BeginGroup(position);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileInfo.Name);
				GUI.color = this.FileNameColor(file);
				Rect rect2 = new Rect(15f, 0f, position.width, position.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				Text.Font = GameFont.Small;
				Widgets.Label(rect2, fileNameWithoutExtension);
				GUI.color = Color.white;
				Rect rect3 = new Rect(270f, 0f, 200f, position.height);
				Dialog_FileList.DrawDateAndVersion(file, rect3);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
				float num4 = (float)(vector.x - 2.0 - vector2.x - vector2.y);
				Rect rect4 = new Rect(num4, 0f, vector2.x, vector2.y);
				if (Widgets.ButtonText(rect4, this.interactButLabel, true, false, true))
				{
					this.DoFileInteraction(Path.GetFileNameWithoutExtension(file.FileInfo.Name));
				}
				Rect rect5 = new Rect((float)(num4 + vector2.x + 5.0), 0f, vector2.y, vector2.y);
				if (Widgets.ButtonImage(rect5, TexButton.DeleteX))
				{
					FileInfo localFile = file.FileInfo;
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDelete".Translate(localFile.Name), (Action)delegate
					{
						localFile.Delete();
						this.ReloadFiles();
					}, true, (string)null));
				}
				TooltipHandler.TipRegion(rect5, "DeleteThisSavegame".Translate());
				GUI.EndGroup();
				num2 = (float)(num2 + (vector.y + 3.0));
				num3++;
			}
			Widgets.EndScrollView();
			if (this.ShouldDoTypeInField)
			{
				this.DoTypeInField(inRect.AtZero());
			}
		}

		protected abstract void DoFileInteraction(string fileName);

		protected abstract void ReloadFiles();

		protected virtual void DoTypeInField(Rect rect)
		{
			GUI.BeginGroup(rect);
			bool flag = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return;
			float y = (float)(rect.height - 52.0);
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
			Rect rect3 = new Rect(420f, y, (float)(rect.width - 400.0 - 20.0), 35f);
			if (Widgets.ButtonText(rect3, "SaveGameButton".Translate(), true, false, true) || flag)
			{
				if (this.typingName.NullOrEmpty())
				{
					Messages.Message("NeedAName".Translate(), MessageTypeDefOf.RejectInput);
				}
				else
				{
					this.DoFileInteraction(this.typingName);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
		}

		protected virtual Color FileNameColor(SaveFileInfo sfi)
		{
			return Dialog_FileList.DefaultFileTextColor;
		}

		public static void DrawDateAndVersion(SaveFileInfo sfi, Rect rect)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(0f, 2f, rect.width, (float)(rect.height / 2.0));
			GUI.color = SaveFileInfo.UnimportantTextColor;
			Widgets.Label(rect2, sfi.FileInfo.LastWriteTime.ToString("g"));
			Rect rect3 = new Rect(0f, rect2.yMax, rect.width, (float)(rect.height / 2.0));
			GUI.color = sfi.VersionColor;
			Widgets.Label(rect3, sfi.GameVersion);
			TooltipHandler.TipRegion(rect3, sfi.CompatibilityTip);
			GUI.EndGroup();
		}
	}
}
