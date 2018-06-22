using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200063F RID: 1599
	public abstract class ScenPart_IncidentBase : ScenPart
	{
		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06002125 RID: 8485 RVA: 0x00119704 File Offset: 0x00117B04
		public IncidentDef Incident
		{
			get
			{
				return this.incident;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06002126 RID: 8486
		protected abstract string IncidentTag { get; }

		// Token: 0x06002127 RID: 8487 RVA: 0x00119720 File Offset: 0x00117B20
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.incident == null)
				{
					this.incident = this.RandomizableIncidents().FirstOrDefault<IncidentDef>();
					Log.Error("ScenPart has null incident after loading. Changing to " + this.incident.ToStringSafe<IncidentDef>(), false);
				}
			}
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x0011978C File Offset: 0x00117B8C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			this.DoIncidentEditInterface(scenPartRect);
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x001197B0 File Offset: 0x00117BB0
		public override string Summary(Scenario scen)
		{
			string key = "ScenPart_" + this.IncidentTag;
			return ScenSummaryList.SummaryWithList(scen, this.IncidentTag, key.Translate());
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x001197E8 File Offset: 0x00117BE8
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == this.IncidentTag)
			{
				yield return this.incident.LabelCap;
			}
			yield break;
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x00119819 File Offset: 0x00117C19
		public override void Randomize()
		{
			this.incident = this.RandomizableIncidents().RandomElement<IncidentDef>();
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x00119830 File Offset: 0x00117C30
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident;
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x0011986C File Offset: 0x00117C6C
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase == null || scenPart_IncidentBase.Incident != this.incident;
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x001198A8 File Offset: 0x00117CA8
		protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return Enumerable.Empty<IncidentDef>();
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x001198C4 File Offset: 0x00117CC4
		protected void DoIncidentEditInterface(Rect rect)
		{
			if (Widgets.ButtonText(rect, this.incident.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu<IncidentDef>(DefDatabase<IncidentDef>.AllDefs, (IncidentDef id) => id.LabelCap, (IncidentDef id) => delegate()
				{
					this.incident = id;
				});
			}
		}

		// Token: 0x040012E7 RID: 4839
		protected IncidentDef incident;
	}
}
