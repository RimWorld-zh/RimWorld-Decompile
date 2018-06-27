using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_GetJoy : ThinkNode_JobGiver
	{
		[Unsaved]
		private DefMap<JoyGiverDef, float> joyGiverChances;

		public JobGiver_GetJoy()
		{
		}

		protected virtual bool CanDoDuringMedicalRest
		{
			get
			{
				return false;
			}
		}

		protected virtual bool JoyGiverAllowed(JoyGiverDef def)
		{
			return true;
		}

		protected virtual Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			return def.Worker.TryGiveJob(pawn);
		}

		public override void ResolveReferences()
		{
			this.joyGiverChances = new DefMap<JoyGiverDef, float>();
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!this.CanDoDuringMedicalRest && pawn.InBed() && HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				result = null;
			}
			else
			{
				Profiler.BeginSample("GetJoy");
				List<JoyGiverDef> allDefsListForReading = DefDatabase<JoyGiverDef>.AllDefsListForReading;
				JoyToleranceSet tolerances = pawn.needs.joy.tolerances;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					JoyGiverDef joyGiverDef = allDefsListForReading[i];
					this.joyGiverChances[joyGiverDef] = 0f;
					if (this.JoyGiverAllowed(joyGiverDef))
					{
						if (!pawn.needs.joy.tolerances.BoredOf(joyGiverDef.joyKind))
						{
							if (joyGiverDef.Worker.MissingRequiredCapacity(pawn) == null)
							{
								if (joyGiverDef.pctPawnsEverDo < 1f)
								{
									Rand.PushState(pawn.thingIDNumber ^ 63216713);
									if (Rand.Value >= joyGiverDef.pctPawnsEverDo)
									{
										Rand.PopState();
										goto IL_150;
									}
									Rand.PopState();
								}
								float num = tolerances[joyGiverDef.joyKind];
								float num2 = Mathf.Pow(1f - num, 5f);
								num2 = Mathf.Max(0.001f, num2);
								this.joyGiverChances[joyGiverDef] = joyGiverDef.Worker.GetChance(pawn) * num2;
							}
						}
					}
					IL_150:;
				}
				for (int j = 0; j < this.joyGiverChances.Count; j++)
				{
					JoyGiverDef def;
					if (!allDefsListForReading.TryRandomElementByWeight((JoyGiverDef d) => this.joyGiverChances[d], out def))
					{
						break;
					}
					Job job = this.TryGiveJobFromJoyGiverDefDirect(def, pawn);
					if (job != null)
					{
						Profiler.EndSample();
						return job;
					}
					this.joyGiverChances[def] = 0f;
				}
				Profiler.EndSample();
				result = null;
			}
			return result;
		}

		[CompilerGenerated]
		private float <TryGiveJob>m__0(JoyGiverDef d)
		{
			return this.joyGiverChances[d];
		}
	}
}
