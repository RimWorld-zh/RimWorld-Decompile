using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6E RID: 3694
	public abstract class Gizmo
	{
		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x060056DA RID: 22234 RVA: 0x00106710 File Offset: 0x00104B10
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x060056DB RID: 22235 RVA: 0x00106728 File Offset: 0x00104B28
		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x060056DC RID: 22236
		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth);

		// Token: 0x060056DD RID: 22237 RVA: 0x0010674B File Offset: 0x00104B4B
		public virtual void GizmoUpdateOnMouseover()
		{
		}

		// Token: 0x060056DE RID: 22238
		public abstract float GetWidth(float maxWidth);

		// Token: 0x060056DF RID: 22239 RVA: 0x0010674E File Offset: 0x00104B4E
		public virtual void ProcessInput(Event ev)
		{
		}

		// Token: 0x060056E0 RID: 22240 RVA: 0x00106754 File Offset: 0x00104B54
		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		// Token: 0x060056E1 RID: 22241 RVA: 0x0010676A File Offset: 0x00104B6A
		public virtual void MergeWith(Gizmo other)
		{
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x00106770 File Offset: 0x00104B70
		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		// Token: 0x060056E3 RID: 22243 RVA: 0x0010678C File Offset: 0x00104B8C
		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		// Token: 0x060056E4 RID: 22244 RVA: 0x001067A8 File Offset: 0x00104BA8
		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}

		// Token: 0x04003991 RID: 14737
		public bool disabled;

		// Token: 0x04003992 RID: 14738
		public string disabledReason;

		// Token: 0x04003993 RID: 14739
		public bool alsoClickIfOtherInGroupClicked = true;

		// Token: 0x04003994 RID: 14740
		public float order = 0f;

		// Token: 0x04003995 RID: 14741
		public const float Height = 75f;
	}
}
