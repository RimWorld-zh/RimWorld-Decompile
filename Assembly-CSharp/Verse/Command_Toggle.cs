using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6A RID: 3690
	public class Command_Toggle : Command
	{
		// Token: 0x04003991 RID: 14737
		public Func<bool> isActive = null;

		// Token: 0x04003992 RID: 14738
		public Action toggleAction;

		// Token: 0x04003993 RID: 14739
		public SoundDef turnOnSound = SoundDefOf.Checkbox_TurnedOn;

		// Token: 0x04003994 RID: 14740
		public SoundDef turnOffSound = SoundDefOf.Checkbox_TurnedOff;

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x060056EE RID: 22254 RVA: 0x002CC6DC File Offset: 0x002CAADC
		public override SoundDef CurActivateSound
		{
			get
			{
				SoundDef result;
				if (this.isActive())
				{
					result = this.turnOffSound;
				}
				else
				{
					result = this.turnOnSound;
				}
				return result;
			}
		}

		// Token: 0x060056EF RID: 22255 RVA: 0x002CC713 File Offset: 0x002CAB13
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		// Token: 0x060056F0 RID: 22256 RVA: 0x002CC728 File Offset: 0x002CAB28
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(loc, maxWidth);
			Rect rect = new Rect(loc.x, loc.y, this.GetWidth(maxWidth), 75f);
			Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
			Texture2D image = (!this.isActive()) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x060056F1 RID: 22257 RVA: 0x002CC7C0 File Offset: 0x002CABC0
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}
	}
}
