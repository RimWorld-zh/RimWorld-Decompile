using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public class VerbTracker : IExposable
	{
		public IVerbOwner directOwner;

		private List<Verb> verbs;

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

		public VerbTracker(IVerbOwner directOwner)
		{
			this.directOwner = directOwner;
		}

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

		public IEnumerable<Command> GetVerbsCommands(KeyCode hotKey = KeyCode.None)
		{
			CompEquippable ce = this.directOwner as CompEquippable;
			if (ce != null)
			{
				Thing ownerThing = ce.parent;
				List<Verb> verbs = this.AllVerbs;
				for (int i = 0; i < verbs.Count; i++)
				{
					Verb verb = verbs[i];
					if (verb.verbProps.hasStandardCommand)
					{
						yield return (Command)this.CreateVerbTargetCommand(ownerThing, verb);
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				CompEquippable equippable = this.directOwner as CompEquippable;
				if (this.directOwner.Tools.NullOrEmpty())
					yield break;
				if (equippable == null)
					yield break;
				if (!equippable.parent.def.IsMeleeWeapon)
					yield break;
				yield return (Command)this.CreateVerbTargetCommand(ownerThing, (from v in verbs
				where v.verbProps.MeleeRange
				select v).FirstOrDefault());
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		private Command_VerbTarget CreateVerbTargetCommand(Thing ownerThing, Verb verb)
		{
			Command_VerbTarget command_VerbTarget = new Command_VerbTarget();
			command_VerbTarget.defaultDesc = ownerThing.LabelCap + ": " + ownerThing.def.description;
			command_VerbTarget.icon = ownerThing.def.uiIcon;
			command_VerbTarget.iconAngle = ownerThing.def.uiIconAngle;
			command_VerbTarget.tutorTag = "VerbTarget";
			command_VerbTarget.verb = verb;
			if (verb.caster.Faction != Faction.OfPlayer)
			{
				command_VerbTarget.Disable("CannotOrderNonControlled".Translate());
			}
			if (verb.CasterIsPawn)
			{
				if (verb.CasterPawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					command_VerbTarget.Disable("IsIncapableOfViolence".Translate(verb.CasterPawn.NameStringShort));
				}
				else if (!verb.CasterPawn.drafter.Drafted)
				{
					command_VerbTarget.Disable("IsNotDrafted".Translate(verb.CasterPawn.NameStringShort));
				}
			}
			return command_VerbTarget;
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Verb>(ref this.verbs, "verbs", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verbs != null)
			{
				List<Verb> sources = this.verbs;
				this.verbs = new List<Verb>();
				this.InitVerbs(delegate(Type type, string id)
				{
					Verb verb = sources.FirstOrDefault((Verb v) => v.loadID == id && v.GetType() == type);
					if (verb == null)
					{
						Log.Warning(string.Format("Replaced verb {0}/{1}; may have been changed through a version update or a mod change", type, id));
						verb = (Verb)Activator.CreateInstance(type);
					}
					this.verbs.Add(verb);
					return verb;
				});
			}
		}

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
							string text2 = Verb.CalculateUniqueLoadID(this.directOwner, -1 - i);
							this.InitVerb(creator(verbProperties2.verbClass, text2), verbProperties2, this.directOwner, null, null, text2);
						}
					}
					catch (Exception ex)
					{
						Log.Error("Could not instantiate Verb (directOwner=" + this.directOwner.ToStringSafe() + "): " + ex);
					}
				}
			}
			List<Tool> tools = this.directOwner.Tools;
			if (tools != null)
			{
				for (int j = 0; j < tools.Count; j++)
				{
					Tool tool = tools[j];
					foreach (ManeuverDef item in from maneuver in DefDatabase<ManeuverDef>.AllDefsListForReading
					where tool.capacities.Contains(maneuver.requiredCapacity)
					select maneuver)
					{
						try
						{
							VerbProperties verb = item.verb;
							string text3 = Verb.CalculateUniqueLoadID(this.directOwner, tool, item);
							this.InitVerb(creator(verb.verbClass, text3), verb, this.directOwner, tool, item, text3);
						}
						catch (Exception ex2)
						{
							Log.Error("Could not instantiate Verb (directOwner=" + this.directOwner.ToStringSafe() + "): " + ex2);
						}
					}
				}
			}
			Pawn pawn = this.directOwner as Pawn;
			if (pawn != null && !pawn.def.tools.NullOrEmpty())
			{
				for (int k = 0; k < pawn.def.tools.Count; k++)
				{
					Tool tool2 = pawn.def.tools[k];
					foreach (ManeuverDef item2 in from maneuver in DefDatabase<ManeuverDef>.AllDefsListForReading
					where tool2.capacities.Contains(maneuver.requiredCapacity)
					select maneuver)
					{
						try
						{
							VerbProperties verb2 = item2.verb;
							string text4 = Verb.CalculateUniqueLoadID(this.directOwner, tool2, item2);
							this.InitVerb(creator(verb2.verbClass, text4), verb2, this.directOwner, tool2, item2, text4);
						}
						catch (Exception ex3)
						{
							Log.Error("Could not instantiate Verb (directOwner=" + this.directOwner.ToStringSafe() + "): " + ex3);
						}
					}
				}
			}
		}

		private void InitVerb(Verb verb, VerbProperties properties, IVerbOwner owner, Tool tool, ManeuverDef maneuver, string id)
		{
			verb.loadID = id;
			verb.verbProps = properties;
			verb.tool = tool;
			verb.maneuver = maneuver;
			CompEquippable compEquippable = this.directOwner as CompEquippable;
			Pawn pawn = this.directOwner as Pawn;
			HediffComp_VerbGiver hediffComp_VerbGiver = this.directOwner as HediffComp_VerbGiver;
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
				else
				{
					verb.implementOwnerType = ImplementOwnerTypeDefOf.Bodypart;
				}
			}
		}
	}
}
