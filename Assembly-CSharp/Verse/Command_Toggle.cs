using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E68 RID: 3688
	public class Command_Toggle : Command
	{
		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x060056EA RID: 22250 RVA: 0x002CC5B0 File Offset: 0x002CA9B0
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

		// Token: 0x060056EB RID: 22251 RVA: 0x002CC5E7 File Offset: 0x002CA9E7
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x002CC5FC File Offset: 0x002CA9FC
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(loc, maxWidth);
			Rect rect = new Rect(loc.x, loc.y, this.GetWidth(maxWidth), 75f);
			Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
			Texture2D image = (!this.isActive()) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x060056ED RID: 22253 RVA: 0x002CC694 File Offset: 0x002CAA94
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}

		// Token: 0x04003991 RID: 14737
		public Func<bool> isActive = null;

		// Token: 0x04003992 RID: 14738
		public Action toggleAction;

		// Token: 0x04003993 RID: 14739
		public SoundDef turnOnSound = SoundDefOf.Checkbox_TurnedOn;

		// Token: 0x04003994 RID: 14740
		public SoundDef turnOffSound = SoundDefOf.Checkbox_TurnedOff;
	}
}
