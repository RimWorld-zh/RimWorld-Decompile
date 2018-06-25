using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6E RID: 3694
	public struct GizmoResult
	{
		// Token: 0x040039A4 RID: 14756
		private GizmoState stateInt;

		// Token: 0x040039A5 RID: 14757
		private Event interactEventInt;

		// Token: 0x060056F7 RID: 22263 RVA: 0x002CCBF3 File Offset: 0x002CAFF3
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x002CCC04 File Offset: 0x002CB004
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x060056F9 RID: 22265 RVA: 0x002CCC18 File Offset: 0x002CB018
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x060056FA RID: 22266 RVA: 0x002CCC34 File Offset: 0x002CB034
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}
	}
}
