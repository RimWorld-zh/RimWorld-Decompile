using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6D RID: 3693
	public struct GizmoResult
	{
		// Token: 0x0400399C RID: 14748
		private GizmoState stateInt;

		// Token: 0x0400399D RID: 14749
		private Event interactEventInt;

		// Token: 0x060056F7 RID: 22263 RVA: 0x002CCA07 File Offset: 0x002CAE07
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x002CCA18 File Offset: 0x002CAE18
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x060056F9 RID: 22265 RVA: 0x002CCA2C File Offset: 0x002CAE2C
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x060056FA RID: 22266 RVA: 0x002CCA48 File Offset: 0x002CAE48
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}
	}
}
