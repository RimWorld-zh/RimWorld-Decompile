using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CB5 RID: 3253
	public class WeatherEventHandler
	{
		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x060047AA RID: 18346 RVA: 0x0025B73C File Offset: 0x00259B3C
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x0025B757 File Offset: 0x00259B57
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x0025B76C File Offset: 0x00259B6C
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

		// Token: 0x060047AD RID: 18349 RVA: 0x0025B7D0 File Offset: 0x00259BD0
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}

		// Token: 0x0400309B RID: 12443
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();
	}
}
