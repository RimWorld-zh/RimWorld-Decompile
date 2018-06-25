using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CB5 RID: 3253
	public class WeatherEventHandler
	{
		// Token: 0x040030AD RID: 12461
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x060047B6 RID: 18358 RVA: 0x0025CEE8 File Offset: 0x0025B2E8
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x0025CF03 File Offset: 0x0025B303
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x0025CF18 File Offset: 0x0025B318
		public void WeatherEventHandlerTick()
		{
			for (int i = this.liveEvents.Count - 1; i >= 0; i--)
			{
				this.liveEvents[i].WeatherEventTick();
				if (this.liveEvents[i].Expired)
				{
					this.liveEvents.RemoveAt(i);
				}
			}
		}

		// Token: 0x060047B9 RID: 18361 RVA: 0x0025CF7C File Offset: 0x0025B37C
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}
	}
}
