using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EF7 RID: 3831
	public struct CurvePoint
	{
		// Token: 0x06005B74 RID: 23412 RVA: 0x002E8E83 File Offset: 0x002E7283
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		// Token: 0x06005B75 RID: 23413 RVA: 0x002E8E93 File Offset: 0x002E7293
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06005B76 RID: 23414 RVA: 0x002E8EA0 File Offset: 0x002E72A0
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06005B77 RID: 23415 RVA: 0x002E8EBC File Offset: 0x002E72BC
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06005B78 RID: 23416 RVA: 0x002E8EDC File Offset: 0x002E72DC
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		// Token: 0x06005B79 RID: 23417 RVA: 0x002E8EFC File Offset: 0x002E72FC
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint((Vector2)ParseHelper.FromString(str, typeof(Vector2)));
		}

		// Token: 0x06005B7A RID: 23418 RVA: 0x002E8F2C File Offset: 0x002E732C
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		// Token: 0x06005B7B RID: 23419 RVA: 0x002E8F4C File Offset: 0x002E734C
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}

		// Token: 0x04003C9D RID: 15517
		private Vector2 loc;
	}
}
