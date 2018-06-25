using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CB4 RID: 3252
	public class WeatherEventHandler
	{
		// Token: 0x040030A6 RID: 12454
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x060047B6 RID: 18358 RVA: 0x0025CC08 File Offset: 0x0025B008
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x0025CC23 File Offset: 0x0025B023
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x0025CC38 File Offset: 0x0025B038
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

		// Token: 0x060047B9 RID: 18361 RVA: 0x0025CC9C File Offset: 0x0025B09C
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}
	}
}
