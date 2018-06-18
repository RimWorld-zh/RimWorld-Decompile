using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E64 RID: 3684
	[StaticConstructorOnStartup]
	public abstract class Command : Gizmo
	{
		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x060056AB RID: 22187 RVA: 0x00159FE0 File Offset: 0x001583E0
		public virtual string Label
		{
			get
			{
				return this.defaultLabel;
			}
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x060056AC RID: 22188 RVA: 0x00159FFC File Offset: 0x001583FC
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x060056AD RID: 22189 RVA: 0x0015A01C File Offset: 0x0015841C
		public virtual string Desc
		{
			get
			{
				return this.defaultDesc;
			}
		}

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x060056AE RID: 22190 RVA: 0x0015A038 File Offset: 0x00158438
		public virtual Color IconDrawColor
		{
			get
			{
				return this.defaultIconColor;
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x060056AF RID: 22191 RVA: 0x0015A054 File Offset: 0x00158454
		public virtual SoundDef CurActivateSound
		{
			get
			{
				return this.activateSound;
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x060056B0 RID: 22192 RVA: 0x0015A070 File Offset: 0x00158470
		protected virtual bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x060056B1 RID: 22193 RVA: 0x0015A088 File Offset: 0x00158488
		public virtual string HighlightTag
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x060056B2 RID: 22194 RVA: 0x0015A0A4 File Offset: 0x001584A4
		public virtual string TutorTagSelect
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x060056B3 RID: 22195 RVA: 0x0015A0C0 File Offset: 0x001584C0
		public override float GetWidth(float maxWidth)
		{
			return 75f;
		}

		// Token: 0x060056B4 RID: 22196 RVA: 0x0015A0DC File Offset: 0x001584DC
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			Text.Font = GameFont.Tiny;
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			bool flag = false;
			if (Mouse.IsOver(rect))
			{
				flag = true;
				GUI.color = GenUI.MouseoverColor;
			}
			Texture2D badTex = this.icon;
			if (badTex == null)
			{
				badTex = BaseContent.BadTex;
			}
			Material mat = (!this.disabled) ? null : TexUI.GrayscaleGUI;
			Graphics.DrawTexture(rect, Command.BGTex, mat);
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Command);
			Rect outerRect = rect;
			outerRect.position += new Vector2(this.iconOffset.x * outerRect.size.x, this.iconOffset.y * outerRect.size.y);
			GUI.color = this.IconDrawColor;
			Widgets.DrawTextureFitted(outerRect, badTex, this.iconDrawScale * 0.85f, this.iconProportions, this.iconTexCoords, this.iconAngle, mat);
			GUI.color = Color.white;
			bool flag2 = false;
			KeyCode keyCode = (this.hotKey != null) ? this.hotKey.MainKey : KeyCode.None;
			if (keyCode != KeyCode.None && !GizmoGridDrawer.drawnHotKeys.Contains(keyCode))
			{
				Rect rect2 = new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, 18f);
				Widgets.Label(rect2, keyCode.ToStringReadable());
				GizmoGridDrawer.drawnHotKeys.Add(keyCode);
				if (this.hotKey.KeyDownEvent)
				{
					flag2 = true;
					Event.current.Use();
				}
			}
			if (Widgets.ButtonInvisible(rect, false))
			{
				flag2 = true;
			}
			string labelCap = this.LabelCap;
			if (!labelCap.NullOrEmpty())
			{
				float num = Text.CalcHeight(labelCap, rect.width);
				Rect rect3 = new Rect(rect.x, rect.yMax - num + 12f, rect.width, num);
				GUI.DrawTexture(rect3, TexUI.GrayTextBG);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(rect3, labelCap);
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
			GUI.color = Color.white;
			if (this.DoTooltip)
			{
				TipSignal tip = this.Desc;
				if (this.disabled && !this.disabledReason.NullOrEmpty())
				{
					string text = tip.text;
					tip.text = string.Concat(new string[]
					{
						text,
						"\n\n",
						"DisabledCommand".Translate(),
						": ",
						this.disabledReason
					});
				}
				TooltipHandler.TipRegion(rect, tip);
			}
			if (!this.HighlightTag.NullOrEmpty() && (Find.WindowStack.FloatMenu == null || !Find.WindowStack.FloatMenu.windowRect.Overlaps(rect)))
			{
				UIHighlighter.HighlightOpportunity(rect, this.HighlightTag);
			}
			Text.Font = GameFont.Small;
			GizmoResult result;
			if (flag2)
			{
				if (this.disabled)
				{
					if (!this.disabledReason.NullOrEmpty())
					{
						Messages.Message(this.disabledReason, MessageTypeDefOf.RejectInput, false);
					}
					result = new GizmoResult(GizmoState.Mouseover, null);
				}
				else
				{
					GizmoResult gizmoResult;
					if (Event.current.button == 1)
					{
						gizmoResult = new GizmoResult(GizmoState.OpenedFloatMenu, Event.current);
					}
					else
					{
						if (!TutorSystem.AllowAction(this.TutorTagSelect))
						{
							return new GizmoResult(GizmoState.Mouseover, null);
						}
						gizmoResult = new GizmoResult(GizmoState.Interacted, Event.current);
						TutorSystem.Notify_Event(this.TutorTagSelect);
					}
					result = gizmoResult;
				}
			}
			else if (flag)
			{
				result = new GizmoResult(GizmoState.Mouseover, null);
			}
			else
			{
				result = new GizmoResult(GizmoState.Clear, null);
			}
			return result;
		}

		// Token: 0x060056B5 RID: 22197 RVA: 0x0015A4E8 File Offset: 0x001588E8
		public override bool GroupsWith(Gizmo other)
		{
			Command command = other as Command;
			return command != null && ((this.hotKey == command.hotKey && this.Label == command.Label && this.icon == command.icon) || (this.groupKey != 0 && command.groupKey != 0 && this.groupKey == command.groupKey));
		}

		// Token: 0x060056B6 RID: 22198 RVA: 0x0015A58C File Offset: 0x0015898C
		public override void ProcessInput(Event ev)
		{
			if (this.CurActivateSound != null)
			{
				this.CurActivateSound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060056B7 RID: 22199 RVA: 0x0015A5A8 File Offset: 0x001589A8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Command(label=",
				this.defaultLabel,
				", defaultDesc=",
				this.defaultDesc,
				")"
			});
		}

		// Token: 0x0400396C RID: 14700
		public string defaultLabel = null;

		// Token: 0x0400396D RID: 14701
		public string defaultDesc = "No description.";

		// Token: 0x0400396E RID: 14702
		public Texture2D icon = null;

		// Token: 0x0400396F RID: 14703
		public float iconAngle;

		// Token: 0x04003970 RID: 14704
		public Vector2 iconProportions = Vector2.one;

		// Token: 0x04003971 RID: 14705
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04003972 RID: 14706
		public float iconDrawScale = 1f;

		// Token: 0x04003973 RID: 14707
		public Vector2 iconOffset;

		// Token: 0x04003974 RID: 14708
		public Color defaultIconColor = Color.white;

		// Token: 0x04003975 RID: 14709
		public KeyBindingDef hotKey;

		// Token: 0x04003976 RID: 14710
		public SoundDef activateSound = null;

		// Token: 0x04003977 RID: 14711
		public int groupKey = 0;

		// Token: 0x04003978 RID: 14712
		public string tutorTag = "TutorTagNotSet";

		// Token: 0x04003979 RID: 14713
		public static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);
	}
}
