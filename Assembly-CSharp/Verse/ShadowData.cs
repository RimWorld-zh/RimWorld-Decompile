using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C51 RID: 3153
	public class ShadowData
	{
		// Token: 0x04002F75 RID: 12149
		public Vector3 volume = Vector3.one;

		// Token: 0x04002F76 RID: 12150
		public Vector3 offset = Vector3.zero;

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06004571 RID: 17777 RVA: 0x0024BB68 File Offset: 0x00249F68
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06004572 RID: 17778 RVA: 0x0024BB88 File Offset: 0x00249F88
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06004573 RID: 17779 RVA: 0x0024BBA8 File Offset: 0x00249FA8
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}
	}
}
