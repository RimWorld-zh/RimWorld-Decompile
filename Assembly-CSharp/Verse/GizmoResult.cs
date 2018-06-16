using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6D RID: 3693
	public struct GizmoResult
	{
		// Token: 0x060056D5 RID: 22229 RVA: 0x002CACCB File Offset: 0x002C90CB
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		// Token: 0x060056D6 RID: 22230 RVA: 0x002CACDC File Offset: 0x002C90DC
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x060056D7 RID: 22231 RVA: 0x002CACF0 File Offset: 0x002C90F0
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x060056D8 RID: 22232 RVA: 0x002CAD0C File Offset: 0x002C910C
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}

		// Token: 0x0400398F RID: 14735
		private GizmoState stateInt;

		// Token: 0x04003990 RID: 14736
		private Event interactEventInt;
	}
}
