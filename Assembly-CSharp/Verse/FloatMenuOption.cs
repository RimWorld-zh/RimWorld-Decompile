using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public class FloatMenuOption
	{
		private string labelInt = null;

		public Action action = null;

		private MenuOptionPriority priorityInt = MenuOptionPriority.Default;

		public bool autoTakeable = false;

		public float autoTakeablePriority;

		public Action mouseoverGuiAction = null;

		public Thing revalidateClickTarget = null;

		public WorldObject revalidateWorldClickTarget = null;

		public float extraPartWidth = 0f;

		public Func<Rect, bool> extraPartOnGUI = null;

		public string tutorTag = null;

		private FloatMenuSizeMode sizeMode = FloatMenuSizeMode.Undefined;

		private float cachedRequiredHeight;

		private float cachedRequiredWidth;

		public const float MaxWidth = 300f;

		private const float NormalVerticalMargin = 4f;

		private const float TinyVerticalMargin = 1f;

		private const float NormalHorizontalMargin = 6f;

		private const float TinyHorizontalMargin = 3f;

		private const float MouseOverLabelShift = 4f;

		private static readonly Color ColorBGActive;

		private static readonly Color ColorBGActiveMouseover;

		private static readonly Color ColorBGDisabled;

		private static readonly Color ColorTextActive;

		private static readonly Color ColorTextDisabled;

		public const float ExtraPartHeight = 30f;

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

		private float VerticalMargin
		{
			get
			{
				return (this.sizeMode != FloatMenuSizeMode.Normal) ? 1f : 4f;
			}
		}

		private float HorizontalMargin
		{
			get
			{
				return (this.sizeMode != FloatMenuSizeMode.Normal) ? 3f : 6f;
			}
		}

		private GameFont CurrentFont
		{
			get
			{
				return (this.sizeMode != FloatMenuSizeMode.Normal) ? GameFont.Tiny : GameFont.Small;
			}
		}

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

		public float RequiredHeight
		{
			get
			{
				return this.cachedRequiredHeight;
			}
		}

		public float RequiredWidth
		{
			get
			{
				return this.cachedRequiredWidth;
			}
		}

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

		public void Chosen(bool colonistOrdering, FloatMenu floatMenu)
		{
			if (floatMenu != null)
			{
				floatMenu.PreOptionChosen(this);
			}
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

		public virtual bool DoGUI(Rect rect, bool colonistOrdering, FloatMenu floatMenu)
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
					this.Chosen(colonistOrdering, floatMenu);
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
