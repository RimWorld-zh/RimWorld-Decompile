using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldDragBox
	{
		private const float DragBoxMinDiagonal = 7f;

		public bool active;

		public Vector2 start;

		public float LeftX
		{
			get
			{
				float x = this.start.x;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				return Math.Min(x, mousePositionOnUIInverted.x);
			}
		}

		public float RightX
		{
			get
			{
				float x = this.start.x;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				return Math.Max(x, mousePositionOnUIInverted.x);
			}
		}

		public float BotZ
		{
			get
			{
				float y = this.start.y;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				return Math.Min(y, mousePositionOnUIInverted.y);
			}
		}

		public float TopZ
		{
			get
			{
				float y = this.start.y;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				return Math.Max(y, mousePositionOnUIInverted.y);
			}
		}

		public Rect ScreenRect
		{
			get
			{
				return new Rect(this.LeftX, this.BotZ, this.RightX - this.LeftX, this.TopZ - this.BotZ);
			}
		}

		public float Diagonal
		{
			get
			{
				Vector2 a = this.start;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				float x = mousePositionOnUIInverted.x;
				Vector2 mousePositionOnUIInverted2 = UI.MousePositionOnUIInverted;
				return (a - new Vector2(x, mousePositionOnUIInverted2.y)).magnitude;
			}
		}

		public bool IsValid
		{
			get
			{
				return this.Diagonal > 7.0;
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

		public bool Contains(WorldObject o)
		{
			return this.Contains(o.ScreenPos());
		}

		public bool Contains(Vector2 screenPoint)
		{
			if (screenPoint.x + 0.5 > this.LeftX && screenPoint.x - 0.5 < this.RightX && screenPoint.y + 0.5 > this.BotZ && screenPoint.y - 0.5 < this.TopZ)
			{
				return true;
			}
			return false;
		}
	}
}
