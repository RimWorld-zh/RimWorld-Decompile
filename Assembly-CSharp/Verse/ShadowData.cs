using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C4F RID: 3151
	public class ShadowData
	{
		// Token: 0x04002F75 RID: 12149
		public Vector3 volume = Vector3.one;

		// Token: 0x04002F76 RID: 12150
		public Vector3 offset = Vector3.zero;

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x0600456E RID: 17774 RVA: 0x0024BA8C File Offset: 0x00249E8C
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x0600456F RID: 17775 RVA: 0x0024BAAC File Offset: 0x00249EAC
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06004570 RID: 17776 RVA: 0x0024BACC File Offset: 0x00249ECC
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}
	}
}
