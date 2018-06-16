using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DC4 RID: 3524
	public class Sustainer
	{
		// Token: 0x06004E8D RID: 20109 RVA: 0x0028FEA0 File Offset: 0x0028E2A0
		public Sustainer(SoundDef def, SoundInfo info)
		{
			this.def = def;
			this.info = info;
			if (def.subSounds.Count > 0)
			{
				foreach (KeyValuePair<string, float> keyValuePair in info.DefinedParameters)
				{
					this.externalParams[keyValuePair.Key] = keyValuePair.Value;
				}
				if (def.HasSubSoundsInWorld)
				{
					if (info.IsOnCamera)
					{
						Log.Error("Playing sound " + def.ToString() + " on camera, but it has sub-sounds in the world.", false);
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

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06004E8E RID: 20110 RVA: 0x00290050 File Offset: 0x0028E450
		public bool Ended
		{
			get
			{
				return this.endRealTime >= 0f;
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06004E8F RID: 20111 RVA: 0x00290078 File Offset: 0x0028E478
		public float TimeSinceEnd
		{
			get
			{
				return Time.realtimeSinceStartup - this.endRealTime;
			}
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x06004E90 RID: 20112 RVA: 0x0029009C File Offset: 0x0028E49C
		public float CameraDistanceSquared
		{
			get
			{
				float result;
				if (this.info.IsOnCamera)
				{
					result = 0f;
				}
				else if (this.worldRootObject == null)
				{
					if (Prefs.DevMode)
					{
						Log.Error(string.Concat(new object[]
						{
							"Sustainer ",
							this.def,
							" info is ",
							this.info,
							" but its worldRootObject is null"
						}), false);
					}
					result = 0f;
				}
				else
				{
					result = (float)(Find.CameraDriver.MapPosition - this.worldRootObject.transform.position.ToIntVec3()).LengthHorizontalSquared;
				}
				return result;
			}
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x00290164 File Offset: 0x0028E564
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
				else if (this.info.Maintenance == MaintenanceType.PerFrame)
				{
					if (Time.frameCount > this.lastMaintainFrame + 1)
					{
						this.End();
						return;
					}
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

		// Token: 0x06004E92 RID: 20114 RVA: 0x00290290 File Offset: 0x0028E690
		private void UpdateRootObjectPosition()
		{
			if (this.worldRootObject != null)
			{
				this.worldRootObject.transform.position = this.info.Maker.Cell.ToVector3ShiftedWithAltitude(0f);
			}
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x002902E0 File Offset: 0x0028E6E0
		public void Maintain()
		{
			if (this.Ended)
			{
				Log.Error("Tried to maintain ended sustainer: " + this.def, false);
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

		// Token: 0x06004E94 RID: 20116 RVA: 0x00290357 File Offset: 0x0028E757
		public void End()
		{
			this.endRealTime = Time.realtimeSinceStartup;
			if (this.def.sustainFadeoutTime < 0.001f)
			{
				this.Cleanup();
			}
		}

		// Token: 0x06004E95 RID: 20117 RVA: 0x00290380 File Offset: 0x0028E780
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
			if (this.def.sustainStopSound != null)
			{
				if (this.worldRootObject != null)
				{
					Map map = this.info.Maker.Map;
					if (map != null)
					{
						SoundInfo soundInfo = SoundInfo.InMap(new TargetInfo(this.worldRootObject.transform.position.ToIntVec3(), map, false), MaintenanceType.None);
						this.def.sustainStopSound.PlayOneShot(soundInfo);
					}
				}
				else
				{
					this.def.sustainStopSound.PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
				}
			}
			if (this.worldRootObject != null)
			{
				UnityEngine.Object.Destroy(this.worldRootObject);
			}
			DebugSoundEventsLog.Notify_SustainerEnded(this, this.info);
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x002904A0 File Offset: 0x0028E8A0
		public string DebugString()
		{
			string text = this.def.defName;
			text = text + "\n  inScopePercent=" + this.scopeFader.inScopePercent;
			text = text + "\n  CameraDistanceSquared=" + this.CameraDistanceSquared;
			foreach (SubSustainer arg in this.subSustainers)
			{
				text = text + "\n  sub: " + arg;
			}
			return text;
		}

		// Token: 0x04003451 RID: 13393
		public SoundDef def;

		// Token: 0x04003452 RID: 13394
		public SoundInfo info;

		// Token: 0x04003453 RID: 13395
		internal GameObject worldRootObject;

		// Token: 0x04003454 RID: 13396
		private int lastMaintainTick;

		// Token: 0x04003455 RID: 13397
		private int lastMaintainFrame;

		// Token: 0x04003456 RID: 13398
		private float endRealTime = -1f;

		// Token: 0x04003457 RID: 13399
		private List<SubSustainer> subSustainers = new List<SubSustainer>();

		// Token: 0x04003458 RID: 13400
		public SoundParams externalParams = new SoundParams();

		// Token: 0x04003459 RID: 13401
		public SustainerScopeFader scopeFader = new SustainerScopeFader();
	}
}
