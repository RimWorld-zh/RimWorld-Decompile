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
		// Token: 0x0600566F RID: 22127 RVA: 0x002C8720 File Offset: 0x002C6B20
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

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06005670 RID: 22128 RVA: 0x002C87C4 File Offset: 0x002C6BC4
		// (set) Token: 0x06005671 RID: 22129 RVA: 0x002C87DF File Offset: 0x002C6BDF
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

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06005672 RID: 22130 RVA: 0x002C8814 File Offset: 0x002C6C14
		private float VerticalMargin
		{
			get
			{
				return (this.sizeMode != FloatMenuSizeMode.Normal) ? 1f : 4f;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06005673 RID: 22131 RVA: 0x002C8844 File Offset: 0x002C6C44
		private float HorizontalMargin
		{
			get
			{
				return (this.sizeMode != FloatMenuSizeMode.Normal) ? 3f : 6f;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06005674 RID: 22132 RVA: 0x002C8874 File Offset: 0x002C6C74
		private GameFont CurrentFont
		{
			get
			{
				return (this.sizeMode != FloatMenuSizeMode.Normal) ? GameFont.Tiny : GameFont.Small;
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06005675 RID: 22133 RVA: 0x002C889C File Offset: 0x002C6C9C
		// (set) Token: 0x06005676 RID: 22134 RVA: 0x002C88BA File Offset: 0x002C6CBA
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

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06005677 RID: 22135 RVA: 0x002C88CC File Offset: 0x002C6CCC
		public float RequiredHeight
		{
			get
			{
				return this.cachedRequiredHeight;
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06005678 RID: 22136 RVA: 0x002C88E8 File Offset: 0x002C6CE8
		public float RequiredWidth
		{
			get
			{
				return this.cachedRequiredWidth;
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06005679 RID: 22137 RVA: 0x002C8904 File Offset: 0x002C6D04
		// (set) Token: 0x0600567A RID: 22138 RVA: 0x002C8931 File Offset: 0x002C6D31
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

		// Token: 0x0600567B RID: 22139 RVA: 0x002C895C File Offset: 0x002C6D5C
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

		// Token: 0x0600567C RID: 22140 RVA: 0x002C89FC File Offset: 0x002C6DFC
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

		// Token: 0x0600567D RID: 22141 RVA: 0x002C8A54 File Offset: 0x002C6E54
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

		// Token: 0x0600567E RID: 22142 RVA: 0x002C8D24 File Offset: 0x002C7124
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

		// Token: 0x0600567F RID: 22143 RVA: 0x002C8D84 File Offset: 0x002C7184
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

		// Token: 0x0400393A RID: 14650
		private string labelInt = null;

		// Token: 0x0400393B RID: 14651
		public Action action = null;

		// Token: 0x0400393C RID: 14652
		private MenuOptionPriority priorityInt = MenuOptionPriority.Default;

		// Token: 0x0400393D RID: 14653
		public bool autoTakeable = false;

		// Token: 0x0400393E RID: 14654
		public float autoTakeablePriority;

		// Token: 0x0400393F RID: 14655
		public Action mouseoverGuiAction = null;

		// Token: 0x04003940 RID: 14656
		public Thing revalidateClickTarget = null;

		// Token: 0x04003941 RID: 14657
		public WorldObject revalidateWorldClickTarget = null;

		// Token: 0x04003942 RID: 14658
		public float extraPartWidth = 0f;

		// Token: 0x04003943 RID: 14659
		public Func<Rect, bool> extraPartOnGUI = null;

		// Token: 0x04003944 RID: 14660
		public string tutorTag = null;

		// Token: 0x04003945 RID: 14661
		private FloatMenuSizeMode sizeMode = FloatMenuSizeMode.Undefined;

		// Token: 0x04003946 RID: 14662
		private float cachedRequiredHeight;

		// Token: 0x04003947 RID: 14663
		private float cachedRequiredWidth;

		// Token: 0x04003948 RID: 14664
		public const float MaxWidth = 300f;

		// Token: 0x04003949 RID: 14665
		private const float NormalVerticalMargin = 4f;

		// Token: 0x0400394A RID: 14666
		private const float TinyVerticalMargin = 1f;

		// Token: 0x0400394B RID: 14667
		private const float NormalHorizontalMargin = 6f;

		// Token: 0x0400394C RID: 14668
		private const float TinyHorizontalMargin = 3f;

		// Token: 0x0400394D RID: 14669
		private const float MouseOverLabelShift = 4f;

		// Token: 0x0400394E RID: 14670
		private static readonly Color ColorBGActive;

		// Token: 0x0400394F RID: 14671
		private static readonly Color ColorBGActiveMouseover;

		// Token: 0x04003950 RID: 14672
		private static readonly Color ColorBGDisabled;

		// Token: 0x04003951 RID: 14673
		private static readonly Color ColorTextActive;

		// Token: 0x04003952 RID: 14674
		private static readonly Color ColorTextDisabled;

		// Token: 0x04003953 RID: 14675
		public const float ExtraPartHeight = 30f;
	}
}
