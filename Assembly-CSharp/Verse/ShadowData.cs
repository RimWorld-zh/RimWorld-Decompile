using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C52 RID: 3154
	public class ShadowData
	{
		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06004565 RID: 17765 RVA: 0x0024A6BC File Offset: 0x00248ABC
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06004566 RID: 17766 RVA: 0x0024A6DC File Offset: 0x00248ADC
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06004567 RID: 17767 RVA: 0x0024A6FC File Offset: 0x00248AFC
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}

		// Token: 0x04002F6B RID: 12139
		public Vector3 volume = Vector3.one;

		// Token: 0x04002F6C RID: 12140
		public Vector3 offset = Vector3.zero;
	}
}
