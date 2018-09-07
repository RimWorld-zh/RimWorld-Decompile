using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public static class VerbUtility
	{
		public static ThingDef GetProjectile(this Verb verb)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = verb as Verb_LaunchProjectile;
			return (verb_LaunchProjectile == null) ? null : verb_LaunchProjectile.Projectile;
		}

		public static DamageDef GetDamageDef(this Verb verb)
		{
			if (!verb.verbProps.LaunchesProjectile)
			{
				return verb.verbProps.meleeDamageDef;
			}
			ThingDef projectile = verb.GetProjectile();
			if (projectile != null)
			{
				return projectile.projectile.damageDef;
			}
			return null;
		}

		public static bool IsIncendiary(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.ai_IsIncendiary;
		}

		public static bool ProjectileFliesOverhead(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.flyOverhead;
		}

		public static bool HarmsHealth(this Verb verb)
		{
			DamageDef damageDef = verb.GetDamageDef();
			return damageDef != null && damageDef.harmsHealth;
		}

		public static bool IsEMP(this Verb verb)
		{
			return verb.GetDamageDef() == DamageDefOf.EMP;
		}

		public static bool UsesExplosiveProjectiles(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.explosionRadius > 0f;
		}

		public static List<Verb> GetConcreteExampleVerbs(Def def, ThingDef stuff = null)
		{
			List<Verb> result = null;
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				Thing concreteExample = thingDef.GetConcreteExample(stuff);
				if (concreteExample is Pawn)
				{
					result = ((Pawn)concreteExample).VerbTracker.AllVerbs;
				}
				else if (concreteExample is ThingWithComps)
				{
					result = ((ThingWithComps)concreteExample).GetComp<CompEquippable>().AllVerbs;
				}
				else
				{
					result = null;
				}
			}
			HediffDef hediffDef = def as HediffDef;
			if (hediffDef != null)
			{
				Hediff concreteExample2 = hediffDef.ConcreteExample;
				result = concreteExample2.TryGetComp<HediffComp_VerbGiver>().VerbTracker.AllVerbs;
			}
			return result;
		}

		public static float CalculateAdjustedForcedMiss(float forcedMiss, IntVec3 vector)
		{
			float num = (float)vector.LengthHorizontalSquared;
			if (num < 9f)
			{
				return 0f;
			}
			if (num < 25f)
			{
				return forcedMiss * 0.5f;
			}
			if (num < 49f)
			{
				return forcedMiss * 0.8f;
			}
			return forcedMiss;
		}

		public static float InterceptChanceFactorFromDistance(Vector3 origin, IntVec3 c)
		{
			float num = (c.ToVector3Shifted() - origin).MagnitudeHorizontalSquared();
			if (num <= 25f)
			{
				return 0f;
			}
			if (num >= 144f)
			{
				return 1f;
			}
			return Mathf.InverseLerp(25f, 144f, num);
		}

		public static IEnumerable<VerbUtility.VerbPropertiesWithSource> GetAllVerbProperties(List<VerbProperties> verbProps, List<Tool> tools)
		{
			if (verbProps != null)
			{
				for (int i = 0; i < verbProps.Count; i++)
				{
					yield return new VerbUtility.VerbPropertiesWithSource(verbProps[i]);
				}
			}
			if (tools != null)
			{
				for (int j = 0; j < tools.Count; j++)
				{
					foreach (ManeuverDef k in tools[j].Maneuvers)
					{
						yield return new VerbUtility.VerbPropertiesWithSource(k.verb, tools[j], k);
					}
				}
			}
			yield break;
		}

		public static bool AllowAdjacentShot(LocalTargetInfo target, Thing caster)
		{
			Pawn pawn = target.Thing as Pawn;
			return pawn == null || !pawn.HostileTo(caster) || pawn.Downed;
		}

		public struct VerbPropertiesWithSource
		{
			public VerbProperties verbProps;

			public Tool tool;

			public ManeuverDef maneuver;

			public VerbPropertiesWithSource(VerbProperties verbProps)
			{
				this.verbProps = verbProps;
				this.tool = null;
				this.maneuver = null;
			}

			public VerbPropertiesWithSource(VerbProperties verbProps, Tool tool, ManeuverDef maneuver)
			{
				this.verbProps = verbProps;
				this.tool = tool;
				this.maneuver = maneuver;
			}

			public ToolCapacityDef ToolCapacity
			{
				get
				{
					return (this.maneuver == null) ? null : this.maneuver.requiredCapacity;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GetAllVerbProperties>c__Iterator0 : IEnumerable, IEnumerable<VerbUtility.VerbPropertiesWithSource>, IEnumerator, IDisposable, IEnumerator<VerbUtility.VerbPropertiesWithSource>
		{
			internal List<VerbProperties> verbProps;

			internal int <i>__1;

			internal List<Tool> tools;

			internal int <i>__2;

			internal IEnumerator<ManeuverDef> $locvar0;

			internal ManeuverDef <m>__3;

			internal VerbUtility.VerbPropertiesWithSource $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetAllVerbProperties>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (verbProps == null)
					{
						goto IL_92;
					}
					i = 0;
					break;
				case 1u:
					i++;
					break;
				case 2u:
					Block_5:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							k = enumerator.Current;
							this.$current = new VerbUtility.VerbPropertiesWithSource(k.verb, tools[j], k);
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					j++;
					goto IL_170;
				default:
					return false;
				}
				if (i < verbProps.Count)
				{
					this.$current = new VerbUtility.VerbPropertiesWithSource(verbProps[i]);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				IL_92:
				if (tools == null)
				{
					goto IL_186;
				}
				j = 0;
				IL_170:
				if (j < tools.Count)
				{
					enumerator = tools[j].Maneuvers.GetEnumerator();
					num = 4294967293u;
					goto Block_5;
				}
				IL_186:
				this.$PC = -1;
				return false;
			}

			VerbUtility.VerbPropertiesWithSource IEnumerator<VerbUtility.VerbPropertiesWithSource>.Current
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.VerbUtility.VerbPropertiesWithSource>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<VerbUtility.VerbPropertiesWithSource> IEnumerable<VerbUtility.VerbPropertiesWithSource>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				VerbUtility.<GetAllVerbProperties>c__Iterator0 <GetAllVerbProperties>c__Iterator = new VerbUtility.<GetAllVerbProperties>c__Iterator0();
				<GetAllVerbProperties>c__Iterator.verbProps = verbProps;
				<GetAllVerbProperties>c__Iterator.tools = tools;
				return <GetAllVerbProperties>c__Iterator;
			}
		}
	}
}
