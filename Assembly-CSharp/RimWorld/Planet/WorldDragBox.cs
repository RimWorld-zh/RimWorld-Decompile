using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008E9 RID: 2281
	public class WorldDragBox
	{
		// Token: 0x04001C64 RID: 7268
		public bool active = false;

		// Token: 0x04001C65 RID: 7269
		public Vector2 start;

		// Token: 0x04001C66 RID: 7270
		private const float DragBoxMinDiagonal = 7f;

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003477 RID: 13431 RVA: 0x001C11D4 File Offset: 0x001BF5D4
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06003478 RID: 13432 RVA: 0x001C1208 File Offset: 0x001BF608
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06003479 RID: 13433 RVA: 0x001C123C File Offset: 0x001BF63C
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x0600347A RID: 13434 RVA: 0x001C1270 File Offset: 0x001BF670
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x0600347B RID: 13435 RVA: 0x001C12A4 File Offset: 0x001BF6A4
		public Rect ScreenRect
		{
			get
			{
				return new Rect(this.LeftX, this.BotZ, this.RightX - this.LeftX, this.TopZ - this.BotZ);
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x0600347C RID: 13436 RVA: 0x001C12E4 File Offset: 0x001BF6E4
		public float Diagonal
		{
			get
			{
				return (this.start - new Vector2(UI.MousePositionOnUIInverted.x, UI.MousePositionOnUIInverted.y)).magnitude;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x0600347D RID: 13437 RVA: 0x001C132C File Offset: 0x001BF72C
		public bool IsValid
		{
			get
			{
				return this.Diagonal > 7f;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x0600347E RID: 13438 RVA: 0x001C1350 File Offset: 0x001BF750
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x001C1379 File Offset: 0x001BF779
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2);
			}
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x001C1394 File Offset: 0x001BF794
		public bool Contains(WorldObject o)
		{
			return this.Contains(o.ScreenPos());
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x001C13B8 File Offset: 0x001BF7B8
		public bool Contains(Vector2 screenPoint)
		{
			return screenPoint.x + 0.5f > this.LeftX && screenPoint.x - 0.5f < this.RightX && screenPoint.y + 0.5f > this.BotZ && screenPoint.y - 0.5f < this.TopZ;
		}
	}
}
