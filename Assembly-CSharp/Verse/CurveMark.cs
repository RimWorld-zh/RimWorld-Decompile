using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF6 RID: 3830
	public struct CurveMark
	{
		// Token: 0x06005B70 RID: 23408 RVA: 0x002E8E18 File Offset: 0x002E7218
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x06005B71 RID: 23409 RVA: 0x002E8E30 File Offset: 0x002E7230
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x06005B72 RID: 23410 RVA: 0x002E8E4C File Offset: 0x002E724C
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06005B73 RID: 23411 RVA: 0x002E8E68 File Offset: 0x002E7268
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x04003C9A RID: 15514
		private float x;

		// Token: 0x04003C9B RID: 15515
		private string message;

		// Token: 0x04003C9C RID: 15516
		private Color color;
	}
}
