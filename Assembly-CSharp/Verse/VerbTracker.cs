using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class VerbTracker : IExposable
	{
		public IVerbOwner directOwner = null;

		private List<Verb> verbs = null;

		[CompilerGenerated]
		private static Predicate<Verb> <>f__am$cache0;

		public VerbTracker(IVerbOwner directOwner)
		{
			this.directOwner = directOwner;
		}

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
			if (!this.directOwner.Tools.NullOrEmpty<Tool>() && ce != null && ce.parent.def.IsMeleeWeapon)
			{
				yield return this.CreateVerbTargetCommand(ownerThing, (from v in verbs
				where v.verbProps.IsMeleeAttack
				select v).FirstOrDefault<Verb>());
			}
			yield break;
		}

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

		public Verb GetVerb(VerbCategory category)
		{
			List<Verb> allVerbs = this.AllVerbs;
			if (allVerbs != null)
			{
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (allVerbs[i].verbProps.category == category)
					{
						return allVerbs[i];
					}
				}
			}
			return null;
		}

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
						this.InitVerb(creator(verbProperties2.verbClass, text), verbProperties2, null, null, text);
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
					foreach (ManeuverDef maneuverDef in tool.Maneuvers)
					{
						try
						{
							VerbProperties verb = maneuverDef.verb;
							string text2 = Verb.CalculateUniqueLoadID(this.directOwner, tool, maneuverDef);
							this.InitVerb(creator(verb.verbClass, text2), verb, tool, maneuverDef, text2);
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
		}

		private void InitVerb(Verb verb, VerbProperties properties, Tool tool, ManeuverDef maneuver, string id)
		{
			verb.loadID = id;
			verb.verbProps = properties;
			verb.verbTracker = this;
			verb.tool = tool;
			verb.maneuver = maneuver;
			verb.caster = this.directOwner.ConstantCaster;
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__0(Verb x)
		{
			return x == null;
		}

		[CompilerGenerated]
		private Verb <InitVerbsFromZero>m__1(Type type, string id)
		{
			Verb verb = (Verb)Activator.CreateInstance(type);
			this.verbs.Add(verb);
			return verb;
		}

		[CompilerGenerated]
		private sealed class <GetVerbsCommands>c__Iterator0 : IEnumerable, IEnumerable<Command>, IEnumerator, IDisposable, IEnumerator<Command>
		{
			internal CompEquippable <ce>__0;

			internal Thing <ownerThing>__0;

			internal List<Verb> <verbs>__0;

			internal int <i>__1;

			internal Verb <verb>__2;

			internal VerbTracker $this;

			internal Command $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<Verb, bool> <>f__am$cache0;

			[DebuggerHidden]
			public <GetVerbsCommands>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					ce = (this.directOwner as CompEquippable);
					if (ce == null)
					{
						return false;
					}
					ownerThing = ce.parent;
					verbs = base.AllVerbs;
					i = 0;
					break;
				case 1u:
					IL_D8:
					i++;
					break;
				case 2u:
					IL_194:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				if (i >= verbs.Count)
				{
					if (this.directOwner.Tools.NullOrEmpty<Tool>() || ce == null || !ce.parent.def.IsMeleeWeapon)
					{
						goto IL_194;
					}
					this.$current = base.CreateVerbTargetCommand(ownerThing, (from v in verbs
					where v.verbProps.IsMeleeAttack
					select v).FirstOrDefault<Verb>());
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
				}
				else
				{
					verb = verbs[i];
					if (!verb.verbProps.hasStandardCommand)
					{
						goto IL_D8;
					}
					this.$current = base.CreateVerbTargetCommand(ownerThing, verb);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
				}
				return true;
			}

			Command IEnumerator<Command>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Command>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Command> IEnumerable<Command>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				VerbTracker.<GetVerbsCommands>c__Iterator0 <GetVerbsCommands>c__Iterator = new VerbTracker.<GetVerbsCommands>c__Iterator0();
				<GetVerbsCommands>c__Iterator.$this = this;
				return <GetVerbsCommands>c__Iterator;
			}

			private static bool <>m__0(Verb v)
			{
				return v.verbProps.IsMeleeAttack;
			}
		}

		[CompilerGenerated]
		private sealed class <ExposeData>c__AnonStorey1
		{
			internal List<Verb> sources;

			internal VerbTracker $this;

			public <ExposeData>c__AnonStorey1()
			{
			}

			internal Verb <>m__0(Type type, string id)
			{
				Verb verb = this.sources.FirstOrDefault((Verb v) => v.loadID == id && v.GetType() == type);
				if (verb == null)
				{
					Log.Warning(string.Format("Replaced verb {0}/{1}; may have been changed through a version update or a mod change", type, id), false);
					verb = (Verb)Activator.CreateInstance(type);
				}
				this.$this.verbs.Add(verb);
				return verb;
			}

			private sealed class <ExposeData>c__AnonStorey2
			{
				internal string id;

				internal Type type;

				internal VerbTracker.<ExposeData>c__AnonStorey1 <>f__ref$1;

				public <ExposeData>c__AnonStorey2()
				{
				}

				internal bool <>m__0(Verb v)
				{
					return v.loadID == this.id && v.GetType() == this.type;
				}
			}
		}
	}
}
