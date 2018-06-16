using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CB6 RID: 3254
	public class WeatherEventHandler
	{
		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x060047AC RID: 18348 RVA: 0x0025B764 File Offset: 0x00259B64
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x0025B77F File Offset: 0x00259B7F
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x0025B794 File Offset: 0x00259B94
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

		// Token: 0x060047AF RID: 18351 RVA: 0x0025B7F8 File Offset: 0x00259BF8
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}

		// Token: 0x0400309D RID: 12445
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();
	}
}
