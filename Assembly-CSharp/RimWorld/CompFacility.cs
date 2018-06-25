using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070F RID: 1807
	public class CompFacility : ThingComp
	{
		// Token: 0x040015D7 RID: 5591
		private List<Thing> linkedBuildings = new List<Thing>();

		// Token: 0x040015D8 RID: 5592
		private HashSet<Thing> thingsToNotify = new HashSet<Thing>();

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060027A3 RID: 10147 RVA: 0x00153E28 File Offset: 0x00152228
		public bool CanBeActive
		{
			get
			{
				CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
				return compPowerTrader == null || compPowerTrader.PowerOn;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x060027A4 RID: 10148 RVA: 0x00153E64 File Offset: 0x00152264
		public CompProperties_Facility Props
		{
			get
			{
				return (CompProperties_Facility)this.props;
			}
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x00153E84 File Offset: 0x00152284
		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompProperties_Facility compProperties = myDef.GetCompProperties<CompProperties_Facility>();
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			for (int i = 0; i < compProperties.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in map.listerThings.ThingsOfDef(compProperties.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanPotentiallyLinkTo(myDef, myPos, myRot))
					{
						GenDraw.DrawLineBetween(a, thing.TrueCenter());
						compAffectedByFacilities.DrawRedLineToPotentiallySupplantedFacility(myDef, myPos, myRot);
					}
				}
			}
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x00153F60 File Offset: 0x00152360
		public void Notify_NewLink(Thing thing)
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i] == thing)
				{
					Log.Error("Notify_NewLink was called but the link is already here.", false);
					return;
				}
			}
			this.linkedBuildings.Add(thing);
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x00153FBC File Offset: 0x001523BC
		public void Notify_LinkRemoved(Thing thing)
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i] == thing)
				{
					this.linkedBuildings.RemoveAt(i);
					return;
				}
			}
			Log.Error("Notify_LinkRemoved was called but there is no such link here.", false);
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x00154017 File Offset: 0x00152417
		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x00154020 File Offset: 0x00152420
		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x00154029 File Offset: 0x00152429
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x00154034 File Offset: 0x00152434
		public override void PostDeSpawn(Map map)
		{
			this.thingsToNotify.Clear();
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				this.thingsToNotify.Add(this.linkedBuildings[i]);
			}
			this.UnlinkAll();
			foreach (Thing thing in this.thingsToNotify)
			{
				thing.TryGetComp<CompAffectedByFacilities>().Notify_FacilityDespawned();
			}
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x001540E0 File Offset: 0x001524E0
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				CompAffectedByFacilities compAffectedByFacilities = this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>();
				if (compAffectedByFacilities.IsFacilityActive(this.parent))
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedBuildings[i].TrueCenter());
				}
				else
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedBuildings[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat);
				}
			}
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x0015417C File Offset: 0x0015257C
		public override string CompInspectStringExtra()
		{
			CompProperties_Facility props = this.Props;
			string result;
			if (props.statOffsets == null)
			{
				result = null;
			}
			else
			{
				bool flag = this.AmIActiveForAnyone();
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < props.statOffsets.Count; i++)
				{
					StatModifier statModifier = props.statOffsets[i];
					StatDef stat = statModifier.stat;
					stringBuilder.Append(stat.LabelCap);
					stringBuilder.Append(": ");
					stringBuilder.Append(statModifier.value.ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Offset));
					if (!flag)
					{
						stringBuilder.Append(" (");
						stringBuilder.Append("InactiveFacility".Translate());
						stringBuilder.Append(")");
					}
					if (i < props.statOffsets.Count - 1)
					{
						stringBuilder.AppendLine();
					}
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x00154278 File Offset: 0x00152678
		private void RelinkAll()
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x00154284 File Offset: 0x00152684
		private void LinkToNearbyBuildings()
		{
			this.UnlinkAll();
			CompProperties_Facility props = this.Props;
			if (props.linkableBuildings != null)
			{
				for (int i = 0; i < props.linkableBuildings.Count; i++)
				{
					foreach (Thing thing in this.parent.Map.listerThings.ThingsOfDef(props.linkableBuildings[i]))
					{
						CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
						if (compAffectedByFacilities != null && compAffectedByFacilities.CanLinkTo(this.parent))
						{
							this.linkedBuildings.Add(thing);
							compAffectedByFacilities.Notify_NewLink(this.parent);
						}
					}
				}
			}
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x00154370 File Offset: 0x00152770
		private bool AmIActiveForAnyone()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				CompAffectedByFacilities compAffectedByFacilities = this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>();
				if (compAffectedByFacilities.IsFacilityActive(this.parent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x001543D0 File Offset: 0x001527D0
		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().Notify_LinkRemoved(this.parent);
			}
			this.linkedBuildings.Clear();
		}
	}
}
