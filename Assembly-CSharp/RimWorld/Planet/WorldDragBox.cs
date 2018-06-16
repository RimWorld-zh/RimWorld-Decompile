using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008EB RID: 2283
	public class WorldDragBox
	{
		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06003478 RID: 13432 RVA: 0x001C0DE4 File Offset: 0x001BF1E4
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003479 RID: 13433 RVA: 0x001C0E18 File Offset: 0x001BF218
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x0600347A RID: 13434 RVA: 0x001C0E4C File Offset: 0x001BF24C
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x0600347B RID: 13435 RVA: 0x001C0E80 File Offset: 0x001BF280
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x0600347C RID: 13436 RVA: 0x001C0EB4 File Offset: 0x001BF2B4
		public Rect ScreenRect
		{
			get
			{
				return new Rect(this.LeftX, this.BotZ, this.RightX - this.LeftX, this.TopZ - this.BotZ);
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x0600347D RID: 13437 RVA: 0x001C0EF4 File Offset: 0x001BF2F4
		public float Diagonal
		{
			get
			{
				return (this.start - new Vector2(UI.MousePositionOnUIInverted.x, UI.MousePositionOnUIInverted.y)).magnitude;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x0600347E RID: 13438 RVA: 0x001C0F3C File Offset: 0x001BF33C
		public bool IsValid
		{
			get
			{
				return this.Diagonal > 7f;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x0600347F RID: 13439 RVA: 0x001C0F60 File Offset: 0x001BF360
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x001C0F89 File Offset: 0x001BF389
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2);
			}
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x001C0FA4 File Offset: 0x001BF3A4
		public bool Contains(WorldObject o)
		{
			return this.Contains(o.ScreenPos());
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x001C0FC8 File Offset: 0x001BF3C8
		public bool Contains(Vector2 screenPoint)
		{
			return screenPoint.x + 0.5f > this.LeftX && screenPoint.x - 0.5f < this.RightX && screenPoint.y + 0.5f > this.BotZ && screenPoint.y - 0.5f < this.TopZ;
		}

		// Token: 0x04001C66 RID: 7270
		public bool active = false;

		// Token: 0x04001C67 RID: 7271
		public Vector2 start;

		// Token: 0x04001C68 RID: 7272
		private const float DragBoxMinDiagonal = 7f;
	}
}
