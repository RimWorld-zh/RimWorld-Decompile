using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF6 RID: 3830
	public struct CurvePoint
	{
		// Token: 0x06005B9A RID: 23450 RVA: 0x002EAF93 File Offset: 0x002E9393
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		// Token: 0x06005B9B RID: 23451 RVA: 0x002EAFA3 File Offset: 0x002E93A3
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x06005B9C RID: 23452 RVA: 0x002EAFB0 File Offset: 0x002E93B0
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x06005B9D RID: 23453 RVA: 0x002EAFCC File Offset: 0x002E93CC
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06005B9E RID: 23454 RVA: 0x002EAFEC File Offset: 0x002E93EC
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x002EB00C File Offset: 0x002E940C
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint((Vector2)ParseHelper.FromString(str, typeof(Vector2)));
		}

		// Token: 0x06005BA0 RID: 23456 RVA: 0x002EB03C File Offset: 0x002E943C
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		// Token: 0x06005BA1 RID: 23457 RVA: 0x002EB05C File Offset: 0x002E945C
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}

		// Token: 0x04003CAF RID: 15535
		private Vector2 loc;
	}
}
