using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E66 RID: 3686
	[StaticConstructorOnStartup]
	public abstract class Command : Gizmo
	{
		// Token: 0x04003983 RID: 14723
		public string defaultLabel = null;

		// Token: 0x04003984 RID: 14724
		public string defaultDesc = "No description.";

		// Token: 0x04003985 RID: 14725
		public Texture2D icon = null;

		// Token: 0x04003986 RID: 14726
		public float iconAngle;

		// Token: 0x04003987 RID: 14727
		public Vector2 iconProportions = Vector2.one;

		// Token: 0x04003988 RID: 14728
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04003989 RID: 14729
		public float iconDrawScale = 1f;

		// Token: 0x0400398A RID: 14730
		public Vector2 iconOffset;

		// Token: 0x0400398B RID: 14731
		public Color defaultIconColor = Color.white;

		// Token: 0x0400398C RID: 14732
		public KeyBindingDef hotKey;

		// Token: 0x0400398D RID: 14733
		public SoundDef activateSound = null;

		// Token: 0x0400398E RID: 14734
		public int groupKey = 0;

		// Token: 0x0400398F RID: 14735
		public string tutorTag = "TutorTagNotSet";

		// Token: 0x04003990 RID: 14736
		public static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x060056CF RID: 22223 RVA: 0x0015A568 File Offset: 0x00158968
		public virtual string Label
		{
			get
			{
				return this.defaultLabel;
			}
		}

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x060056D0 RID: 22224 RVA: 0x0015A584 File Offset: 0x00158984
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x060056D1 RID: 22225 RVA: 0x0015A5A4 File Offset: 0x001589A4
		public virtual string Desc
		{
			get
			{
				return this.defaultDesc;
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x060056D2 RID: 22226 RVA: 0x0015A5C0 File Offset: 0x001589C0
		public virtual Color IconDrawColor
		{
			get
			{
				return this.defaultIconColor;
			}
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x060056D3 RID: 22227 RVA: 0x0015A5DC File Offset: 0x001589DC
		public virtual SoundDef CurActivateSound
		{
			get
			{
				return this.activateSound;
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x060056D4 RID: 22228 RVA: 0x0015A5F8 File Offset: 0x001589F8
		protected virtual bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x060056D5 RID: 22229 RVA: 0x0015A610 File Offset: 0x00158A10
		public virtual string HighlightTag
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x060056D6 RID: 22230 RVA: 0x0015A62C File Offset: 0x00158A2C
		public virtual string TutorTagSelect
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x060056D7 RID: 22231 RVA: 0x0015A648 File Offset: 0x00158A48
		public override float GetWidth(float maxWidth)
		{
			return 75f;
		}

		// Token: 0x060056D8 RID: 22232 RVA: 0x0015A664 File Offset: 0x00158A64
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

		// Token: 0x060056D9 RID: 22233 RVA: 0x0015AA70 File Offset: 0x00158E70
		public override bool GroupsWith(Gizmo other)
		{
			Command command = other as Command;
			return command != null && ((this.hotKey == command.hotKey && this.Label == command.Label && this.icon == command.icon) || (this.groupKey != 0 && command.groupKey != 0 && this.groupKey == command.groupKey));
		}

		// Token: 0x060056DA RID: 22234 RVA: 0x0015AB14 File Offset: 0x00158F14
		public override void ProcessInput(Event ev)
		{
			if (this.CurActivateSound != null)
			{
				this.CurActivateSound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060056DB RID: 22235 RVA: 0x0015AB30 File Offset: 0x00158F30
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
	}
}
