using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB3 RID: 3251
	public abstract class WeatherEvent
	{
		// Token: 0x040030A5 RID: 12453
		protected Map map;

		// Token: 0x060047AC RID: 18348 RVA: 0x000A4867 File Offset: 0x000A2C67
		public WeatherEvent(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x060047AD RID: 18349
		public abstract bool Expired { get; }

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x060047AE RID: 18350 RVA: 0x000A4878 File Offset: 0x000A2C78
		public bool CurrentlyAffectsSky
		{
			get
			{
				return this.SkyTargetLerpFactor > 0f;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x060047AF RID: 18351 RVA: 0x000A489A File Offset: 0x000A2C9A
		public virtual SkyTarget SkyTarget
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x060047B0 RID: 18352 RVA: 0x000A48A4 File Offset: 0x000A2CA4
		public virtual float SkyTargetLerpFactor
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x060047B1 RID: 18353 RVA: 0x000A48C0 File Offset: 0x000A2CC0
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060047B2 RID: 18354
		public abstract void FireEvent();

		// Token: 0x060047B3 RID: 18355
		public abstract void WeatherEventTick();

		// Token: 0x060047B4 RID: 18356 RVA: 0x000A48DE File Offset: 0x000A2CDE
		public virtual void WeatherEventDraw()
		{
		}
	}
}
