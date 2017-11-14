using RimWorld;
using RimWorld.Planet;
using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public class FloatMenuOption
	{
		private string labelInt;

		public Action action;

		private MenuOptionPriority priorityInt = MenuOptionPriority.Default;

		public bool autoTakeable;

		public Action mouseoverGuiAction;

		public Thing revalidateClickTarget;

		public WorldObject revalidateWorldClickTarget;

		public float extraPartWidth;

		public Func<Rect, bool> extraPartOnGUI;

		public string tutorTag;

		private FloatMenuSizeMode sizeMode;

		private float cachedRequiredHeight;

		private float cachedRequiredWidth;

		public const float MaxWidth = 300f;

		private const float NormalVerticalMargin = 4f;

		private const float TinyVerticalMargin = 1f;

		private const float NormalHorizontalMargin = 6f;

		private const float TinyHorizontalMargin = 3f;

		private const float MouseOverLabelShift = 4f;

		private static readonly Color ColorBGActive = new ColorInt(21, 25, 29).ToColor;

		private static readonly Color ColorBGActiveMouseover = new ColorInt(29, 45, 50).ToColor;

		private static readonly Color ColorBGDisabled = new ColorInt(40, 40, 40).ToColor;

		private static readonly Color ColorTextActive = Color.white;

		private static readonly Color ColorTextDisabled = new Color(0.9f, 0.9f, 0.9f);

		public const float ExtraPartHeight = 30f;

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
				this.labelInt = value.TrimEnd();
				this.SetSizeMode(this.sizeMode);
			}
		}

		private float VerticalMargin
		{
			get
			{
				return (float)((this.sizeMode != FloatMenuSizeMode.Normal) ? 1.0 : 4.0);
			}
		}

		private float HorizontalMargin
		{
			get
			{
				return (float)((this.sizeMode != FloatMenuSizeMode.Normal) ? 3.0 : 6.0);
			}
		}

		private GameFont CurrentFont
		{
			get
			{
				return (GameFont)((this.sizeMode == FloatMenuSizeMode.Normal) ? 1 : 0);
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
				if (this.Disabled)
				{
					return MenuOptionPriority.DisabledOption;
				}
				return this.priorityInt;
			}
			set
			{
				if (this.Disabled)
				{
					Log.Error("Setting priority on disabled FloatMenuOption: " + this.Label);
				}
				this.priorityInt = value;
			}
		}

		public FloatMenuOption(MenuOptionPriority priority = MenuOptionPriority.Default)
		{
			this.priorityInt = priority;
		}

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

		public void SetSizeMode(FloatMenuSizeMode newSizeMode)
		{
			this.sizeMode = newSizeMode;
			Text.Font = this.CurrentFont;
			float width = (float)(300.0 - (2.0 * this.HorizontalMargin + 4.0 + this.extraPartWidth));
			this.cachedRequiredHeight = (float)(2.0 * this.VerticalMargin + Text.CalcHeight(this.Label, width));
			double num = this.HorizontalMargin + 4.0;
			Vector2 vector = Text.CalcSize(this.Label);
			this.cachedRequiredWidth = (float)(num + vector.x + this.extraPartWidth + this.HorizontalMargin);
		}

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
			if (this.extraPartWidth != 0.0)
			{
				Vector2 vector = Text.CalcSize(this.Label);
				float num = Mathf.Min(vector.x, (float)(rect3.width - 4.0));
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
			if (Widgets.ButtonInvisible(rect2, false))
			{
				if (this.tutorTag != null && !TutorSystem.AllowAction(this.tutorTag))
				{
					return false;
				}
				this.Chosen(colonistOrdering);
				if (this.tutorTag != null)
				{
					TutorSystem.Notify_Event(this.tutorTag);
				}
				return true;
			}
			return false;
		}

		public override string ToString()
		{
			return "FloatMenuOption(" + this.Label + ")";
		}
	}
}
