using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class DragBox
	{
		private const float DragBoxMinDiagonal = 0.5f;

		public bool active;

		public Vector3 start;

		public float LeftX
		{
			get
			{
				float x = this.start.x;
				Vector3 vector = UI.MouseMapPosition();
				return Math.Min(x, vector.x);
			}
		}

		public float RightX
		{
			get
			{
				float x = this.start.x;
				Vector3 vector = UI.MouseMapPosition();
				return Math.Max(x, vector.x);
			}
		}

		public float BotZ
		{
			get
			{
				float z = this.start.z;
				Vector3 vector = UI.MouseMapPosition();
				return Math.Min(z, vector.z);
			}
		}

		public float TopZ
		{
			get
			{
				float z = this.start.z;
				Vector3 vector = UI.MouseMapPosition();
				return Math.Max(z, vector.z);
			}
		}

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

		public bool IsValid
		{
			get
			{
				return (this.start - UI.MouseMapPosition()).magnitude > 0.5;
			}
		}

		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2);
			}
		}

		public bool Contains(Thing t)
		{
			if (t is Pawn)
			{
				return this.Contains((t as Pawn).Drawer.DrawPos);
			}
			CellRect.CellRectIterator iterator = t.OccupiedRect().GetIterator();
			while (!iterator.Done())
			{
				if (this.Contains(iterator.Current.ToVector3Shifted()))
				{
					return true;
				}
				iterator.MoveNext();
			}
			return false;
		}

		public bool Contains(Vector3 v)
		{
			if (v.x + 0.5 > this.LeftX && v.x - 0.5 < this.RightX && v.z + 0.5 > this.BotZ && v.z - 0.5 < this.TopZ)
			{
				return true;
			}
			return false;
		}
	}
}
