using UnityEngine;

namespace Verse
{
	public class CompColorable : ThingComp
	{
		private Color color = Color.white;

		private bool active = false;

		public Color Color
		{
			get
			{
				return this.active ? this.color : base.parent.def.graphicData.color;
			}
			set
			{
				if (!(value == this.color))
				{
					this.active = true;
					this.color = value;
					base.parent.Notify_ColorChanged();
				}
			}
		}

		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (base.parent.def.colorGenerator != null)
			{
				if (base.parent.Stuff != null && !base.parent.Stuff.stuffProps.allowColorGenerators)
					return;
				this.Color = base.parent.def.colorGenerator.NewRandomizedColor();
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && !this.active)
				return;
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
		}

		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}
	}
}
