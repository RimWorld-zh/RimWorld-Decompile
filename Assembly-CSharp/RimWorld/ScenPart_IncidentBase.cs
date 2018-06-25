using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000641 RID: 1601
	public abstract class ScenPart_IncidentBase : ScenPart
	{
		// Token: 0x040012E7 RID: 4839
		protected IncidentDef incident;

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06002129 RID: 8489 RVA: 0x00119854 File Offset: 0x00117C54
		public IncidentDef Incident
		{
			get
			{
				return this.incident;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600212A RID: 8490
		protected abstract string IncidentTag { get; }

		// Token: 0x0600212B RID: 8491 RVA: 0x00119870 File Offset: 0x00117C70
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

		// Token: 0x0600212C RID: 8492 RVA: 0x001198DC File Offset: 0x00117CDC
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			this.DoIncidentEditInterface(scenPartRect);
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x00119900 File Offset: 0x00117D00
		public override string Summary(Scenario scen)
		{
			string key = "ScenPart_" + this.IncidentTag;
			return ScenSummaryList.SummaryWithList(scen, this.IncidentTag, key.Translate());
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x00119938 File Offset: 0x00117D38
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == this.IncidentTag)
			{
				yield return this.incident.LabelCap;
			}
			yield break;
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x00119969 File Offset: 0x00117D69
		public override void Randomize()
		{
			this.incident = this.RandomizableIncidents().RandomElement<IncidentDef>();
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x00119980 File Offset: 0x00117D80
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident;
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x001199BC File Offset: 0x00117DBC
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase == null || scenPart_IncidentBase.Incident != this.incident;
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x001199F8 File Offset: 0x00117DF8
		protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return Enumerable.Empty<IncidentDef>();
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x00119A14 File Offset: 0x00117E14
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
	}
}
