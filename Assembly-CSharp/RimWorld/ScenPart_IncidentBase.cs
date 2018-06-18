using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000643 RID: 1603
	public abstract class ScenPart_IncidentBase : ScenPart
	{
		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x0600212D RID: 8493 RVA: 0x00119658 File Offset: 0x00117A58
		public IncidentDef Incident
		{
			get
			{
				return this.incident;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600212E RID: 8494
		protected abstract string IncidentTag { get; }

		// Token: 0x0600212F RID: 8495 RVA: 0x00119673 File Offset: 0x00117A73
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x0011968C File Offset: 0x00117A8C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			this.DoIncidentEditInterface(scenPartRect);
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x001196B0 File Offset: 0x00117AB0
		public override string Summary(Scenario scen)
		{
			string key = "ScenPart_" + this.IncidentTag;
			return ScenSummaryList.SummaryWithList(scen, this.IncidentTag, key.Translate());
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x001196E8 File Offset: 0x00117AE8
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == this.IncidentTag)
			{
				yield return this.incident.LabelCap;
			}
			yield break;
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x00119719 File Offset: 0x00117B19
		public override void Randomize()
		{
			this.incident = this.RandomizableIncidents().RandomElement<IncidentDef>();
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x00119730 File Offset: 0x00117B30
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident;
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x0011976C File Offset: 0x00117B6C
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase == null || scenPart_IncidentBase.Incident != this.incident;
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x001197A8 File Offset: 0x00117BA8
		protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return Enumerable.Empty<IncidentDef>();
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x001197C4 File Offset: 0x00117BC4
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

		// Token: 0x040012EA RID: 4842
		protected IncidentDef incident;
	}
}
