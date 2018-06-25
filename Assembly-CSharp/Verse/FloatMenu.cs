using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E57 RID: 3671
	public class FloatMenu : Window
	{
		// Token: 0x04003934 RID: 14644
		public bool givesColonistOrders = false;

		// Token: 0x04003935 RID: 14645
		public bool vanishIfMouseDistant = true;

		// Token: 0x04003936 RID: 14646
		public Action onCloseCallback = null;

		// Token: 0x04003937 RID: 14647
		protected List<FloatMenuOption> options;

		// Token: 0x04003938 RID: 14648
		private string title = null;

		// Token: 0x04003939 RID: 14649
		private bool needSelection = false;

		// Token: 0x0400393A RID: 14650
		private Color baseColor = Color.white;

		// Token: 0x0400393B RID: 14651
		private Vector2 scrollPosition;

		// Token: 0x0400393C RID: 14652
		private static readonly Vector2 TitleOffset = new Vector2(30f, -25f);

		// Token: 0x0400393D RID: 14653
		private const float OptionSpacing = -1f;

		// Token: 0x0400393E RID: 14654
		private const float MaxScreenHeightPercent = 0.9f;

		// Token: 0x0400393F RID: 14655
		private const float MinimumColumnWidth = 70f;

		// Token: 0x04003940 RID: 14656
		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);

		// Token: 0x04003941 RID: 14657
		private const float FadeStartMouseDist = 5f;

		// Token: 0x04003942 RID: 14658
		private const float FadeFinishMouseDist = 100f;

		// Token: 0x06005676 RID: 22134 RVA: 0x002C9A58 File Offset: 0x002C7E58
		public FloatMenu(List<FloatMenuOption> options)
		{
			if (options.NullOrEmpty<FloatMenuOption>())
			{
				Log.Error("Created FloatMenu with no options. Closing.", false);
				this.Close(true);
			}
			this.options = (from op in options
			orderby op.Priority descending
			select op).ToList<FloatMenuOption>();
			for (int i = 0; i < options.Count; i++)
			{
				options[i].SetSizeMode(this.SizeMode);
			}
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x06005677 RID: 22135 RVA: 0x002C9B39 File Offset: 0x002C7F39
		public FloatMenu(List<FloatMenuOption> options, string title, bool needSelection = false) : this(options)
		{
			this.title = title;
			this.needSelection = needSelection;
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06005678 RID: 22136 RVA: 0x002C9B54 File Offset: 0x002C7F54
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06005679 RID: 22137 RVA: 0x002C9B70 File Offset: 0x002C7F70
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.TotalWidth, this.TotalWindowHeight);
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x0600567A RID: 22138 RVA: 0x002C9B98 File Offset: 0x002C7F98
		private float MaxWindowHeight
		{
			get
			{
				return (float)UI.screenHeight * 0.9f;
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x0600567B RID: 22139 RVA: 0x002C9BBC File Offset: 0x002C7FBC
		private float TotalWindowHeight
		{
			get
			{
				return Mathf.Min(this.TotalViewHeight, this.MaxWindowHeight) + 1f;
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x0600567C RID: 22140 RVA: 0x002C9BE8 File Offset: 0x002C7FE8
		private float MaxViewHeight
		{
			get
			{
				float result;
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
						num2 += requiredHeight + -1f;
					}
					int columnCount = this.ColumnCount;
					num2 += (float)columnCount * num;
					result = num2 / (float)columnCount;
				}
				else
				{
					result = this.MaxWindowHeight;
				}
				return result;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x0600567D RID: 22141 RVA: 0x002C9C7C File Offset: 0x002C807C
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
					if (num2 + requiredHeight + -1f > maxViewHeight)
					{
						if (num2 > num)
						{
							num = num2;
						}
						num2 = requiredHeight;
					}
					else
					{
						num2 += requiredHeight + -1f;
					}
				}
				return Mathf.Max(num, num2);
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x0600567E RID: 22142 RVA: 0x002C9D0C File Offset: 0x002C810C
		private float TotalWidth
		{
			get
			{
				float num = (float)this.ColumnCount * this.ColumnWidth;
				if (this.UsingScrollbar)
				{
					num += 16f;
				}
				return num;
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x0600567F RID: 22143 RVA: 0x002C9D44 File Offset: 0x002C8144
		private float ColumnWidth
		{
			get
			{
				float num = 70f;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredWidth = this.options[i].RequiredWidth;
					if (requiredWidth >= 300f)
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

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06005680 RID: 22144 RVA: 0x002C9DB4 File Offset: 0x002C81B4
		private int MaxColumns
		{
			get
			{
				return Mathf.FloorToInt(((float)UI.screenWidth - 16f) / this.ColumnWidth);
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06005681 RID: 22145 RVA: 0x002C9DE4 File Offset: 0x002C81E4
		private bool UsingScrollbar
		{
			get
			{
				return this.ColumnCountIfNoScrollbar > this.MaxColumns;
			}
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06005682 RID: 22146 RVA: 0x002C9E08 File Offset: 0x002C8208
		private int ColumnCount
		{
			get
			{
				return Mathf.Min(this.ColumnCountIfNoScrollbar, this.MaxColumns);
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06005683 RID: 22147 RVA: 0x002C9E30 File Offset: 0x002C8230
		private int ColumnCountIfNoScrollbar
		{
			get
			{
				int result;
				if (this.options == null)
				{
					result = 1;
				}
				else
				{
					Text.Font = GameFont.Small;
					int num = 1;
					float num2 = 0f;
					float maxWindowHeight = this.MaxWindowHeight;
					for (int i = 0; i < this.options.Count; i++)
					{
						float requiredHeight = this.options[i].RequiredHeight;
						if (num2 + requiredHeight + -1f > maxWindowHeight)
						{
							num2 = requiredHeight;
							num++;
						}
						else
						{
							num2 += requiredHeight + -1f;
						}
					}
					result = num;
				}
				return result;
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06005684 RID: 22148 RVA: 0x002C9ECC File Offset: 0x002C82CC
		public FloatMenuSizeMode SizeMode
		{
			get
			{
				FloatMenuSizeMode result;
				if (this.options.Count > 60)
				{
					result = FloatMenuSizeMode.Tiny;
				}
				else
				{
					result = FloatMenuSizeMode.Normal;
				}
				return result;
			}
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x002C9EFC File Offset: 0x002C82FC
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = UI.MousePositionOnUIInverted + FloatMenu.InitialPositionShift;
			if (vector.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				vector.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (vector.y + this.InitialSize.y > (float)UI.screenHeight)
			{
				vector.y = (float)UI.screenHeight - this.InitialSize.y;
			}
			this.windowRect = new Rect(vector.x, vector.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x06005686 RID: 22150 RVA: 0x002C9FCC File Offset: 0x002C83CC
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (!this.title.NullOrEmpty())
			{
				Vector2 vector = new Vector2(this.windowRect.x, this.windowRect.y);
				Text.Font = GameFont.Small;
				float width = Mathf.Max(150f, 15f + Text.CalcSize(this.title).x);
				Rect titleRect = new Rect(vector.x + FloatMenu.TitleOffset.x, vector.y + FloatMenu.TitleOffset.y, width, 23f);
				Find.WindowStack.ImmediateWindow(6830963, titleRect, WindowLayer.Super, delegate
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

		// Token: 0x06005687 RID: 22151 RVA: 0x002CA0AC File Offset: 0x002C84AC
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
					Widgets.BeginScrollView(rect, ref this.scrollPosition, new Rect(0f, 0f, this.TotalWidth - 16f, this.TotalViewHeight), true);
				}
				foreach (FloatMenuOption floatMenuOption in this.options)
				{
					float requiredHeight = floatMenuOption.RequiredHeight;
					if (zero.y + requiredHeight + -1f > maxViewHeight)
					{
						zero.y = 0f;
						zero.x += columnWidth + -1f;
					}
					Rect rect2 = new Rect(zero.x, zero.y, columnWidth, requiredHeight);
					zero.y += requiredHeight + -1f;
					bool flag = floatMenuOption.DoGUI(rect2, this.givesColonistOrders);
					if (flag)
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

		// Token: 0x06005688 RID: 22152 RVA: 0x002CA284 File Offset: 0x002C8684
		public override void PostClose()
		{
			base.PostClose();
			if (this.onCloseCallback != null)
			{
				this.onCloseCallback();
			}
		}

		// Token: 0x06005689 RID: 22153 RVA: 0x002CA2A3 File Offset: 0x002C86A3
		public void Cancel()
		{
			SoundDefOf.FloatMenu_Cancel.PlayOneShotOnCamera(null);
			Find.WindowStack.TryRemove(this, true);
		}

		// Token: 0x0600568A RID: 22154 RVA: 0x002CA2C0 File Offset: 0x002C86C0
		private void UpdateBaseColor()
		{
			this.baseColor = Color.white;
			if (this.vanishIfMouseDistant)
			{
				Rect r = new Rect(0f, 0f, this.TotalWidth, this.TotalWindowHeight).ContractedBy(-5f);
				if (!r.Contains(Event.current.mousePosition))
				{
					float num = GenUI.DistFromRect(r, Event.current.mousePosition);
					this.baseColor = new Color(1f, 1f, 1f, 1f - num / 95f);
					if (num > 95f)
					{
						this.Close(false);
						this.Cancel();
					}
				}
			}
		}
	}
}
