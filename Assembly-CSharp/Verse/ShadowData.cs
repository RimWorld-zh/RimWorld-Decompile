using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C53 RID: 3155
	public class ShadowData
	{
		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06004567 RID: 17767 RVA: 0x0024A6E4 File Offset: 0x00248AE4
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06004568 RID: 17768 RVA: 0x0024A704 File Offset: 0x00248B04
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06004569 RID: 17769 RVA: 0x0024A724 File Offset: 0x00248B24
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}

		// Token: 0x04002F6D RID: 12141
		public Vector3 volume = Vector3.one;

		// Token: 0x04002F6E RID: 12142
		public Vector3 offset = Vector3.zero;
	}
}
