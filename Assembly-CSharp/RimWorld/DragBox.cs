using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000846 RID: 2118
	public class DragBox
	{
		// Token: 0x040019F2 RID: 6642
		public bool active;

		// Token: 0x040019F3 RID: 6643
		public Vector3 start;

		// Token: 0x040019F4 RID: 6644
		private const float DragBoxMinDiagonal = 0.5f;

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06002FEE RID: 12270 RVA: 0x001A1464 File Offset: 0x0019F864
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002FEF RID: 12271 RVA: 0x001A1498 File Offset: 0x0019F898
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002FF0 RID: 12272 RVA: 0x001A14CC File Offset: 0x0019F8CC
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06002FF1 RID: 12273 RVA: 0x001A1500 File Offset: 0x0019F900
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x001A1534 File Offset: 0x0019F934
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

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x001A160C File Offset: 0x0019FA0C
		public bool IsValid
		{
			get
			{
				return (this.start - UI.MouseMapPosition()).magnitude > 0.5f;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06002FF4 RID: 12276 RVA: 0x001A1640 File Offset: 0x0019FA40
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x001A1669 File Offset: 0x0019FA69
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2);
			}
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x001A1684 File Offset: 0x0019FA84
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

		// Token: 0x06002FF7 RID: 12279 RVA: 0x001A170C File Offset: 0x0019FB0C
		public bool Contains(Vector3 v)
		{
			return v.x + 0.5f > this.LeftX && v.x - 0.5f < this.RightX && v.z + 0.5f > this.BotZ && v.z - 0.5f < this.TopZ;
		}
	}
}
