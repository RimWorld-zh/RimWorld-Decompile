using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E59 RID: 3673
	public class FloatMenuOption
	{
		// Token: 0x04003947 RID: 14663
		private string labelInt = null;

		// Token: 0x04003948 RID: 14664
		public Action action = null;

		// Token: 0x04003949 RID: 14665
		private MenuOptionPriority priorityInt = MenuOptionPriority.Default;

		// Token: 0x0400394A RID: 14666
		public bool autoTakeable = false;

		// Token: 0x0400394B RID: 14667
		public float autoTakeablePriority;

		// Token: 0x0400394C RID: 14668
		public Action mouseoverGuiAction = null;

		// Token: 0x0400394D RID: 14669
		public Thing revalidateClickTarget = null;

		// Token: 0x0400394E RID: 14670
		public WorldObject revalidateWorldClickTarget = null;

		// Token: 0x0400394F RID: 14671
		public float extraPartWidth = 0f;

		// Token: 0x04003950 RID: 14672
		public Func<Rect, bool> extraPartOnGUI = null;

		// Token: 0x04003951 RID: 14673
		public string tutorTag = null;

		// Token: 0x04003952 RID: 14674
		private FloatMenuSizeMode sizeMode = FloatMenuSizeMode.Undefined;

		// Token: 0x04003953 RID: 14675
		private float cachedRequiredHeight;

		// Token: 0x04003954 RID: 14676
		private float cachedRequiredWidth;

		// Token: 0x04003955 RID: 14677
		public const float MaxWidth = 300f;

		// Token: 0x04003956 RID: 14678
		private const float NormalVerticalMargin = 4f;

		// Token: 0x04003957 RID: 14679
		private const float TinyVerticalMargin = 1f;

		// Token: 0x04003958 RID: 14680
		private const float NormalHorizontalMargin = 6f;

		// Token: 0x04003959 RID: 14681
		private const float TinyHorizontalMargin = 3f;

		// Token: 0x0400395A RID: 14682
		private const float MouseOverLabelShift = 4f;

		// Token: 0x0400395B RID: 14683
		private static readonly Color ColorBGActive;

		// Token: 0x0400395C RID: 14684
		private static readonly Color ColorBGActiveMouseover;

		// Token: 0x0400395D RID: 14685
		private static readonly Color ColorBGDisabled;

		// Token: 0x0400395E RID: 14686
		private static readonly Color ColorTextActive;

		// Token: 0x0400395F RID: 14687
		private static readonly Color ColorTextDisabled;

		// Token: 0x04003960 RID: 14688
		public const float ExtraPartHeight = 30f;

		// Token: 0x06005691 RID: 22161 RVA: 0x002CA45C File Offset: 0x002C885C
		public FloatMenuOption(string label, Action action, MenuOptionPriority priority = MenuOptionPriority.Default, Action mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null)
		{
			this.Label = label;
			this.action = action;
			this.priorityInt = priority;
			this.revalidateClickTarget = revalidateClickTarget;
			this.mouseoverGuiAction = mouseoverGuiAction;
			this.extraPartWidth = extraPartWidth;
			this.extraPartOnGUI = extraPartOnGUI;
			this.revalidateWorldClickTarget = revalidateWorldClickTarget;
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06005692 RID: 22162 RVA: 0x002CA500 File Offset: 0x002C8900
		// (set) Token: 0x06005693 RID: 22163 RVA: 0x002CA51B File Offset: 0x002C891B
		public string Label
		{
			get
			{
				return this.labelInt;
			}
			set
			{
				if (value.NullOrEmpty())
				{
					value = "(missing label)";
				}
				this.labelInt = value.TrimEnd(new char[0]);
				this.SetSizeMode(this.sizeMode);
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06005694 RID: 22164 RVA: 0x002CA550 File Offset: 0x002C8950
		private float VerticalMargin
		{
			get
			{
				return (this.sizeMode != FloatMenuSizeMode.Normal) ? 1f : 4f;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06005695 RID: 22165 RVA: 0x002CA580 File Offset: 0x002C8980
		private float HorizontalMargin
		{
			get
			{
				return (this.sizeMode != FloatMenuSizeMode.Normal) ? 3f : 6f;
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06005696 RID: 22166 RVA: 0x002CA5B0 File Offset: 0x002C89B0
		private GameFont CurrentFont
		{
			get
			{
				return (this.sizeMode != FloatMenuSizeMode.Normal) ? GameFont.Tiny : GameFont.Small;
			}
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06005697 RID: 22167 RVA: 0x002CA5D8 File Offset: 0x002C89D8
		// (set) Token: 0x06005698 RID: 22168 RVA: 0x002CA5F6 File Offset: 0x002C89F6
		public bool Disabled
		{
			get
			{
				return this.action == null;
			}
			set
			{
				if (value)
				{
					this.action = null;
				}
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06005699 RID: 22169 RVA: 0x002CA608 File Offset: 0x002C8A08
		public float RequiredHeight
		{
			get
			{
				return this.cachedRequiredHeight;
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x0600569A RID: 22170 RVA: 0x002CA624 File Offset: 0x002C8A24
		public float RequiredWidth
		{
			get
			{
				return this.cachedRequiredWidth;
			}
		}

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x0600569B RID: 22171 RVA: 0x002CA640 File Offset: 0x002C8A40
		// (set) Token: 0x0600569C RID: 22172 RVA: 0x002CA66D File Offset: 0x002C8A6D
		public MenuOptionPriority Priority
		{
			get
			{
				MenuOptionPriority result;
				if (this.Disabled)
				{
					result = MenuOptionPriority.DisabledOption;
				}
				else
				{
					result = this.priorityInt;
				}
				return result;
			}
			set
			{
				if (this.Disabled)
				{
					Log.Error("Setting priority on disabled FloatMenuOption: " + this.Label, false);
				}
				this.priorityInt = value;
			}
		}

		// Token: 0x0600569D RID: 22173 RVA: 0x002CA698 File Offset: 0x002C8A98
		public void SetSizeMode(FloatMenuSizeMode newSizeMode)
		{
			this.sizeMode = newSizeMode;
			GameFont font = Text.Font;
			Text.Font = this.CurrentFont;
			float width = 300f - (2f * this.HorizontalMargin + 4f + this.extraPartWidth);
			this.cachedRequiredHeight = 2f * this.VerticalMargin + Text.CalcHeight(this.Label, width);
			this.cachedRequiredWidth = this.HorizontalMargin + 4f + Text.CalcSize(this.Label).x + this.extraPartWidth + this.HorizontalMargin;
			Text.Font = font;
		}

		// Token: 0x0600569E RID: 22174 RVA: 0x002CA738 File Offset: 0x002C8B38
		public void Chosen(bool colonistOrdering)
		{
			if (!this.Disabled)
			{
				if (this.action != null)
				{
					if (colonistOrdering)
					{
						SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
					}
					this.action();
				}
			}
			else
			{
				SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600569F RID: 22175 RVA: 0x002CA790 File Offset: 0x002C8B90
		public virtual bool DoGUI(Rect rect, bool colonistOrdering)
		{
			Rect rect2 = rect;
			rect2.height -= 1f;
			bool flag = !this.Disabled && Mouse.IsOver(rect2);
			bool flag2 = false;
			Text.Font = this.CurrentFont;
			Rect rect3 = rect;
			rect3.xMin += this.HorizontalMargin;
			rect3.xMax -= this.HorizontalMargin;
			rect3.xMax -= 4f;
			rect3.xMax -= this.extraPartWidth;
			if (flag)
			{
				rect3.x += 4f;
			}
			Rect rect4 = default(Rect);
			if (this.extraPartWidth != 0f)
			{
				float num = Mathf.Min(Text.CalcSize(this.Label).x, rect3.width - 4f);
				rect4 = new Rect(rect3.xMin + num, rect3.yMin, this.extraPartWidth, 30f);
				flag2 = Mouse.IsOver(rect4);
			}
			if (!this.Disabled)
			{
				MouseoverSounds.DoRegion(rect2);
			}
			Color color = GUI.color;
			if (this.Disabled)
			{
				GUI.color = FloatMenuOption.ColorBGDisabled * color;
			}
			else if (flag && !flag2)
			{
				GUI.color = FloatMenuOption.ColorBGActiveMouseover * color;
			}
			else
			{
				GUI.color = FloatMenuOption.ColorBGActive * color;
			}
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = (this.Disabled ? FloatMenuOption.ColorTextDisabled : FloatMenuOption.ColorTextActive) * color;
			if (this.sizeMode == FloatMenuSizeMode.Tiny)
			{
				rect3.y += 1f;
			}
			Widgets.DrawAtlas(rect, TexUI.FloatMenuOptionBG);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, this.Label);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = color;
			if (this.extraPartOnGUI != null)
			{
				bool flag3 = this.extraPartOnGUI(rect4);
				GUI.color = color;
				if (flag3)
				{
					return true;
				}
			}
			if (flag && this.mouseoverGuiAction != null)
			{
				this.mouseoverGuiAction();
			}
			if (this.tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.tutorTag);
			}
			bool result;
			if (Widgets.ButtonInvisible(rect2, false))
			{
				if (this.tutorTag != null && !TutorSystem.AllowAction(this.tutorTag))
				{
					result = false;
				}
				else
				{
					this.Chosen(colonistOrdering);
					if (this.tutorTag != null)
					{
						TutorSystem.Notify_Event(this.tutorTag);
					}
					result = true;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060056A0 RID: 22176 RVA: 0x002CAA60 File Offset: 0x002C8E60
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"FloatMenuOption(",
				this.Label,
				", ",
				(!this.Disabled) ? "enabled" : "disabled",
				")"
			});
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x002CAAC0 File Offset: 0x002C8EC0
		// Note: this type is marked as 'beforefieldinit'.
		static FloatMenuOption()
		{
			ColorInt colorInt = new ColorInt(21, 25, 29);
			FloatMenuOption.ColorBGActive = colorInt.ToColor;
			ColorInt colorInt2 = new ColorInt(29, 45, 50);
			FloatMenuOption.ColorBGActiveMouseover = colorInt2.ToColor;
			ColorInt colorInt3 = new ColorInt(40, 40, 40);
			FloatMenuOption.ColorBGDisabled = colorInt3.ToColor;
			FloatMenuOption.ColorTextActive = Color.white;
			FloatMenuOption.ColorTextDisabled = new Color(0.9f, 0.9f, 0.9f);
		}
	}
}
