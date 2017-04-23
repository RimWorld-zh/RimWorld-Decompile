using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Building_Grave : Building_Casket, IAssignableBuilding, IStoreSettingsParent
	{
		private StorageSettings storageSettings;

		private Graphic cachedGraphicFull;

		public Pawn assignedPawn;

		public override Graphic Graphic
		{
			get
			{
				if (!this.HasCorpse)
				{
					return base.Graphic;
				}
				if (this.def.building.fullGraveGraphicData == null)
				{
					return base.Graphic;
				}
				if (this.cachedGraphicFull == null)
				{
					this.cachedGraphicFull = this.def.building.fullGraveGraphicData.GraphicColoredFor(this);
				}
				return this.cachedGraphicFull;
			}
		}

		public bool HasCorpse
		{
			get
			{
				return this.Corpse != null;
			}
		}

		public Corpse Corpse
		{
			get
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Corpse corpse = this.innerContainer[i] as Corpse;
					if (corpse != null)
					{
						return corpse;
					}
				}
				return null;
			}
		}

		public IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!base.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				IEnumerable<Pawn> second = from Corpse x in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse)
				where x.InnerPawn.IsColonist
				select x.InnerPawn;
				return base.Map.mapPawns.FreeColonistsSpawned.Concat(second);
			}
		}

		public IEnumerable<Pawn> AssignedPawns
		{
			get
			{
				Building_Grave.<>c__Iterator156 <>c__Iterator = new Building_Grave.<>c__Iterator156();
				<>c__Iterator.<>f__this = this;
				Building_Grave.<>c__Iterator156 expr_0E = <>c__Iterator;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		public int MaxAssignedPawnsCount
		{
			get
			{
				return 1;
			}
		}

		public bool StorageTabVisible
		{
			get
			{
				return this.assignedPawn == null && !this.HasCorpse;
			}
		}

		public void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimGrave(this);
		}

		public void TryUnassignPawn(Pawn pawn)
		{
			if (pawn == this.assignedPawn)
			{
				pawn.ownership.UnclaimGrave();
			}
		}

		public StorageSettings GetStoreSettings()
		{
			return this.storageSettings;
		}

		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		public override void PostMake()
		{
			base.PostMake();
			this.storageSettings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.storageSettings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		public override void TickRare()
		{
			base.TickRare();
			this.innerContainer.ThingOwnerTickRare(true);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.storageSettings, "storageSettings", new object[]
			{
				this
			});
		}

		public override void EjectContents()
		{
			base.EjectContents();
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		public virtual void Notify_CorpseBuried(Pawn worker)
		{
			CompArt comp = base.GetComp<CompArt>();
			if (comp != null && !comp.Active)
			{
				comp.JustCreatedBy(worker);
				comp.InitializeArt(this.Corpse.InnerPawn);
			}
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
		}

		public override bool Accepts(Thing thing)
		{
			if (!base.Accepts(thing))
			{
				return false;
			}
			if (this.HasCorpse)
			{
				return false;
			}
			if (this.assignedPawn != null)
			{
				Corpse corpse = thing as Corpse;
				if (corpse == null)
				{
					return false;
				}
				if (corpse.InnerPawn != this.assignedPawn)
				{
					return false;
				}
			}
			else if (!this.storageSettings.AllowedToAccept(thing))
			{
				return false;
			}
			return true;
		}

		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null && corpse.InnerPawn.ownership != null && corpse.InnerPawn.ownership.AssignedGrave != this)
				{
					corpse.InnerPawn.ownership.UnclaimGrave();
				}
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				}
				return true;
			}
			return false;
		}

		[DebuggerHidden]
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_Grave.<GetGizmos>c__Iterator157 <GetGizmos>c__Iterator = new Building_Grave.<GetGizmos>c__Iterator157();
			<GetGizmos>c__Iterator.<>f__this = this;
			Building_Grave.<GetGizmos>c__Iterator157 expr_0E = <GetGizmos>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (!this.HasCorpse && this.assignedPawn != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AssignedColonist".Translate());
				stringBuilder.Append(": ");
				stringBuilder.Append(this.assignedPawn.LabelCap);
			}
			return stringBuilder.ToString();
		}
	}
}
