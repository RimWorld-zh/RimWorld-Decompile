using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6B RID: 3691
	public class Command_Toggle : Command
	{
		// Token: 0x04003999 RID: 14745
		public Func<bool> isActive = null;

		// Token: 0x0400399A RID: 14746
		public Action toggleAction;

		// Token: 0x0400399B RID: 14747
		public SoundDef turnOnSound = SoundDefOf.Checkbox_TurnedOn;

		// Token: 0x0400399C RID: 14748
		public SoundDef turnOffSound = SoundDefOf.Checkbox_TurnedOff;

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x060056EE RID: 22254 RVA: 0x002CC8C8 File Offset: 0x002CACC8
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

		// Token: 0x060056EF RID: 22255 RVA: 0x002CC8FF File Offset: 0x002CACFF
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		// Token: 0x060056F0 RID: 22256 RVA: 0x002CC914 File Offset: 0x002CAD14
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(loc, maxWidth);
			Rect rect = new Rect(loc.x, loc.y, this.GetWidth(maxWidth), 75f);
			Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
			Texture2D image = (!this.isActive()) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x060056F1 RID: 22257 RVA: 0x002CC9AC File Offset: 0x002CADAC
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}
	}
}
