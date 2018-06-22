using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6C RID: 3692
	public abstract class Gizmo
	{
		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x060056F8 RID: 22264 RVA: 0x001067DC File Offset: 0x00104BDC
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x060056F9 RID: 22265 RVA: 0x001067F4 File Offset: 0x00104BF4
		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x060056FA RID: 22266
		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth);

		// Token: 0x060056FB RID: 22267 RVA: 0x00106817 File Offset: 0x00104C17
		public virtual void GizmoUpdateOnMouseover()
		{
		}

		// Token: 0x060056FC RID: 22268
		public abstract float GetWidth(float maxWidth);

		// Token: 0x060056FD RID: 22269 RVA: 0x0010681A File Offset: 0x00104C1A
		public virtual void ProcessInput(Event ev)
		{
		}

		// Token: 0x060056FE RID: 22270 RVA: 0x00106820 File Offset: 0x00104C20
		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		// Token: 0x060056FF RID: 22271 RVA: 0x00106836 File Offset: 0x00104C36
		public virtual void MergeWith(Gizmo other)
		{
		}

		// Token: 0x06005700 RID: 22272 RVA: 0x0010683C File Offset: 0x00104C3C
		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		// Token: 0x06005701 RID: 22273 RVA: 0x00106858 File Offset: 0x00104C58
		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		// Token: 0x06005702 RID: 22274 RVA: 0x00106874 File Offset: 0x00104C74
		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}

		// Token: 0x0400399E RID: 14750
		public bool disabled;

		// Token: 0x0400399F RID: 14751
		public string disabledReason;

		// Token: 0x040039A0 RID: 14752
		public bool alsoClickIfOtherInGroupClicked = true;

		// Token: 0x040039A1 RID: 14753
		public float order = 0f;

		// Token: 0x040039A2 RID: 14754
		public const float Height = 75f;
	}
}
