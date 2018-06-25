using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6E RID: 3694
	public abstract class Gizmo
	{
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

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x060056FC RID: 22268 RVA: 0x0010692C File Offset: 0x00104D2C
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x060056FD RID: 22269 RVA: 0x00106944 File Offset: 0x00104D44
		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x060056FE RID: 22270
		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth);

		// Token: 0x060056FF RID: 22271 RVA: 0x00106967 File Offset: 0x00104D67
		public virtual void GizmoUpdateOnMouseover()
		{
		}

		// Token: 0x06005700 RID: 22272
		public abstract float GetWidth(float maxWidth);

		// Token: 0x06005701 RID: 22273 RVA: 0x0010696A File Offset: 0x00104D6A
		public virtual void ProcessInput(Event ev)
		{
		}

		// Token: 0x06005702 RID: 22274 RVA: 0x00106970 File Offset: 0x00104D70
		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		// Token: 0x06005703 RID: 22275 RVA: 0x00106986 File Offset: 0x00104D86
		public virtual void MergeWith(Gizmo other)
		{
		}

		// Token: 0x06005704 RID: 22276 RVA: 0x0010698C File Offset: 0x00104D8C
		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		// Token: 0x06005705 RID: 22277 RVA: 0x001069A8 File Offset: 0x00104DA8
		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x001069C4 File Offset: 0x00104DC4
		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}
	}
}
