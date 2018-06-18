using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000640 RID: 1600
	internal class ScenPart_CreateIncident : ScenPart_IncidentBase
	{
		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x0600211B RID: 8475 RVA: 0x001199EC File Offset: 0x00117DEC
		protected override string IncidentTag
		{
			get
			{
				return "CreateIncident";
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x0600211C RID: 8476 RVA: 0x00119A08 File Offset: 0x00117E08
		private float IntervalTicks
		{
			get
			{
				return 60000f * this.intervalDays;
			}
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x00119A2C File Offset: 0x00117E2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalDays, "intervalDays", 0f, false);
			Scribe_Values.Look<bool>(ref this.repeat, "repeat", false, false);
			Scribe_Values.Look<float>(ref this.occurTick, "occurTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.isFinished, "isFinished", false, false);
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x00119A90 File Offset: 0x00117E90
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

		// Token: 0x0600211F RID: 8479 RVA: 0x00119B98 File Offset: 0x00117F98
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

		// Token: 0x06002120 RID: 8480 RVA: 0x00119BFC File Offset: 0x00117FFC
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x00119C1F File Offset: 0x0011801F
		public override void PostGameStart()
		{
			base.PostGameStart();
			this.occurTick = (float)Find.TickManager.TicksGame + this.IntervalTicks;
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00119C40 File Offset: 0x00118040
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

		// Token: 0x040012DE RID: 4830
		private const float IntervalMidpoint = 30f;

		// Token: 0x040012DF RID: 4831
		private const float IntervalDeviation = 15f;

		// Token: 0x040012E0 RID: 4832
		private float intervalDays = 0f;

		// Token: 0x040012E1 RID: 4833
		private bool repeat = false;

		// Token: 0x040012E2 RID: 4834
		private string intervalDaysBuffer;

		// Token: 0x040012E3 RID: 4835
		private float occurTick = 0f;

		// Token: 0x040012E4 RID: 4836
		private bool isFinished = false;
	}
}
