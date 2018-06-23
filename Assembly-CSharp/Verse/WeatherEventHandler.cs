using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CB2 RID: 3250
	public class WeatherEventHandler
	{
		// Token: 0x040030A6 RID: 12454
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x060047B3 RID: 18355 RVA: 0x0025CB2C File Offset: 0x0025AF2C
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		// Token: 0x060047B4 RID: 18356 RVA: 0x0025CB47 File Offset: 0x0025AF47
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		// Token: 0x060047B5 RID: 18357 RVA: 0x0025CB5C File Offset: 0x0025AF5C
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

		// Token: 0x060047B6 RID: 18358 RVA: 0x0025CBC0 File Offset: 0x0025AFC0
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}
	}
}
