using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF9 RID: 3833
	public struct CurveMark
	{
		// Token: 0x04003CB6 RID: 15542
		private float x;

		// Token: 0x04003CB7 RID: 15543
		private string message;

		// Token: 0x04003CB8 RID: 15544
		private Color color;

		// Token: 0x06005B9A RID: 23450 RVA: 0x002EB279 File Offset: 0x002E9679
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06005B9B RID: 23451 RVA: 0x002EB294 File Offset: 0x002E9694
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06005B9C RID: 23452 RVA: 0x002EB2B0 File Offset: 0x002E96B0
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06005B9D RID: 23453 RVA: 0x002EB2CC File Offset: 0x002E96CC
		public Color Color
		{
			get
			{
				return this.color;
			}
		}
	}
}
