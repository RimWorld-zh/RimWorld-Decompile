using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C52 RID: 3154
	public class ShadowData
	{
		// Token: 0x04002F7C RID: 12156
		public Vector3 volume = Vector3.one;

		// Token: 0x04002F7D RID: 12157
		public Vector3 offset = Vector3.zero;

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06004571 RID: 17777 RVA: 0x0024BE48 File Offset: 0x0024A248
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06004572 RID: 17778 RVA: 0x0024BE68 File Offset: 0x0024A268
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06004573 RID: 17779 RVA: 0x0024BE88 File Offset: 0x0024A288
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}
	}
}
