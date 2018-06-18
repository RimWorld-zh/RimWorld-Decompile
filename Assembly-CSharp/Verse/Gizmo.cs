using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6D RID: 3693
	public abstract class Gizmo
	{
		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x060056D8 RID: 22232 RVA: 0x00106788 File Offset: 0x00104B88
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x060056D9 RID: 22233 RVA: 0x001067A0 File Offset: 0x00104BA0
		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x060056DA RID: 22234
		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth);

		// Token: 0x060056DB RID: 22235 RVA: 0x001067C3 File Offset: 0x00104BC3
		public virtual void GizmoUpdateOnMouseover()
		{
		}

		// Token: 0x060056DC RID: 22236
		public abstract float GetWidth(float maxWidth);

		// Token: 0x060056DD RID: 22237 RVA: 0x001067C6 File Offset: 0x00104BC6
		public virtual void ProcessInput(Event ev)
		{
		}

		// Token: 0x060056DE RID: 22238 RVA: 0x001067CC File Offset: 0x00104BCC
		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		// Token: 0x060056DF RID: 22239 RVA: 0x001067E2 File Offset: 0x00104BE2
		public virtual void MergeWith(Gizmo other)
		{
		}

		// Token: 0x060056E0 RID: 22240 RVA: 0x001067E8 File Offset: 0x00104BE8
		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		// Token: 0x060056E1 RID: 22241 RVA: 0x00106804 File Offset: 0x00104C04
		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x00106820 File Offset: 0x00104C20
		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}

		// Token: 0x0400398F RID: 14735
		public bool disabled;

		// Token: 0x04003990 RID: 14736
		public string disabledReason;

		// Token: 0x04003991 RID: 14737
		public bool alsoClickIfOtherInGroupClicked = true;

		// Token: 0x04003992 RID: 14738
		public float order = 0f;

		// Token: 0x04003993 RID: 14739
		public const float Height = 75f;
	}
}
