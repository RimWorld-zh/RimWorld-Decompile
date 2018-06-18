using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF5 RID: 3829
	public struct CurveMark
	{
		// Token: 0x06005B6E RID: 23406 RVA: 0x002E8EF4 File Offset: 0x002E72F4
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x06005B6F RID: 23407 RVA: 0x002E8F0C File Offset: 0x002E730C
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x06005B70 RID: 23408 RVA: 0x002E8F28 File Offset: 0x002E7328
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x06005B71 RID: 23409 RVA: 0x002E8F44 File Offset: 0x002E7344
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x04003C99 RID: 15513
		private float x;

		// Token: 0x04003C9A RID: 15514
		private string message;

		// Token: 0x04003C9B RID: 15515
		private Color color;
	}
}
