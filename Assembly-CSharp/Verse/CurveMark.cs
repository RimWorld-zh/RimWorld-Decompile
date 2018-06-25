using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF8 RID: 3832
	public struct CurveMark
	{
		// Token: 0x04003CAE RID: 15534
		private float x;

		// Token: 0x04003CAF RID: 15535
		private string message;

		// Token: 0x04003CB0 RID: 15536
		private Color color;

		// Token: 0x06005B9A RID: 23450 RVA: 0x002EB059 File Offset: 0x002E9459
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06005B9B RID: 23451 RVA: 0x002EB074 File Offset: 0x002E9474
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06005B9C RID: 23452 RVA: 0x002EB090 File Offset: 0x002E9490
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06005B9D RID: 23453 RVA: 0x002EB0AC File Offset: 0x002E94AC
		public Color Color
		{
			get
			{
				return this.color;
			}
		}
	}
}
