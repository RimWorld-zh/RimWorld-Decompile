using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000EAE RID: 3758
	public class WidgetRow
	{
		// Token: 0x04003B05 RID: 15109
		private float startX;

		// Token: 0x04003B06 RID: 15110
		private float curX;

		// Token: 0x04003B07 RID: 15111
		private float curY;

		// Token: 0x04003B08 RID: 15112
		private float maxWidth = 99999f;

		// Token: 0x04003B09 RID: 15113
		private float gap;

		// Token: 0x04003B0A RID: 15114
		private UIDirection growDirection = UIDirection.RightThenUp;

		// Token: 0x04003B0B RID: 15115
		public const float IconSize = 24f;

		// Token: 0x04003B0C RID: 15116
		public const float DefaultGap = 4f;

		// Token: 0x04003B0D RID: 15117
		private const float DefaultMaxWidth = 99999f;

		// Token: 0x0600589A RID: 22682 RVA: 0x002D6E6D File Offset: 0x002D526D
		public WidgetRow()
		{
		}

		// Token: 0x0600589B RID: 22683 RVA: 0x002D6E88 File Offset: 0x002D5288
		public WidgetRow(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.Init(x, y, growDirection, maxWidth, gap);
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x0600589C RID: 22684 RVA: 0x002D6EB0 File Offset: 0x002D52B0
		public float FinalX
		{
			get
			{
				return this.curX;
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x0600589D RID: 22685 RVA: 0x002D6ECC File Offset: 0x002D52CC
		public float FinalY
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x0600589E RID: 22686 RVA: 0x002D6EE7 File Offset: 0x002D52E7
		public void Init(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.growDirection = growDirection;
			this.startX = x;
			this.curX = x;
			this.curY = y;
			this.maxWidth = maxWidth;
			this.gap = gap;
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x002D6F18 File Offset: 0x002D5318
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

		// Token: 0x060058A0 RID: 22688 RVA: 0x002D6F5C File Offset: 0x002D535C
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

		// Token: 0x060058A1 RID: 22689 RVA: 0x002D6FC8 File Offset: 0x002D53C8
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

		// Token: 0x060058A2 RID: 22690 RVA: 0x002D7030 File Offset: 0x002D5430
		private void IncrementYIfWillExceedMaxWidth(float width)
		{
			if (Mathf.Abs(this.curX - this.startX) + Mathf.Abs(width) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x060058A3 RID: 22691 RVA: 0x002D705D File Offset: 0x002D545D
		public void Gap(float width)
		{
			if (this.curX != this.startX)
			{
				this.IncrementPosition(width);
			}
		}

		// Token: 0x060058A4 RID: 22692 RVA: 0x002D7078 File Offset: 0x002D5478
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

		// Token: 0x060058A5 RID: 22693 RVA: 0x002D710E File Offset: 0x002D550E
		public void GapButtonIcon()
		{
			if (this.curY != this.startX)
			{
				this.IncrementPosition(24f + this.gap);
			}
		}

		// Token: 0x060058A6 RID: 22694 RVA: 0x002D7134 File Offset: 0x002D5534
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

		// Token: 0x060058A7 RID: 22695 RVA: 0x002D724C File Offset: 0x002D564C
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

		// Token: 0x060058A8 RID: 22696 RVA: 0x002D72C0 File Offset: 0x002D56C0
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

		// Token: 0x060058A9 RID: 22697 RVA: 0x002D7370 File Offset: 0x002D5770
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

		// Token: 0x060058AA RID: 22698 RVA: 0x002D73EC File Offset: 0x002D57EC
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
	}
}
