using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF6 RID: 3830
	public struct CurvePoint
	{
		// Token: 0x06005B72 RID: 23410 RVA: 0x002E8F5F File Offset: 0x002E735F
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		// Token: 0x06005B73 RID: 23411 RVA: 0x002E8F6F File Offset: 0x002E736F
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06005B74 RID: 23412 RVA: 0x002E8F7C File Offset: 0x002E737C
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06005B75 RID: 23413 RVA: 0x002E8F98 File Offset: 0x002E7398
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06005B76 RID: 23414 RVA: 0x002E8FB8 File Offset: 0x002E73B8
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		// Token: 0x06005B77 RID: 23415 RVA: 0x002E8FD8 File Offset: 0x002E73D8
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint((Vector2)ParseHelper.FromString(str, typeof(Vector2)));
		}

		// Token: 0x06005B78 RID: 23416 RVA: 0x002E9008 File Offset: 0x002E7408
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		// Token: 0x06005B79 RID: 23417 RVA: 0x002E9028 File Offset: 0x002E7428
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}

		// Token: 0x04003C9C RID: 15516
		private Vector2 loc;
	}
}
