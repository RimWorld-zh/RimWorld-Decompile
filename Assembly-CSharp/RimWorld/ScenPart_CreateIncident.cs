using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063E RID: 1598
	internal class ScenPart_CreateIncident : ScenPart_IncidentBase
	{
		// Token: 0x040012DF RID: 4831
		private const float IntervalMidpoint = 30f;

		// Token: 0x040012E0 RID: 4832
		private const float IntervalDeviation = 15f;

		// Token: 0x040012E1 RID: 4833
		private float intervalDays = 0f;

		// Token: 0x040012E2 RID: 4834
		private bool repeat = false;

		// Token: 0x040012E3 RID: 4835
		private string intervalDaysBuffer;

		// Token: 0x040012E4 RID: 4836
		private float occurTick = 0f;

		// Token: 0x040012E5 RID: 4837
		private bool isFinished = false;

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06002116 RID: 8470 RVA: 0x00119EA4 File Offset: 0x001182A4
		protected override string IncidentTag
		{
			get
			{
				return "CreateIncident";
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06002117 RID: 8471 RVA: 0x00119EC0 File Offset: 0x001182C0
		private float IntervalTicks
		{
			get
			{
				return 60000f * this.intervalDays;
			}
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x00119EE4 File Offset: 0x001182E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalDays, "intervalDays", 0f, false);
			Scribe_Values.Look<bool>(ref this.repeat, "repeat", false, false);
			Scribe_Values.Look<float>(ref this.occurTick, "occurTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.isFinished, "isFinished", false, false);
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x00119F48 File Offset: 0x00118348
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect2 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect3 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 2f / 3f, scenPartRect.width, scenPartRect.height / 3f);
			base.DoIncidentEditInterface(rect);
			Widgets.TextFieldNumericLabeled<float>(rect2, "intervalDays".Translate(), ref this.intervalDays, ref this.intervalDaysBuffer, 0f, 1E+09f);
			Widgets.CheckboxLabeled(rect3, "repeat".Translate(), ref this.repeat, false, null, null, false);
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x0011A050 File Offset: 0x00118450
		public override void Randomize()
		{
			base.Randomize();
			this.intervalDays = 15f * Rand.Gaussian(0f, 1f) + 30f;
			if (this.intervalDays < 0f)
			{
				this.intervalDays = 0f;
			}
			this.repeat = (Rand.Range(0, 100) < 50);
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x0011A0B4 File Offset: 0x001184B4
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x0011A0D7 File Offset: 0x001184D7
		public override void PostGameStart()
		{
			base.PostGameStart();
			this.occurTick = (float)Find.TickManager.TicksGame + this.IntervalTicks;
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x0011A0F8 File Offset: 0x001184F8
		public override void Tick()
		{
			base.Tick();
			if (Find.AnyPlayerHomeMap != null)
			{
				if (!this.isFinished)
				{
					if (this.incident == null)
					{
						Log.Error("Trying to tick ScenPart_CreateIncident but the incident is null", false);
						this.isFinished = true;
					}
					else if ((float)Find.TickManager.TicksGame >= this.occurTick)
					{
						IncidentParms parms = StorytellerUtility.DefaultParmsNow(this.incident.category, (from x in Find.Maps
						where x.IsPlayerHome
						select x).RandomElement<Map>());
						if (!this.incident.Worker.TryExecute(parms))
						{
							this.isFinished = true;
						}
						else if (this.repeat && this.intervalDays > 0f)
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
}
