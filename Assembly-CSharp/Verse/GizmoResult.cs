using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E6B RID: 3691
	public struct GizmoResult
	{
		// Token: 0x0400399C RID: 14748
		private GizmoState stateInt;

		// Token: 0x0400399D RID: 14749
		private Event interactEventInt;

		// Token: 0x060056F3 RID: 22259 RVA: 0x002CC8DB File Offset: 0x002CACDB
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x002CC8EC File Offset: 0x002CACEC
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x060056F5 RID: 22261 RVA: 0x002CC900 File Offset: 0x002CAD00
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x060056F6 RID: 22262 RVA: 0x002CC91C File Offset: 0x002CAD1C
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}
	}
}
