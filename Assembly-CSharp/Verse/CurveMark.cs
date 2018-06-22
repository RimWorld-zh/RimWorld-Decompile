using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF5 RID: 3829
	public struct CurveMark
	{
		// Token: 0x06005B96 RID: 23446 RVA: 0x002EAF28 File Offset: 0x002E9328
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06005B97 RID: 23447 RVA: 0x002EAF40 File Offset: 0x002E9340
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06005B98 RID: 23448 RVA: 0x002EAF5C File Offset: 0x002E935C
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06005B99 RID: 23449 RVA: 0x002EAF78 File Offset: 0x002E9378
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x04003CAC RID: 15532
		private float x;

		// Token: 0x04003CAD RID: 15533
		private string message;

		// Token: 0x04003CAE RID: 15534
		private Color color;
	}
}
