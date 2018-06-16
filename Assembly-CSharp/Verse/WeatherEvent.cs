using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB5 RID: 3253
	public abstract class WeatherEvent
	{
		// Token: 0x060047A2 RID: 18338 RVA: 0x000A46FB File Offset: 0x000A2AFB
		public WeatherEvent(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x060047A3 RID: 18339
		public abstract bool Expired { get; }

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x060047A4 RID: 18340 RVA: 0x000A470C File Offset: 0x000A2B0C
		public bool CurrentlyAffectsSky
		{
			get
			{
				return this.SkyTargetLerpFactor > 0f;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x060047A5 RID: 18341 RVA: 0x000A472E File Offset: 0x000A2B2E
		public virtual SkyTarget SkyTarget
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x060047A6 RID: 18342 RVA: 0x000A4738 File Offset: 0x000A2B38
		public virtual float SkyTargetLerpFactor
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x060047A7 RID: 18343 RVA: 0x000A4754 File Offset: 0x000A2B54
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060047A8 RID: 18344
		public abstract void FireEvent();

		// Token: 0x060047A9 RID: 18345
		public abstract void WeatherEventTick();

		// Token: 0x060047AA RID: 18346 RVA: 0x000A4772 File Offset: 0x000A2B72
		public virtual void WeatherEventDraw()
		{
		}

		// Token: 0x0400309C RID: 12444
		protected Map map;
	}
}
