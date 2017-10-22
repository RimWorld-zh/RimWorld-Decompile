using UnityEngine;

namespace Verse
{
	public class MoteText : MoteThrown
	{
		public string text;

		public Color textColor = Color.white;

		public float overrideTimeBeforeStartFadeout = -1f;

		protected float TimeBeforeStartFadeout
		{
			get
			{
				return (!(this.overrideTimeBeforeStartFadeout >= 0.0)) ? base.def.mote.solidTime : this.overrideTimeBeforeStartFadeout;
			}
		}

		protected override float LifespanSecs
		{
			get
			{
				return this.TimeBeforeStartFadeout + base.def.mote.fadeOutTime;
			}
		}

		public override void Draw()
		{
		}

		public override void DrawGUIOverlay()
		{
			float a = (float)(1.0 - (base.AgeSecs - this.TimeBeforeStartFadeout) / base.def.mote.fadeOutTime);
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(base.exactPosition.x, base.exactPosition.z), this.text, color);
		}
	}
}
