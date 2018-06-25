using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6F RID: 3695
	public abstract class Gizmo
	{
		// Token: 0x040039A6 RID: 14758
		public bool disabled;

		// Token: 0x040039A7 RID: 14759
		public string disabledReason;

		// Token: 0x040039A8 RID: 14760
		public bool alsoClickIfOtherInGroupClicked = true;

		// Token: 0x040039A9 RID: 14761
		public float order = 0f;

		// Token: 0x040039AA RID: 14762
		public const float Height = 75f;

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x060056FC RID: 22268 RVA: 0x00106B94 File Offset: 0x00104F94
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x060056FD RID: 22269 RVA: 0x00106BAC File Offset: 0x00104FAC
		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x060056FE RID: 22270
		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth);

		// Token: 0x060056FF RID: 22271 RVA: 0x00106BCF File Offset: 0x00104FCF
		public virtual void GizmoUpdateOnMouseover()
		{
		}

		// Token: 0x06005700 RID: 22272
		public abstract float GetWidth(float maxWidth);

		// Token: 0x06005701 RID: 22273 RVA: 0x00106BD2 File Offset: 0x00104FD2
		public virtual void ProcessInput(Event ev)
		{
		}

		// Token: 0x06005702 RID: 22274 RVA: 0x00106BD8 File Offset: 0x00104FD8
		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		// Token: 0x06005703 RID: 22275 RVA: 0x00106BEE File Offset: 0x00104FEE
		public virtual void MergeWith(Gizmo other)
		{
		}

		// Token: 0x06005704 RID: 22276 RVA: 0x00106BF4 File Offset: 0x00104FF4
		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		// Token: 0x06005705 RID: 22277 RVA: 0x00106C10 File Offset: 0x00105010
		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x00106C2C File Offset: 0x0010502C
		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}
	}
}
