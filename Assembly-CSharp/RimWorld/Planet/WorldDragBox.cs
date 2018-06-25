using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008E9 RID: 2281
	public class WorldDragBox
	{
		// Token: 0x04001C6A RID: 7274
		public bool active = false;

		// Token: 0x04001C6B RID: 7275
		public Vector2 start;

		// Token: 0x04001C6C RID: 7276
		private const float DragBoxMinDiagonal = 7f;

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003477 RID: 13431 RVA: 0x001C14A8 File Offset: 0x001BF8A8
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06003478 RID: 13432 RVA: 0x001C14DC File Offset: 0x001BF8DC
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06003479 RID: 13433 RVA: 0x001C1510 File Offset: 0x001BF910
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x0600347A RID: 13434 RVA: 0x001C1544 File Offset: 0x001BF944
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x0600347B RID: 13435 RVA: 0x001C1578 File Offset: 0x001BF978
		public Rect ScreenRect
		{
			get
			{
				return new Rect(this.LeftX, this.BotZ, this.RightX - this.LeftX, this.TopZ - this.BotZ);
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x0600347C RID: 13436 RVA: 0x001C15B8 File Offset: 0x001BF9B8
		public float Diagonal
		{
			get
			{
				return (this.start - new Vector2(UI.MousePositionOnUIInverted.x, UI.MousePositionOnUIInverted.y)).magnitude;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x0600347D RID: 13437 RVA: 0x001C1600 File Offset: 0x001BFA00
		public bool IsValid
		{
			get
			{
				return this.Diagonal > 7f;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x0600347E RID: 13438 RVA: 0x001C1624 File Offset: 0x001BFA24
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x001C164D File Offset: 0x001BFA4D
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2);
			}
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x001C1668 File Offset: 0x001BFA68
		public bool Contains(WorldObject o)
		{
			return this.Contains(o.ScreenPos());
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x001C168C File Offset: 0x001BFA8C
		public bool Contains(Vector2 screenPoint)
		{
			return screenPoint.x + 0.5f > this.LeftX && screenPoint.x - 0.5f < this.RightX && screenPoint.y + 0.5f > this.BotZ && screenPoint.y - 0.5f < this.TopZ;
		}
	}
}
