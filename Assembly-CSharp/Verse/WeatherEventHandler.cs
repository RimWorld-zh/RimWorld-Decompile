using System.Collections.Generic;

namespace Verse
{
	public class WeatherEventHandler
	{
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();

		public WeatherEvent OverridingWeatherEvent
		{
			get
			{
				for (int i = 0; i < this.liveEvents.Count; i++)
				{
					if (this.liveEvents[i].CurrentlyOverridesSky)
					{
						return this.liveEvents[i];
					}
				}
				return null;
			}
		}

		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		public void WeatherEventHandlerTick()
		{
			for (int num = this.liveEvents.Count - 1; num >= 0; num--)
			{
				this.liveEvents[num].WeatherEventTick();
				if (this.liveEvents[num].Expired)
				{
					this.liveEvents.RemoveAt(num);
				}
			}
		}

		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}
	}
}
