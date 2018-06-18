using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB4 RID: 3252
	public abstract class WeatherEvent
	{
		// Token: 0x060047A0 RID: 18336 RVA: 0x000A4707 File Offset: 0x000A2B07
		public WeatherEvent(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x060047A1 RID: 18337
		public abstract bool Expired { get; }

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x060047A2 RID: 18338 RVA: 0x000A4718 File Offset: 0x000A2B18
		public bool CurrentlyAffectsSky
		{
			get
			{
				return this.SkyTargetLerpFactor > 0f;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x060047A3 RID: 18339 RVA: 0x000A473A File Offset: 0x000A2B3A
		public virtual SkyTarget SkyTarget
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x060047A4 RID: 18340 RVA: 0x000A4744 File Offset: 0x000A2B44
		public virtual float SkyTargetLerpFactor
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x060047A5 RID: 18341 RVA: 0x000A4760 File Offset: 0x000A2B60
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060047A6 RID: 18342
		public abstract void FireEvent();

		// Token: 0x060047A7 RID: 18343
		public abstract void WeatherEventTick();

		// Token: 0x060047A8 RID: 18344 RVA: 0x000A477E File Offset: 0x000A2B7E
		public virtual void WeatherEventDraw()
		{
		}

		// Token: 0x0400309A RID: 12442
		protected Map map;
	}
}
