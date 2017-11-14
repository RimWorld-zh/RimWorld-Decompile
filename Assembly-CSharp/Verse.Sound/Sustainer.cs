using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	public class Sustainer
	{
		public SoundDef def;

		public SoundInfo info;

		internal GameObject worldRootObject;

		private int lastMaintainTick;

		private int lastMaintainFrame;

		private float endRealTime = -1f;

		private List<SubSustainer> subSustainers = new List<SubSustainer>();

		public SoundParams externalParams = new SoundParams();

		public SustainerScopeFader scopeFader = new SustainerScopeFader();

		public bool Ended
		{
			get
			{
				return this.endRealTime >= 0.0;
			}
		}

		public float TimeSinceEnd
		{
			get
			{
				return Time.realtimeSinceStartup - this.endRealTime;
			}
		}

		public float CameraDistanceSquared
		{
			get
			{
				if (this.info.IsOnCamera)
				{
					return 0f;
				}
				if ((Object)this.worldRootObject == (Object)null)
				{
					if (Prefs.DevMode)
					{
						Log.Error("Sustainer " + this.def + " info is " + this.info + " but its worldRootObject is null");
					}
					return 0f;
				}
				return (float)(Find.CameraDriver.MapPosition - this.worldRootObject.transform.position.ToIntVec3()).LengthHorizontalSquared;
			}
		}

		public Sustainer(SoundDef def, SoundInfo info)
		{
			this.def = def;
			this.info = info;
			if (def.subSounds.Count > 0)
			{
				foreach (KeyValuePair<string, float> definedParameter in info.DefinedParameters)
				{
					this.externalParams[definedParameter.Key] = definedParameter.Value;
				}
				if (def.HasSubSoundsInWorld)
				{
					if (info.IsOnCamera)
					{
						Log.Error("Playing sound " + def.ToString() + " on camera, but it has sub-sounds in the world.");
					}
					this.worldRootObject = new GameObject("SustainerRootObject_" + def.defName);
					this.UpdateRootObjectPosition();
				}
				else if (!info.IsOnCamera)
				{
					info = SoundInfo.OnCamera(info.Maintenance);
				}
				Find.SoundRoot.sustainerManager.RegisterSustainer(this);
				if (!info.IsOnCamera)
				{
					Find.SoundRoot.sustainerManager.UpdateAllSustainerScopes();
				}
				for (int i = 0; i < def.subSounds.Count; i++)
				{
					this.subSustainers.Add(new SubSustainer(this, def.subSounds[i]));
				}
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
				this.lastMaintainFrame = Time.frameCount;
			});
		}

		public void SustainerUpdate()
		{
			if (!this.Ended)
			{
				if (this.info.Maintenance == MaintenanceType.PerTick)
				{
					if (Find.TickManager.TicksGame > this.lastMaintainTick + 1)
					{
						this.End();
						return;
					}
				}
				else if (this.info.Maintenance == MaintenanceType.PerFrame && Time.frameCount > this.lastMaintainFrame + 1)
				{
					this.End();
					return;
				}
			}
			else if (this.TimeSinceEnd > this.def.sustainFadeoutTime)
			{
				this.Cleanup();
			}
			if (this.def.subSounds.Count > 0)
			{
				if (!this.info.IsOnCamera && this.info.Maker.HasThing)
				{
					this.UpdateRootObjectPosition();
				}
				this.scopeFader.SustainerScopeUpdate();
				for (int i = 0; i < this.subSustainers.Count; i++)
				{
					this.subSustainers[i].SubSustainerUpdate();
				}
			}
		}

		private void UpdateRootObjectPosition()
		{
			if ((Object)this.worldRootObject != (Object)null)
			{
				this.worldRootObject.transform.position = this.info.Maker.Cell.ToVector3ShiftedWithAltitude(0f);
			}
		}

		public void Maintain()
		{
			if (this.Ended)
			{
				Log.Error("Tried to maintain ended sustainer: " + this.def);
			}
			else if (this.info.Maintenance == MaintenanceType.PerTick)
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}
			else if (this.info.Maintenance == MaintenanceType.PerFrame)
			{
				this.lastMaintainFrame = Time.frameCount;
			}
		}

		public void End()
		{
			this.endRealTime = Time.realtimeSinceStartup;
			if (this.def.sustainFadeoutTime < 0.0010000000474974513)
			{
				this.Cleanup();
			}
		}

		private void Cleanup()
		{
			if (this.def.subSounds.Count > 0)
			{
				Find.SoundRoot.sustainerManager.DeregisterSustainer(this);
				for (int i = 0; i < this.subSustainers.Count; i++)
				{
					this.subSustainers[i].Cleanup();
				}
			}
			if (this.def.sustainStopSound != string.Empty)
			{
				if ((Object)this.worldRootObject != (Object)null)
				{
					Map map = this.info.Maker.Map;
					if (map != null)
					{
						SoundInfo soundInfo = SoundInfo.InMap(new TargetInfo(this.worldRootObject.transform.position.ToIntVec3(), map, false), MaintenanceType.None);
						SoundDef.Named(this.def.sustainStopSound).PlayOneShot(soundInfo);
					}
				}
				else
				{
					SoundDef.Named(this.def.sustainStopSound).PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
				}
			}
			if ((Object)this.worldRootObject != (Object)null)
			{
				Object.Destroy(this.worldRootObject);
			}
			DebugSoundEventsLog.Notify_SustainerEnded(this, this.info);
		}

		public string DebugString()
		{
			string defName = this.def.defName;
			defName = defName + "\n  inScopePercent=" + this.scopeFader.inScopePercent;
			defName = defName + "\n  CameraDistanceSquared=" + this.CameraDistanceSquared;
			foreach (SubSustainer subSustainer in this.subSustainers)
			{
				defName = defName + "\n  sub: " + subSustainer;
			}
			return defName;
		}
	}
}
