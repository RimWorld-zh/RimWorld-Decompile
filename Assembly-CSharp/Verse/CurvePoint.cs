using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFA RID: 3834
	public struct CurvePoint
	{
		// Token: 0x04003CB9 RID: 15545
		private Vector2 loc;

		// Token: 0x06005B9E RID: 23454 RVA: 0x002EB2E7 File Offset: 0x002E96E7
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x002EB2F7 File Offset: 0x002E96F7
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06005BA0 RID: 23456 RVA: 0x002EB304 File Offset: 0x002E9704
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x06005BA1 RID: 23457 RVA: 0x002EB320 File Offset: 0x002E9720
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x06005BA2 RID: 23458 RVA: 0x002EB340 File Offset: 0x002E9740
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		// Token: 0x06005BA3 RID: 23459 RVA: 0x002EB360 File Offset: 0x002E9760
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint((Vector2)ParseHelper.FromString(str, typeof(Vector2)));
		}

		// Token: 0x06005BA4 RID: 23460 RVA: 0x002EB390 File Offset: 0x002E9790
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		// Token: 0x06005BA5 RID: 23461 RVA: 0x002EB3B0 File Offset: 0x002E97B0
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}
	}
}
