using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Building_Grave : Building_Casket, IStoreSettingsParent, IAssignableBuilding
	{
		private StorageSettings storageSettings;

		private Graphic cachedGraphicFull;

		public Pawn assignedPawn;

		public override Graphic Graphic
		{
			get
			{
				if (this.HasCorpse)
				{
					if (base.def.building.fullGraveGraphicData == null)
					{
						return base.Graphic;
					}
					if (this.cachedGraphicFull == null)
					{
						this.cachedGraphicFull = base.def.building.fullGraveGraphicData.GraphicColoredFor(this);
					}
					return this.cachedGraphicFull;
				}
				return base.Graphic;
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
				for (int i = 0; i < base.innerContainer.Count; i++)
				{
					Corpse corpse = base.innerContainer[i] as Corpse;
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
				if (this.assignedPawn == null)
					yield break;
				yield return this.assignedPawn;
				/*Error: Unable to find new state assignment for yield return*/;
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
			return base.def.building.fixedStorageSettings;
		}

		public override void PostMake()
		{
			base.PostMake();
			this.storageSettings = new StorageSettings(this);
			if (base.def.building.defaultStorageSettings != null)
			{
				this.storageSettings.CopyFrom(base.def.building.defaultStorageSettings);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.storageSettings, "storageSettings", new object[1]
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

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = base.GetGizmos().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo g2 = enumerator.Current;
					yield return g2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.StorageTabVisible)
			{
				using (IEnumerator<Gizmo> enumerator2 = StorageSettingsClipboard.CopyPasteGizmosFor(this.storageSettings).GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						Gizmo g = enumerator2.Current;
						yield return g;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.HasCorpse)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "CommandGraveAssignColonistLabel".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Commands/AssignOwner", true),
				defaultDesc = "CommandGraveAssignColonistDesc".Translate(),
				action = delegate
				{
					Find.WindowStack.Add(new Dialog_AssignBuildingOwner(((_003CGetGizmos_003Ec__Iterator1)/*Error near IL_01bb: stateMachine*/)._0024this));
				},
				hotKey = KeyBindingDefOf.Misc3
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0205:
			/*Error near IL_0206: Unexpected return in MoveNext()*/;
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
