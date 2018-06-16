using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6A RID: 3690
	public class Command_Toggle : Command
	{
		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x060056CC RID: 22220 RVA: 0x002CA9A0 File Offset: 0x002C8DA0
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

		// Token: 0x060056CD RID: 22221 RVA: 0x002CA9D7 File Offset: 0x002C8DD7
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		// Token: 0x060056CE RID: 22222 RVA: 0x002CA9EC File Offset: 0x002C8DEC
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(loc, maxWidth);
			Rect rect = new Rect(loc.x, loc.y, this.GetWidth(maxWidth), 75f);
			Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
			Texture2D image = (!this.isActive()) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x060056CF RID: 22223 RVA: 0x002CAA84 File Offset: 0x002C8E84
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}

		// Token: 0x04003984 RID: 14724
		public Func<bool> isActive = null;

		// Token: 0x04003985 RID: 14725
		public Action toggleAction;

		// Token: 0x04003986 RID: 14726
		public SoundDef turnOnSound = SoundDefOf.Checkbox_TurnedOn;

		// Token: 0x04003987 RID: 14727
		public SoundDef turnOffSound = SoundDefOf.Checkbox_TurnedOff;
	}
}
