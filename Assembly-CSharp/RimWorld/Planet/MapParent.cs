using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02000603 RID: 1539
	[StaticConstructorOnStartup]
	public class MapParent : WorldObject, IThingHolder
	{
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001E98 RID: 7832 RVA: 0x00107B48 File Offset: 0x00105F48
		public bool HasMap
		{
			get
			{
				return this.Map != null;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001E99 RID: 7833 RVA: 0x00107B6C File Offset: 0x00105F6C
		protected virtual bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001E9A RID: 7834 RVA: 0x00107B84 File Offset: 0x00105F84
		public Map Map
		{
			get
			{
				return Current.Game.FindMap(this);
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001E9B RID: 7835 RVA: 0x00107BA4 File Offset: 0x00105FA4
		public virtual MapGeneratorDef MapGeneratorDef
		{
			get
			{
				return (this.def.mapGenerator == null) ? MapGeneratorDefOf.Encounter : this.def.mapGenerator;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001E9C RID: 7836 RVA: 0x00107BE0 File Offset: 0x00105FE0
		public virtual IEnumerable<GenStepDef> ExtraGenStepDefs
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001E9D RID: 7837 RVA: 0x00107C04 File Offset: 0x00106004
		public override bool ExpandMore
		{
			get
			{
				return base.ExpandMore || this.HasMap;
			}
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x00107C30 File Offset: 0x00106030
		public virtual void PostMapGenerate()
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMapGenerate();
			}
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x00107C6C File Offset: 0x0010606C
		public virtual void Notify_MyMapRemoved(Map map)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMyMapRemoved();
			}
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x00107CA8 File Offset: 0x001060A8
		public virtual void Notify_CaravanFormed(Caravan caravan)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostCaravanFormed(caravan);
			}
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x00107CE3 File Offset: 0x001060E3
		public virtual void Notify_HibernatableChanged()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x00107CEC File Offset: 0x001060EC
		public virtual void FinalizeLoading()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x00107CF8 File Offset: 0x001060F8
		public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x00107D11 File Offset: 0x00106111
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.HasMap)
			{
				Current.Game.DeinitAndRemoveMap(this.Map);
			}
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x00107D35 File Offset: 0x00106135
		public override void Tick()
		{
			base.Tick();
			this.CheckRemoveMapNow();
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x00107D44 File Offset: 0x00106144
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo g in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return g;
			}
			if (this.HasMap)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandShowMap".Translate(),
					defaultDesc = "CommandShowMapDesc".Translate(),
					icon = MapParent.ShowMapCommand,
					hotKey = KeyBindingDefOf.Misc1,
					action = delegate()
					{
						Current.Game.CurrentMap = this.Map;
						if (!CameraJumper.TryHideWorld())
						{
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
					}
				};
			}
			yield break;
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x00107D70 File Offset: 0x00106170
		public override IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			foreach (IncidentTargetTypeDef type in this.<AcceptedTypes>__BaseCallProxy1())
			{
				yield return type;
			}
			if (this.hibernatableIncidentTargets != null && this.hibernatableIncidentTargets.Count > 0)
			{
				foreach (IncidentTargetTypeDef type2 in this.hibernatableIncidentTargets)
				{
					yield return type2;
				}
			}
			yield break;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x00107D9C File Offset: 0x0010619C
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption o in this.<GetFloatMenuOptions>__BaseCallProxy2(caravan))
			{
				yield return o;
			}
			if (this.UseGenericEnterMapFloatMenuOption)
			{
				foreach (FloatMenuOption f in CaravanArrivalAction_Enter.GetFloatMenuOptions(caravan, this))
				{
					yield return f;
				}
			}
			yield break;
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x00107DD0 File Offset: 0x001061D0
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption o in this.<GetTransportPodsFloatMenuOptions>__BaseCallProxy3(pods, representative))
			{
				yield return o;
			}
			if (TransportPodsArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this))
			{
				yield return new FloatMenuOption("LandInExistingMap".Translate(new object[]
				{
					this.Label
				}), delegate()
				{
					Map myMap = representative.parent.Map;
					Map map = this.Map;
					Current.Game.CurrentMap = map;
					CameraJumper.TryHideWorld();
					Find.Targeter.BeginTargeting(TargetingParameters.ForDropPodsDestination(), delegate(LocalTargetInfo x)
					{
						representative.TryLaunch(this.Tile, new TransportPodsArrivalAction_LandInSpecificCell(this.$this, x.Cell));
					}, null, delegate()
					{
						if (Find.Maps.Contains(myMap))
						{
							Current.Game.CurrentMap = myMap;
						}
					}, CompLaunchable.TargeterMouseAttachment);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x00107E08 File Offset: 0x00106208
		public void CheckRemoveMapNow()
		{
			bool flag;
			if (this.HasMap && this.ShouldRemoveMapNow(out flag))
			{
				Map map = this.Map;
				Current.Game.DeinitAndRemoveMap(map);
				if (flag)
				{
					Find.WorldObjects.Remove(this);
				}
			}
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x00107E54 File Offset: 0x00106254
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.EnterCooldownBlocksEntering())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "EnterCooldown".Translate(new object[]
				{
					this.EnterCooldownDaysLeft().ToString("0.#")
				});
			}
			return text;
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x00107EC4 File Offset: 0x001062C4
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x00107EDA File Offset: 0x001062DA
		public virtual void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.HasMap)
			{
				outChildren.Add(this.Map);
			}
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x00107F00 File Offset: 0x00106300
		private void RecalculateHibernatableIncidentTargets()
		{
			this.hibernatableIncidentTargets = null;
			foreach (ThingWithComps thing in this.Map.listerThings.AllThings.OfType<ThingWithComps>())
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Starting && compHibernatable.Props.incidentTargetWhileStarting != null)
				{
					if (this.hibernatableIncidentTargets == null)
					{
						this.hibernatableIncidentTargets = new HashSet<IncidentTargetTypeDef>();
					}
					this.hibernatableIncidentTargets.Add(compHibernatable.Props.incidentTargetWhileStarting);
				}
			}
		}

		// Token: 0x0400121B RID: 4635
		private HashSet<IncidentTargetTypeDef> hibernatableIncidentTargets;

		// Token: 0x0400121C RID: 4636
		private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);
	}
}
