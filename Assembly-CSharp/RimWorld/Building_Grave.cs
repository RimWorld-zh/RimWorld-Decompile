using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Building_Grave : Building_Casket, IStoreSettingsParent, IAssignableBuilding
	{
		private StorageSettings storageSettings = null;

		private Graphic cachedGraphicFull = null;

		public Pawn assignedPawn = null;

		public override Graphic Graphic
		{
			get
			{
				Graphic graphic;
				if (this.HasCorpse)
				{
					if (base.def.building.fullGraveGraphicData == null)
					{
						graphic = base.Graphic;
					}
					else
					{
						if (this.cachedGraphicFull == null)
						{
							this.cachedGraphicFull = base.def.building.fullGraveGraphicData.GraphicColoredFor(this);
						}
						graphic = this.cachedGraphicFull;
					}
				}
				else
				{
					graphic = base.Graphic;
				}
				return graphic;
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
				int num = 0;
				Corpse result;
				while (true)
				{
					if (num < base.innerContainer.Count)
					{
						Corpse corpse = base.innerContainer[num] as Corpse;
						if (corpse != null)
						{
							result = corpse;
							break;
						}
						num++;
						continue;
					}
					result = null;
					break;
				}
				return result;
			}
		}

		public IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				IEnumerable<Pawn> result;
				if (!base.Spawned)
				{
					result = Enumerable.Empty<Pawn>();
				}
				else
				{
					IEnumerable<Pawn> second = from Corpse x in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse)
					where x.InnerPawn.IsColonist
					select x.InnerPawn;
					result = base.Map.mapPawns.FreeColonistsSpawned.Concat(second);
				}
				return result;
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
			bool result;
			if (!base.Accepts(thing))
			{
				result = false;
			}
			else if (this.HasCorpse)
			{
				result = false;
			}
			else
			{
				if (this.assignedPawn != null)
				{
					Corpse corpse = thing as Corpse;
					if (corpse == null)
					{
						result = false;
						goto IL_0085;
					}
					if (corpse.InnerPawn != this.assignedPawn)
					{
						result = false;
						goto IL_0085;
					}
				}
				else if (!this.storageSettings.AllowedToAccept(thing))
				{
					result = false;
					goto IL_0085;
				}
				result = true;
			}
			goto IL_0085;
			IL_0085:
			return result;
		}

		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			bool result;
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
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy0().GetEnumerator())
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
				action = (Action)delegate
				{
					Find.WindowStack.Add(new Dialog_AssignBuildingOwner(((_003CGetGizmos_003Ec__Iterator1)/*Error near IL_01c5: stateMachine*/)._0024this));
				},
				hotKey = KeyBindingDefOf.Misc3
			};
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0210:
			/*Error near IL_0211: Unexpected return in MoveNext()*/;
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
