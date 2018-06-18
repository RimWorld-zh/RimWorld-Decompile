using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6C RID: 3692
	public struct GizmoResult
	{
		// Token: 0x060056D3 RID: 22227 RVA: 0x002CACCB File Offset: 0x002C90CB
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		// Token: 0x060056D4 RID: 22228 RVA: 0x002CACDC File Offset: 0x002C90DC
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x060056D5 RID: 22229 RVA: 0x002CACF0 File Offset: 0x002C90F0
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x060056D6 RID: 22230 RVA: 0x002CAD0C File Offset: 0x002C910C
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}

		// Token: 0x0400398D RID: 14733
		private GizmoState stateInt;

		// Token: 0x0400398E RID: 14734
		private Event interactEventInt;
	}
}
