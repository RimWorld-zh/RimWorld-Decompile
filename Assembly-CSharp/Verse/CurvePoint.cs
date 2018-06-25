using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF9 RID: 3833
	public struct CurvePoint
	{
		// Token: 0x04003CB1 RID: 15537
		private Vector2 loc;

		// Token: 0x06005B9E RID: 23454 RVA: 0x002EB0C7 File Offset: 0x002E94C7
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x002EB0D7 File Offset: 0x002E94D7
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06005BA0 RID: 23456 RVA: 0x002EB0E4 File Offset: 0x002E94E4
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x06005BA1 RID: 23457 RVA: 0x002EB100 File Offset: 0x002E9500
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x06005BA2 RID: 23458 RVA: 0x002EB120 File Offset: 0x002E9520
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		// Token: 0x06005BA3 RID: 23459 RVA: 0x002EB140 File Offset: 0x002E9540
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint((Vector2)ParseHelper.FromString(str, typeof(Vector2)));
		}

		// Token: 0x06005BA4 RID: 23460 RVA: 0x002EB170 File Offset: 0x002E9570
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		// Token: 0x06005BA5 RID: 23461 RVA: 0x002EB190 File Offset: 0x002E9590
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}
	}
}
