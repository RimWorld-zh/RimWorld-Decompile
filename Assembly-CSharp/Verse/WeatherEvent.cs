using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB1 RID: 3249
	public abstract class WeatherEvent
	{
		// Token: 0x040030A5 RID: 12453
		protected Map map;

		// Token: 0x060047A9 RID: 18345 RVA: 0x000A4717 File Offset: 0x000A2B17
		public WeatherEvent(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x060047AA RID: 18346
		public abstract bool Expired { get; }

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x060047AB RID: 18347 RVA: 0x000A4728 File Offset: 0x000A2B28
		public bool CurrentlyAffectsSky
		{
			get
			{
				return this.SkyTargetLerpFactor > 0f;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x060047AC RID: 18348 RVA: 0x000A474A File Offset: 0x000A2B4A
		public virtual SkyTarget SkyTarget
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x060047AD RID: 18349 RVA: 0x000A4754 File Offset: 0x000A2B54
		public virtual float SkyTargetLerpFactor
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x060047AE RID: 18350 RVA: 0x000A4770 File Offset: 0x000A2B70
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060047AF RID: 18351
		public abstract void FireEvent();

		// Token: 0x060047B0 RID: 18352
		public abstract void WeatherEventTick();

		// Token: 0x060047B1 RID: 18353 RVA: 0x000A478E File Offset: 0x000A2B8E
		public virtual void WeatherEventDraw()
		{
		}
	}
}
