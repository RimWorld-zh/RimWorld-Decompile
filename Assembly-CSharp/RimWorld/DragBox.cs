using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000848 RID: 2120
	public class DragBox
	{
		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x001A0ECC File Offset: 0x0019F2CC
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x001A0F00 File Offset: 0x0019F300
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002FF4 RID: 12276 RVA: 0x001A0F34 File Offset: 0x0019F334
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002FF5 RID: 12277 RVA: 0x001A0F68 File Offset: 0x0019F368
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x001A0F9C File Offset: 0x0019F39C
		public Rect ScreenRect
		{
			get
			{
				Vector2 vector = this.start.MapToUIPosition();
				Vector2 mousePosition = Event.current.mousePosition;
				if (mousePosition.x < vector.x)
				{
					float x = mousePosition.x;
					mousePosition.x = vector.x;
					vector.x = x;
				}
				if (mousePosition.y < vector.y)
				{
					float y = mousePosition.y;
					mousePosition.y = vector.y;
					vector.y = y;
				}
				return new Rect
				{
					xMin = vector.x,
					xMax = mousePosition.x,
					yMin = vector.y,
					yMax = mousePosition.y
				};
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x001A1074 File Offset: 0x0019F474
		public bool IsValid
		{
			get
			{
				return (this.start - UI.MouseMapPosition()).magnitude > 0.5f;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x001A10A8 File Offset: 0x0019F4A8
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x001A10D1 File Offset: 0x0019F4D1
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2);
			}
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x001A10EC File Offset: 0x0019F4EC
		public bool Contains(Thing t)
		{
			bool result;
			if (t is Pawn)
			{
				result = this.Contains((t as Pawn).Drawer.DrawPos);
			}
			else
			{
				CellRect.CellRectIterator iterator = t.OccupiedRect().GetIterator();
				while (!iterator.Done())
				{
					IntVec3 intVec = iterator.Current;
					if (this.Contains(intVec.ToVector3Shifted()))
					{
						return true;
					}
					iterator.MoveNext();
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x001A1174 File Offset: 0x0019F574
		public bool Contains(Vector3 v)
		{
			return v.x + 0.5f > this.LeftX && v.x - 0.5f < this.RightX && v.z + 0.5f > this.BotZ && v.z - 0.5f < this.TopZ;
		}

		// Token: 0x040019F0 RID: 6640
		public bool active;

		// Token: 0x040019F1 RID: 6641
		public Vector3 start;

		// Token: 0x040019F2 RID: 6642
		private const float DragBoxMinDiagonal = 0.5f;
	}
}
