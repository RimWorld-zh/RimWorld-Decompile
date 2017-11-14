using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	internal class ScenPart_CreateIncident : ScenPart_IncidentBase
	{
		private const float IntervalMidpoint = 30f;

		private const float IntervalDeviation = 15f;

		private float intervalDays;

		private bool repeat;

		private string intervalDaysBuffer;

		private float occurTick;

		private bool isFinished;

		protected override string IncidentTag
		{
			get
			{
				return "CreateIncident";
			}
		}

		private float IntervalTicks
		{
			get
			{
				return (float)(60000.0 * this.intervalDays);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalDays, "intervalDays", 0f, false);
			Scribe_Values.Look<bool>(ref this.repeat, "repeat", false, false);
			Scribe_Values.Look<float>(ref this.occurTick, "occurTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.isFinished, "isFinished", false, false);
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, (float)(ScenPart.RowHeight * 3.0));
			Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, (float)(scenPartRect.height / 3.0));
			Rect rect2 = new Rect(scenPartRect.x, (float)(scenPartRect.y + scenPartRect.height / 3.0), scenPartRect.width, (float)(scenPartRect.height / 3.0));
			Rect rect3 = new Rect(scenPartRect.x, (float)(scenPartRect.y + scenPartRect.height * 2.0 / 3.0), scenPartRect.width, (float)(scenPartRect.height / 3.0));
			base.DoIncidentEditInterface(rect);
			Widgets.TextFieldNumericLabeled<float>(rect2, "intervalDays".Translate(), ref this.intervalDays, ref this.intervalDaysBuffer, 0f, 1E+09f);
			Widgets.CheckboxLabeled(rect3, "repeat".Translate(), ref this.repeat, false);
		}

		public override void Randomize()
		{
			base.Randomize();
			this.intervalDays = (float)(15.0 * Rand.Gaussian(0f, 1f) + 30.0);
			if (this.intervalDays < 0.0)
			{
				this.intervalDays = 0f;
			}
			this.repeat = (Rand.Range(0, 100) < 50);
		}

		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.Eclipse;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override void PostGameStart()
		{
			base.PostGameStart();
			this.occurTick = (float)Find.TickManager.TicksGame + this.IntervalTicks;
		}

		public override void Tick()
		{
			base.Tick();
			if (Find.AnyPlayerHomeMap != null && !this.isFinished)
			{
				if (base.incident == null)
				{
					Log.Error("Trying to tick ScenPart_CreateIncident but the incident is null");
					this.isFinished = true;
				}
				else if ((float)Find.TickManager.TicksGame >= this.occurTick)
				{
					IncidentParms parms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, base.incident.category, (from x in Find.Maps
					where x.IsPlayerHome
					select x).RandomElement());
					if (!base.incident.Worker.TryExecute(parms))
					{
						this.isFinished = true;
					}
					else if (this.repeat && this.intervalDays > 0.0)
					{
						this.occurTick += this.IntervalTicks;
					}
					else
					{
						this.isFinished = true;
					}
				}
			}
		}
	}
}
