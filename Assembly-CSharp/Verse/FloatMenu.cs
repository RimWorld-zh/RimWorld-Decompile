using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public class FloatMenu : Window
	{
		private const float OptionSpacing = -1f;

		private const float MaxScreenHeightPercent = 0.9f;

		private const float MinimumColumnWidth = 70f;

		private const float FadeStartMouseDist = 5f;

		private const float FadeFinishMouseDist = 100f;

		public bool givesColonistOrders;

		public bool vanishIfMouseDistant = true;

		protected List<FloatMenuOption> options;

		private string title;

		private bool needSelection;

		private Color baseColor = Color.white;

		private Vector2 scrollPosition;

		private static readonly Vector2 TitleOffset = new Vector2(30f, -25f);

		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);

		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.TotalWidth, this.TotalWindowHeight);
			}
		}

		private float MaxWindowHeight
		{
			get
			{
				return (float)((float)UI.screenHeight * 0.89999997615814209);
			}
		}

		private float TotalWindowHeight
		{
			get
			{
				return (float)(Mathf.Min(this.TotalViewHeight, this.MaxWindowHeight) + 1.0);
			}
		}

		private float MaxViewHeight
		{
			get
			{
				if (this.UsingScrollbar)
				{
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < this.options.Count; i++)
					{
						float requiredHeight = this.options[i].RequiredHeight;
						if (requiredHeight > num)
						{
							num = requiredHeight;
						}
						num2 = (float)(num2 + (requiredHeight + -1.0));
					}
					int columnCount = this.ColumnCount;
					num2 += (float)columnCount * num;
					return num2 / (float)columnCount;
				}
				return this.MaxWindowHeight;
			}
		}

		private float TotalViewHeight
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				float maxViewHeight = this.MaxViewHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1.0 > maxViewHeight)
					{
						if (num2 > num)
						{
							num = num2;
						}
						num2 = requiredHeight;
					}
					else
					{
						num2 = (float)(num2 + (requiredHeight + -1.0));
					}
				}
				return Mathf.Max(num, num2);
			}
		}

		private float TotalWidth
		{
			get
			{
				float num = (float)this.ColumnCount * this.ColumnWidth;
				if (this.UsingScrollbar)
				{
					num = (float)(num + 16.0);
				}
				return num;
			}
		}

		private float ColumnWidth
		{
			get
			{
				float num = 70f;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredWidth = this.options[i].RequiredWidth;
					if (requiredWidth >= 300.0)
					{
						return 300f;
					}
					if (requiredWidth > num)
					{
						num = requiredWidth;
					}
				}
				return Mathf.Round(num);
			}
		}

		private int MaxColumns
		{
			get
			{
				return Mathf.FloorToInt((float)(((float)UI.screenWidth - 16.0) / this.ColumnWidth));
			}
		}

		private bool UsingScrollbar
		{
			get
			{
				return this.ColumnCountIfNoScrollbar > this.MaxColumns;
			}
		}

		private int ColumnCount
		{
			get
			{
				return Mathf.Min(this.ColumnCountIfNoScrollbar, this.MaxColumns);
			}
		}

		private int ColumnCountIfNoScrollbar
		{
			get
			{
				if (this.options == null)
				{
					return 1;
				}
				Text.Font = GameFont.Small;
				int num = 1;
				float num2 = 0f;
				float maxWindowHeight = this.MaxWindowHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1.0 > maxWindowHeight)
					{
						num2 = requiredHeight;
						num++;
					}
					else
					{
						num2 = (float)(num2 + (requiredHeight + -1.0));
					}
				}
				return num;
			}
		}

		public FloatMenuSizeMode SizeMode
		{
			get
			{
				if (this.options.Count > 60)
				{
					return FloatMenuSizeMode.Tiny;
				}
				return FloatMenuSizeMode.Normal;
			}
		}

		public FloatMenu(List<FloatMenuOption> options)
		{
			if (options.NullOrEmpty())
			{
				Log.Error("Created FloatMenu with no options. Closing.");
				this.Close(true);
			}
			this.options = options;
			for (int i = 0; i < options.Count; i++)
			{
				options[i].SetSizeMode(this.SizeMode);
			}
			base.layer = WindowLayer.Super;
			base.closeOnClickedOutside = true;
			base.doWindowBackground = false;
			base.drawShadow = false;
			SoundDefOf.FloatMenuOpen.PlayOneShotOnCamera(null);
		}

		public FloatMenu(List<FloatMenuOption> options, string title, bool needSelection = false) : this(options)
		{
			this.title = title;
			this.needSelection = needSelection;
		}

		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = UI.MousePositionOnUIInverted + FloatMenu.InitialPositionShift;
			float x = vector.x;
			Vector2 initialSize = this.InitialSize;
			if (x + initialSize.x > (float)UI.screenWidth)
			{
				float num = (float)UI.screenWidth;
				Vector2 initialSize2 = this.InitialSize;
				vector.x = num - initialSize2.x;
			}
			float y = vector.y;
			Vector2 initialSize3 = this.InitialSize;
			if (y + initialSize3.y > (float)UI.screenHeight)
			{
				float num2 = (float)UI.screenHeight;
				Vector2 initialSize4 = this.InitialSize;
				vector.y = num2 - initialSize4.y;
			}
			float x2 = vector.x;
			float y2 = vector.y;
			Vector2 initialSize5 = this.InitialSize;
			float x3 = initialSize5.x;
			Vector2 initialSize6 = this.InitialSize;
			base.windowRect = new Rect(x2, y2, x3, initialSize6.y);
		}

		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (!this.title.NullOrEmpty())
			{
				Vector2 vector = new Vector2(base.windowRect.x, base.windowRect.y);
				Text.Font = GameFont.Small;
				Vector2 vector2 = Text.CalcSize(this.title);
				float width = Mathf.Max(150f, (float)(15.0 + vector2.x));
				float x = vector.x;
				Vector2 titleOffset = FloatMenu.TitleOffset;
				float x2 = x + titleOffset.x;
				float y = vector.y;
				Vector2 titleOffset2 = FloatMenu.TitleOffset;
				Rect titleRect = new Rect(x2, y + titleOffset2.y, width, 23f);
				Find.WindowStack.ImmediateWindow(6830963, titleRect, WindowLayer.Super, (Action)delegate
				{
					GUI.color = this.baseColor;
					Text.Font = GameFont.Small;
					Rect position = titleRect.AtZero();
					position.width = 150f;
					GUI.DrawTexture(position, TexUI.TextBGBlack);
					Rect rect = titleRect.AtZero();
					rect.x += 15f;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect, this.title);
					Text.Anchor = TextAnchor.UpperLeft;
				}, false, false, 0f);
			}
		}

		public override void DoWindowContents(Rect rect)
		{
			if (this.needSelection && Find.Selector.SingleSelectedThing == null)
			{
				Find.WindowStack.TryRemove(this, true);
			}
			else
			{
				this.UpdateBaseColor();
				bool usingScrollbar = this.UsingScrollbar;
				GUI.color = this.baseColor;
				Text.Font = GameFont.Small;
				Vector2 zero = Vector2.zero;
				float maxViewHeight = this.MaxViewHeight;
				float columnWidth = this.ColumnWidth;
				if (usingScrollbar)
				{
					rect.width -= 10f;
					Widgets.BeginScrollView(rect, ref this.scrollPosition, new Rect(0f, 0f, (float)(this.TotalWidth - 16.0), this.TotalViewHeight), true);
				}
				foreach (FloatMenuOption item in from op in this.options
				orderby op.Priority descending
				select op)
				{
					float requiredHeight = item.RequiredHeight;
					if (zero.y + requiredHeight + -1.0 > maxViewHeight)
					{
						zero.y = 0f;
						zero.x += (float)(columnWidth + -1.0);
					}
					Rect rect2 = new Rect(zero.x, zero.y, columnWidth, requiredHeight);
					zero.y += (float)(requiredHeight + -1.0);
					if (item.DoGUI(rect2, this.givesColonistOrders))
					{
						Find.WindowStack.TryRemove(this, true);
						break;
					}
				}
				if (usingScrollbar)
				{
					Widgets.EndScrollView();
				}
				if (Event.current.type == EventType.MouseDown)
				{
					Event.current.Use();
					this.Close(true);
				}
				GUI.color = Color.white;
			}
		}

		public void Cancel()
		{
			SoundDefOf.FloatMenuCancel.PlayOneShotOnCamera(null);
			Find.WindowStack.TryRemove(this, true);
		}

		private void UpdateBaseColor()
		{
			this.baseColor = Color.white;
			if (this.vanishIfMouseDistant)
			{
				Rect r = new Rect(0f, 0f, this.TotalWidth, this.TotalWindowHeight).ContractedBy(-5f);
				if (!r.Contains(Event.current.mousePosition))
				{
					float num = GenUI.DistFromRect(r, Event.current.mousePosition);
					this.baseColor = new Color(1f, 1f, 1f, (float)(1.0 - num / 95.0));
					if (num > 95.0)
					{
						this.Close(false);
						this.Cancel();
					}
				}
			}
		}
	}
}
