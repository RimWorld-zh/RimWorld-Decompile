using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FD2 RID: 4050
	public class VerbTracker : IExposable
	{
		// Token: 0x060061EB RID: 25067 RVA: 0x003149E1 File Offset: 0x00312DE1
		public VerbTracker(IVerbOwner directOwner)
		{
			this.directOwner = directOwner;
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x060061EC RID: 25068 RVA: 0x00314A00 File Offset: 0x00312E00
		public List<Verb> AllVerbs
		{
			get
			{
				if (this.verbs == null)
				{
					this.InitVerbsFromZero();
				}
				return this.verbs;
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x060061ED RID: 25069 RVA: 0x00314A2C File Offset: 0x00312E2C
		public Verb PrimaryVerb
		{
			get
			{
				if (this.verbs == null)
				{
					this.InitVerbsFromZero();
				}
				for (int i = 0; i < this.verbs.Count; i++)
				{
					if (this.verbs[i].verbProps.isPrimary)
					{
						return this.verbs[i];
					}
				}
				return null;
			}
		}

		// Token: 0x060061EE RID: 25070 RVA: 0x00314AA0 File Offset: 0x00312EA0
		public void VerbsTick()
		{
			if (this.verbs != null)
			{
				for (int i = 0; i < this.verbs.Count; i++)
				{
					this.verbs[i].VerbTick();
				}
			}
		}

		// Token: 0x060061EF RID: 25071 RVA: 0x00314AF0 File Offset: 0x00312EF0
		public IEnumerable<Command> GetVerbsCommands(KeyCode hotKey = KeyCode.None)
		{
			CompEquippable ce = this.directOwner as CompEquippable;
			if (ce == null)
			{
				yield break;
			}
			Thing ownerThing = ce.parent;
			List<Verb> verbs = this.AllVerbs;
			for (int i = 0; i < verbs.Count; i++)
			{
				Verb verb = verbs[i];
				if (verb.verbProps.hasStandardCommand)
				{
					yield return this.CreateVerbTargetCommand(ownerThing, verb);
				}
			}
			CompEquippable equippable = this.directOwner as CompEquippable;
			if (!this.directOwner.Tools.NullOrEmpty<Tool>() && equippable != null && equippable.parent.def.IsMeleeWeapon)
			{
				yield return this.CreateVerbTargetCommand(ownerThing, (from v in verbs
				where v.verbProps.IsMeleeAttack
				select v).FirstOrDefault<Verb>());
			}
			yield break;
		}

		// Token: 0x060061F0 RID: 25072 RVA: 0x00314B1C File Offset: 0x00312F1C
		private Command_VerbTarget CreateVerbTargetCommand(Thing ownerThing, Verb verb)
		{
			Command_VerbTarget command_VerbTarget = new Command_VerbTarget();
			command_VerbTarget.defaultDesc = ownerThing.LabelCap + ": " + ownerThing.def.description.CapitalizeFirst();
			command_VerbTarget.icon = ownerThing.def.uiIcon;
			command_VerbTarget.iconAngle = ownerThing.def.uiIconAngle;
			command_VerbTarget.iconOffset = ownerThing.def.uiIconOffset;
			command_VerbTarget.tutorTag = "VerbTarget";
			command_VerbTarget.verb = verb;
			if (verb.caster.Faction != Faction.OfPlayer)
			{
				command_VerbTarget.Disable("CannotOrderNonControlled".Translate());
			}
			else if (verb.CasterIsPawn)
			{
				if (verb.CasterPawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					command_VerbTarget.Disable("IsIncapableOfViolence".Translate(new object[]
					{
						verb.CasterPawn.LabelShort
					}));
				}
				else if (!verb.CasterPawn.drafter.Drafted)
				{
					command_VerbTarget.Disable("IsNotDrafted".Translate(new object[]
					{
						verb.CasterPawn.LabelShort
					}));
				}
			}
			return command_VerbTarget;
		}

		// Token: 0x060061F1 RID: 25073 RVA: 0x00314C54 File Offset: 0x00313054
		public void ExposeData()
		{
			Scribe_Collections.Look<Verb>(ref this.verbs, "verbs", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.verbs != null)
				{
					if (this.verbs.RemoveAll((Verb x) => x == null) != 0)
					{
						Log.Error("Some verbs were null after loading. directOwner=" + this.directOwner.ToStringSafe<IVerbOwner>(), false);
					}
					List<Verb> sources = this.verbs;
					this.verbs = new List<Verb>();
					this.InitVerbs(delegate(Type type, string id)
					{
						Verb verb = sources.FirstOrDefault((Verb v) => v.loadID == id && v.GetType() == type);
						if (verb == null)
						{
							Log.Warning(string.Format("Replaced verb {0}/{1}; may have been changed through a version update or a mod change", type, id), false);
							verb = (Verb)Activator.CreateInstance(type);
						}
						this.verbs.Add(verb);
						return verb;
					});
				}
			}
		}

		// Token: 0x060061F2 RID: 25074 RVA: 0x00314D11 File Offset: 0x00313111
		private void InitVerbsFromZero()
		{
			this.verbs = new List<Verb>();
			this.InitVerbs(delegate(Type type, string id)
			{
				Verb verb = (Verb)Activator.CreateInstance(type);
				this.verbs.Add(verb);
				return verb;
			});
		}

		// Token: 0x060061F3 RID: 25075 RVA: 0x00314D34 File Offset: 0x00313134
		private void InitVerbs(Func<Type, string, Verb> creator)
		{
			List<VerbProperties> verbProperties = this.directOwner.VerbProperties;
			if (verbProperties != null)
			{
				for (int i = 0; i < verbProperties.Count; i++)
				{
					try
					{
						VerbProperties verbProperties2 = verbProperties[i];
						string text = Verb.CalculateUniqueLoadID(this.directOwner, i);
						this.InitVerb(creator(verbProperties2.verbClass, text), verbProperties2, this.directOwner, null, null, text);
						if (verbProperties2.LaunchesProjectile && !verbProperties2.onlyManualCast)
						{
							VerbProperties verbProperties3 = verbProperties2.MemberwiseClone();
							verbProperties3.defaultCooldownTime += verbProperties3.warmupTime;
							verbProperties3.warmupTime = 0f;
							verbProperties3.meleeShoot = true;
							verbProperties3.hasStandardCommand = false;
							string text2 = Verb.CalculateUniqueLoadID(this.directOwner, -1 - i);
							this.InitVerb(creator(verbProperties3.verbClass, text2), verbProperties3, this.directOwner, null, null, text2);
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate Verb (directOwner=",
							this.directOwner.ToStringSafe<IVerbOwner>(),
							"): ",
							ex
						}), false);
					}
				}
			}
			List<Tool> tools = this.directOwner.Tools;
			if (tools != null)
			{
				for (int j = 0; j < tools.Count; j++)
				{
					Tool tool = tools[j];
					foreach (ManeuverDef maneuverDef in from maneuver in DefDatabase<ManeuverDef>.AllDefsListForReading
					where tool.capacities.Contains(maneuver.requiredCapacity)
					select maneuver)
					{
						try
						{
							VerbProperties verb = maneuverDef.verb;
							string text3 = Verb.CalculateUniqueLoadID(this.directOwner, tool, maneuverDef);
							this.InitVerb(creator(verb.verbClass, text3), verb, this.directOwner, tool, maneuverDef, text3);
						}
						catch (Exception ex2)
						{
							Log.Error(string.Concat(new object[]
							{
								"Could not instantiate Verb (directOwner=",
								this.directOwner.ToStringSafe<IVerbOwner>(),
								"): ",
								ex2
							}), false);
						}
					}
				}
			}
			Pawn pawn = this.directOwner as Pawn;
			if (pawn != null && !pawn.def.tools.NullOrEmpty<Tool>())
			{
				for (int k = 0; k < pawn.def.tools.Count; k++)
				{
					Tool tool = pawn.def.tools[k];
					foreach (ManeuverDef maneuverDef2 in from maneuver in DefDatabase<ManeuverDef>.AllDefsListForReading
					where tool.capacities.Contains(maneuver.requiredCapacity)
					select maneuver)
					{
						try
						{
							VerbProperties verb2 = maneuverDef2.verb;
							string text4 = Verb.CalculateUniqueLoadID(this.directOwner, tool, maneuverDef2);
							this.InitVerb(creator(verb2.verbClass, text4), verb2, this.directOwner, tool, maneuverDef2, text4);
						}
						catch (Exception ex3)
						{
							Log.Error(string.Concat(new object[]
							{
								"Could not instantiate Verb (directOwner=",
								this.directOwner.ToStringSafe<IVerbOwner>(),
								"): ",
								ex3
							}), false);
						}
					}
				}
			}
		}

		// Token: 0x060061F4 RID: 25076 RVA: 0x0031511C File Offset: 0x0031351C
		private void InitVerb(Verb verb, VerbProperties properties, IVerbOwner owner, Tool tool, ManeuverDef maneuver, string id)
		{
			verb.loadID = id;
			verb.verbProps = properties;
			verb.tool = tool;
			verb.maneuver = maneuver;
			CompEquippable compEquippable = this.directOwner as CompEquippable;
			Pawn pawn = this.directOwner as Pawn;
			HediffComp_VerbGiver hediffComp_VerbGiver = this.directOwner as HediffComp_VerbGiver;
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = this.directOwner as Pawn_MeleeVerbs_TerrainSource;
			if (compEquippable != null)
			{
				verb.ownerEquipment = compEquippable.parent;
			}
			else if (pawn != null)
			{
				verb.caster = pawn;
			}
			else if (hediffComp_VerbGiver != null)
			{
				verb.ownerHediffComp = hediffComp_VerbGiver;
				verb.caster = hediffComp_VerbGiver.Pawn;
			}
			else if (pawn_MeleeVerbs_TerrainSource != null)
			{
				verb.terrainDef = pawn_MeleeVerbs_TerrainSource.def;
				verb.caster = pawn_MeleeVerbs_TerrainSource.parent.Pawn;
			}
			else
			{
				Log.ErrorOnce("Couldn't find verb source", 27797477, false);
			}
			if (verb.tool != null)
			{
				if (verb.ownerEquipment != null)
				{
					verb.implementOwnerType = ImplementOwnerTypeDefOf.Weapon;
				}
				else if (verb.ownerHediffComp != null)
				{
					verb.implementOwnerType = ImplementOwnerTypeDefOf.Hediff;
				}
				else if (verb.terrainDef != null)
				{
					verb.implementOwnerType = ImplementOwnerTypeDefOf.Terrain;
				}
				else
				{
					verb.implementOwnerType = ImplementOwnerTypeDefOf.Bodypart;
				}
			}
		}

		// Token: 0x04003FFF RID: 16383
		public IVerbOwner directOwner = null;

		// Token: 0x04004000 RID: 16384
		private List<Verb> verbs = null;
	}
}
