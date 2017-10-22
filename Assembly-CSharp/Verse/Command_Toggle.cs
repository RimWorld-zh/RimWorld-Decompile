using RimWorld;
using System;
using UnityEngine;

namespace Verse
{
	public class Command_Toggle : Command
	{
		public Func<bool> isActive = null;

		public Action toggleAction;

		public SoundDef turnOnSound = SoundDefOf.CheckboxTurnedOn;

		public SoundDef turnOffSound = SoundDefOf.CheckboxTurnedOff;

		public override SoundDef CurActivateSound
		{
			get
			{
				return (!this.isActive()) ? this.turnOnSound : this.turnOffSound;
			}
		}

		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		public override GizmoResult GizmoOnGUI(Vector2 loc)
		{
			GizmoResult result = base.GizmoOnGUI(loc);
			Rect rect = new Rect(loc.x, loc.y, this.Width, 75f);
			Rect position = new Rect((float)(rect.x + rect.width - 24.0), rect.y, 24f, 24f);
			Texture2D image = (!this.isActive()) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
			GUI.DrawTexture(position, image);
			return result;
		}

		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}
	}
}
