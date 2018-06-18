using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000711 RID: 1809
	public class CompFacility : ThingComp
	{
		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060027A7 RID: 10151 RVA: 0x00153B30 File Offset: 0x00151F30
		public bool CanBeActive
		{
			get
			{
				CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
				return compPowerTrader == null || compPowerTrader.PowerOn;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x060027A8 RID: 10152 RVA: 0x00153B6C File Offset: 0x00151F6C
		public CompProperties_Facility Props
		{
			get
			{
				return (CompProperties_Facility)this.props;
			}
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x00153B8C File Offset: 0x00151F8C
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

		// Token: 0x060027AA RID: 10154 RVA: 0x00153C68 File Offset: 0x00152068
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

		// Token: 0x060027AB RID: 10155 RVA: 0x00153CC4 File Offset: 0x001520C4
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

		// Token: 0x060027AC RID: 10156 RVA: 0x00153D1F File Offset: 0x0015211F
		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x00153D28 File Offset: 0x00152128
		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x00153D31 File Offset: 0x00152131
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x00153D3C File Offset: 0x0015213C
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

		// Token: 0x060027B0 RID: 10160 RVA: 0x00153DE8 File Offset: 0x001521E8
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

		// Token: 0x060027B1 RID: 10161 RVA: 0x00153E84 File Offset: 0x00152284
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

		// Token: 0x060027B2 RID: 10162 RVA: 0x00153F80 File Offset: 0x00152380
		private void RelinkAll()
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x00153F8C File Offset: 0x0015238C
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

		// Token: 0x060027B4 RID: 10164 RVA: 0x00154078 File Offset: 0x00152478
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

		// Token: 0x060027B5 RID: 10165 RVA: 0x001540D8 File Offset: 0x001524D8
		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().Notify_LinkRemoved(this.parent);
			}
			this.linkedBuildings.Clear();
		}

		// Token: 0x040015D9 RID: 5593
		private List<Thing> linkedBuildings = new List<Thing>();

		// Token: 0x040015DA RID: 5594
		private HashSet<Thing> thingsToNotify = new HashSet<Thing>();
	}
}
