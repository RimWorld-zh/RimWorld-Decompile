using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063C RID: 1596
	internal class ScenPart_CreateIncident : ScenPart_IncidentBase
	{
		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06002113 RID: 8467 RVA: 0x00119AEC File Offset: 0x00117EEC
		protected override string IncidentTag
		{
			get
			{
				return "CreateIncident";
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06002114 RID: 8468 RVA: 0x00119B08 File Offset: 0x00117F08
		private float IntervalTicks
		{
			get
			{
				return 60000f * this.intervalDays;
			}
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x00119B2C File Offset: 0x00117F2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalDays, "intervalDays", 0f, false);
			Scribe_Values.Look<bool>(ref this.repeat, "repeat", false, false);
			Scribe_Values.Look<float>(ref this.occurTick, "occurTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.isFinished, "isFinished", false, false);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x00119B90 File Offset: 0x00117F90
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

		// Token: 0x06002117 RID: 8471 RVA: 0x00119C98 File Offset: 0x00118098
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

		// Token: 0x06002118 RID: 8472 RVA: 0x00119CFC File Offset: 0x001180FC
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x00119D1F File Offset: 0x0011811F
		public override void PostGameStart()
		{
			base.PostGameStart();
			this.occurTick = (float)Find.TickManager.TicksGame + this.IntervalTicks;
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x00119D40 File Offset: 0x00118140
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

		// Token: 0x040012DB RID: 4827
		private const float IntervalMidpoint = 30f;

		// Token: 0x040012DC RID: 4828
		private const float IntervalDeviation = 15f;

		// Token: 0x040012DD RID: 4829
		private float intervalDays = 0f;

		// Token: 0x040012DE RID: 4830
		private bool repeat = false;

		// Token: 0x040012DF RID: 4831
		private string intervalDaysBuffer;

		// Token: 0x040012E0 RID: 4832
		private float occurTick = 0f;

		// Token: 0x040012E1 RID: 4833
		private bool isFinished = false;
	}
}
