using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000EAD RID: 3757
	public class WidgetRow
	{
		// Token: 0x06005876 RID: 22646 RVA: 0x002D5131 File Offset: 0x002D3531
		public WidgetRow()
		{
		}

		// Token: 0x06005877 RID: 22647 RVA: 0x002D514C File Offset: 0x002D354C
		public WidgetRow(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.Init(x, y, growDirection, maxWidth, gap);
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06005878 RID: 22648 RVA: 0x002D5174 File Offset: 0x002D3574
		public float FinalX
		{
			get
			{
				return this.curX;
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06005879 RID: 22649 RVA: 0x002D5190 File Offset: 0x002D3590
		public float FinalY
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x0600587A RID: 22650 RVA: 0x002D51AB File Offset: 0x002D35AB
		public void Init(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.growDirection = growDirection;
			this.startX = x;
			this.curX = x;
			this.curY = y;
			this.maxWidth = maxWidth;
			this.gap = gap;
		}

		// Token: 0x0600587B RID: 22651 RVA: 0x002D51DC File Offset: 0x002D35DC
		private float LeftX(float elementWidth)
		{
			float result;
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				result = this.curX;
			}
			else
			{
				result = this.curX - elementWidth;
			}
			return result;
		}

		// Token: 0x0600587C RID: 22652 RVA: 0x002D5220 File Offset: 0x002D3620
		private void IncrementPosition(float amount)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				this.curX += amount;
			}
			else
			{
				this.curX -= amount;
			}
			if (Mathf.Abs(this.curX - this.startX) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x0600587D RID: 22653 RVA: 0x002D528C File Offset: 0x002D368C
		private void IncrementY()
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.LeftThenUp)
			{
				this.curY -= 24f + this.gap;
			}
			else
			{
				this.curY += 24f + this.gap;
			}
			this.curX = this.startX;
		}

		// Token: 0x0600587E RID: 22654 RVA: 0x002D52F4 File Offset: 0x002D36F4
		private void IncrementYIfWillExceedMaxWidth(float width)
		{
			if (Mathf.Abs(this.curX - this.startX) + Mathf.Abs(width) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x0600587F RID: 22655 RVA: 0x002D5321 File Offset: 0x002D3721
		public void Gap(float width)
		{
			if (this.curX != this.startX)
			{
				this.IncrementPosition(width);
			}
		}

		// Token: 0x06005880 RID: 22656 RVA: 0x002D533C File Offset: 0x002D373C
		public bool ButtonIcon(Texture2D tex, string tooltip = null, Color? mouseoverColor = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			bool result = Widgets.ButtonImage(rect, tex, Color.white, (mouseoverColor == null) ? GenUI.MouseoverColor : mouseoverColor.Value);
			this.IncrementPosition(24f + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x06005881 RID: 22657 RVA: 0x002D53D2 File Offset: 0x002D37D2
		public void GapButtonIcon()
		{
			if (this.curY != this.startX)
			{
				this.IncrementPosition(24f + this.gap);
			}
		}

		// Token: 0x06005882 RID: 22658 RVA: 0x002D53F8 File Offset: 0x002D37F8
		public void ToggleableIcon(ref bool toggleable, Texture2D tex, string tooltip, SoundDef mouseoverSound = null, string tutorTag = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			bool flag = Widgets.ButtonImage(rect, tex);
			this.IncrementPosition(24f + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			Rect position = new Rect(rect.x + rect.width / 2f, rect.y, rect.height / 2f, rect.height / 2f);
			Texture2D image = (!toggleable) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
			GUI.DrawTexture(position, image);
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect, mouseoverSound);
			}
			if (flag)
			{
				toggleable = !toggleable;
				if (toggleable)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			if (tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, tutorTag);
			}
		}

		// Token: 0x06005883 RID: 22659 RVA: 0x002D5510 File Offset: 0x002D3910
		public Rect Icon(Texture2D tex, string tooltip = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			GUI.DrawTexture(rect, tex);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(24f + this.gap);
			return rect;
		}

		// Token: 0x06005884 RID: 22660 RVA: 0x002D5584 File Offset: 0x002D3984
		public bool ButtonText(string label, string tooltip = null, bool drawBackground = true, bool doMouseoverSound = false)
		{
			Vector2 vector = Text.CalcSize(label);
			vector.x += 16f;
			vector.y += 2f;
			this.IncrementYIfWillExceedMaxWidth(vector.x);
			Rect rect = new Rect(this.LeftX(vector.x), this.curY, vector.x, vector.y);
			bool result = Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, true);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(rect.width + this.gap);
			return result;
		}

		// Token: 0x06005885 RID: 22661 RVA: 0x002D5634 File Offset: 0x002D3A34
		public Rect Label(string text, float width = -1f)
		{
			if (width < 0f)
			{
				width = Text.CalcSize(text).x;
			}
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, 24f);
			this.IncrementPosition(2f);
			Widgets.Label(rect, text);
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x06005886 RID: 22662 RVA: 0x002D56B0 File Offset: 0x002D3AB0
		public Rect FillableBar(float width, float height, float fillPct, string label, Texture2D fillTex, Texture2D bgTex = null)
		{
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.FillableBar(rect, fillPct, fillTex, bgTex, false);
			if (!label.NullOrEmpty())
			{
				Rect rect2 = rect;
				rect2.xMin += 2f;
				rect2.xMax -= 2f;
				if (Text.Anchor >= TextAnchor.UpperLeft)
				{
					rect2.height += 14f;
				}
				Text.Font = GameFont.Tiny;
				Text.WordWrap = false;
				Widgets.Label(rect2, label);
				Text.WordWrap = true;
			}
			this.IncrementPosition(width);
			return rect;
		}

		// Token: 0x04003AF5 RID: 15093
		private float startX;

		// Token: 0x04003AF6 RID: 15094
		private float curX;

		// Token: 0x04003AF7 RID: 15095
		private float curY;

		// Token: 0x04003AF8 RID: 15096
		private float maxWidth = 99999f;

		// Token: 0x04003AF9 RID: 15097
		private float gap;

		// Token: 0x04003AFA RID: 15098
		private UIDirection growDirection = UIDirection.RightThenUp;

		// Token: 0x04003AFB RID: 15099
		public const float IconSize = 24f;

		// Token: 0x04003AFC RID: 15100
		public const float DefaultGap = 4f;

		// Token: 0x04003AFD RID: 15101
		private const float DefaultMaxWidth = 99999f;
	}
}
