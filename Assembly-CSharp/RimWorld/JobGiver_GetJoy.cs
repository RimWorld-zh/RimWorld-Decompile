#define ENABLE_PROFILER
using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_GetJoy : ThinkNode_JobGiver
	{
		private DefMap<JoyGiverDef, float> joyGiverChances;

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
			Job job;
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
					if (this.JoyGiverAllowed(joyGiverDef) && joyGiverDef.Worker.MissingRequiredCapacity(pawn) == null)
					{
						if (joyGiverDef.pctPawnsEverDo < 1.0)
						{
							Rand.PushState(pawn.thingIDNumber ^ 63216713);
							if (Rand.Value >= joyGiverDef.pctPawnsEverDo)
							{
								Rand.PopState();
								continue;
							}
							Rand.PopState();
						}
						float chance = joyGiverDef.Worker.GetChance(pawn);
						float num = (float)(1.0 - tolerances[joyGiverDef.joyKind]);
						chance *= num * num;
						this.joyGiverChances[joyGiverDef] = chance;
					}
				}
				int num2 = 0;
				JoyGiverDef def = default(JoyGiverDef);
				while (num2 < this.joyGiverChances.Count && ((IEnumerable<JoyGiverDef>)allDefsListForReading).TryRandomElementByWeight<JoyGiverDef>((Func<JoyGiverDef, float>)((JoyGiverDef d) => this.joyGiverChances[d]), out def))
				{
					job = this.TryGiveJobFromJoyGiverDefDirect(def, pawn);
					if (job != null)
						goto IL_0162;
					this.joyGiverChances[def] = 0f;
					num2++;
				}
				Profiler.EndSample();
				result = null;
			}
			goto IL_01a7;
			IL_0162:
			Profiler.EndSample();
			result = job;
			goto IL_01a7;
			IL_01a7:
			return result;
		}
	}
}
