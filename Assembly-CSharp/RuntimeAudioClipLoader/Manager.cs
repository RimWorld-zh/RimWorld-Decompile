using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RuntimeAudioClipLoader
{
	// Token: 0x020009DD RID: 2525
	[StaticConstructorOnStartup]
	public class Manager : MonoBehaviour
	{
		// Token: 0x0400241B RID: 9243
		private static readonly string[] supportedFormats;

		// Token: 0x0400241C RID: 9244
		private static Dictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();

		// Token: 0x0400241D RID: 9245
		private static Queue<Manager.AudioInstance> deferredLoadQueue = new Queue<Manager.AudioInstance>();

		// Token: 0x0400241E RID: 9246
		private static Queue<Manager.AudioInstance> deferredSetDataQueue = new Queue<Manager.AudioInstance>();

		// Token: 0x0400241F RID: 9247
		private static Queue<Manager.AudioInstance> deferredSetFail = new Queue<Manager.AudioInstance>();

		// Token: 0x04002420 RID: 9248
		private static Thread deferredLoaderThread;

		// Token: 0x04002421 RID: 9249
		private static GameObject managerInstance;

		// Token: 0x04002422 RID: 9250
		private static Dictionary<AudioClip, AudioClipLoadType> audioClipLoadType = new Dictionary<AudioClip, AudioClipLoadType>();

		// Token: 0x04002423 RID: 9251
		private static Dictionary<AudioClip, AudioDataLoadState> audioLoadState = new Dictionary<AudioClip, AudioDataLoadState>();

		// Token: 0x04002424 RID: 9252
		[CompilerGenerated]
		private static ThreadStart <>f__mg$cache0;

		// Token: 0x06003891 RID: 14481 RVA: 0x001E3BD0 File Offset: 0x001E1FD0
		static Manager()
		{
			Manager.supportedFormats = Enum.GetNames(typeof(AudioFormat));
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x001E3C38 File Offset: 0x001E2038
		public static AudioClip Load(string filePath, bool doStream = false, bool loadInBackground = true, bool useCache = true)
		{
			AudioClip result;
			if (!Manager.IsSupportedFormat(filePath))
			{
				Debug.LogError("Could not load AudioClip at path '" + filePath + "' it's extensions marks unsupported format, supported formats are: " + string.Join(", ", Enum.GetNames(typeof(AudioFormat))));
				result = null;
			}
			else
			{
				AudioClip audioClip = null;
				if (useCache && Manager.cache.TryGetValue(filePath, out audioClip) && audioClip)
				{
					result = audioClip;
				}
				else
				{
					StreamReader streamReader = new StreamReader(filePath);
					audioClip = Manager.Load(streamReader.BaseStream, Manager.GetAudioFormat(filePath), filePath, doStream, loadInBackground, true);
					if (useCache)
					{
						Manager.cache[filePath] = audioClip;
					}
					result = audioClip;
				}
			}
			return result;
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x001E3CF0 File Offset: 0x001E20F0
		public static AudioClip Load(Stream dataStream, AudioFormat audioFormat, string unityAudioClipName, bool doStream = false, bool loadInBackground = true, bool diposeDataStreamIfNotNeeded = true)
		{
			AudioClip audioClip = null;
			CustomAudioFileReader reader = null;
			try
			{
				reader = new CustomAudioFileReader(dataStream, audioFormat);
				Manager.AudioInstance audioInstance = new Manager.AudioInstance
				{
					reader = reader,
					samplesCount = (int)(reader.Length / (long)(reader.WaveFormat.BitsPerSample / 8))
				};
				if (doStream)
				{
					audioClip = AudioClip.Create(unityAudioClipName, audioInstance.samplesCount / audioInstance.channels, audioInstance.channels, audioInstance.sampleRate, doStream, delegate(float[] target)
					{
						reader.Read(target, 0, target.Length);
					}, delegate(int target)
					{
						reader.Seek((long)target, SeekOrigin.Begin);
					});
					audioInstance.audioClip = audioClip;
					Manager.SetAudioClipLoadType(audioInstance, AudioClipLoadType.Streaming);
					Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loaded);
				}
				else
				{
					audioClip = AudioClip.Create(unityAudioClipName, audioInstance.samplesCount / audioInstance.channels, audioInstance.channels, audioInstance.sampleRate, doStream);
					audioInstance.audioClip = audioClip;
					if (diposeDataStreamIfNotNeeded)
					{
						audioInstance.streamToDisposeOnceDone = dataStream;
					}
					Manager.SetAudioClipLoadType(audioInstance, AudioClipLoadType.DecompressOnLoad);
					Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loading);
					if (loadInBackground)
					{
						object obj = Manager.deferredLoadQueue;
						lock (obj)
						{
							Manager.deferredLoadQueue.Enqueue(audioInstance);
						}
						Manager.RunDeferredLoaderThread();
						Manager.EnsureInstanceExists();
					}
					else
					{
						audioInstance.dataToSet = new float[audioInstance.samplesCount];
						audioInstance.reader.Read(audioInstance.dataToSet, 0, audioInstance.dataToSet.Length);
						audioInstance.audioClip.SetData(audioInstance.dataToSet, 0);
						Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loaded);
					}
				}
			}
			catch (Exception ex)
			{
				Manager.SetAudioClipLoadState(audioClip, AudioDataLoadState.Failed);
				Debug.LogError(string.Concat(new object[]
				{
					"Could not load AudioClip named '",
					unityAudioClipName,
					"', exception:",
					ex
				}));
			}
			return audioClip;
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x001E3F14 File Offset: 0x001E2314
		private static void RunDeferredLoaderThread()
		{
			if (Manager.deferredLoaderThread == null || !Manager.deferredLoaderThread.IsAlive)
			{
				if (Manager.<>f__mg$cache0 == null)
				{
					Manager.<>f__mg$cache0 = new ThreadStart(Manager.DeferredLoaderMain);
				}
				Manager.deferredLoaderThread = new Thread(Manager.<>f__mg$cache0);
				Manager.deferredLoaderThread.IsBackground = true;
				Manager.deferredLoaderThread.Start();
			}
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x001E3F7C File Offset: 0x001E237C
		private static void DeferredLoaderMain()
		{
			Manager.AudioInstance audioInstance = null;
			bool flag = true;
			long num = 100000L;
			while (flag || num > 0L)
			{
				num -= 1L;
				object obj = Manager.deferredLoadQueue;
				lock (obj)
				{
					flag = (Manager.deferredLoadQueue.Count > 0);
					if (!flag)
					{
						continue;
					}
					audioInstance = Manager.deferredLoadQueue.Dequeue();
				}
				num = 100000L;
				try
				{
					audioInstance.dataToSet = new float[audioInstance.samplesCount];
					audioInstance.reader.Read(audioInstance.dataToSet, 0, audioInstance.dataToSet.Length);
					audioInstance.reader.Close();
					audioInstance.reader.Dispose();
					if (audioInstance.streamToDisposeOnceDone != null)
					{
						audioInstance.streamToDisposeOnceDone.Close();
						audioInstance.streamToDisposeOnceDone.Dispose();
						audioInstance.streamToDisposeOnceDone = null;
					}
					object obj2 = Manager.deferredSetDataQueue;
					lock (obj2)
					{
						Manager.deferredSetDataQueue.Enqueue(audioInstance);
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					object obj3 = Manager.deferredSetFail;
					lock (obj3)
					{
						Manager.deferredSetFail.Enqueue(audioInstance);
					}
				}
			}
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x001E40FC File Offset: 0x001E24FC
		private void Update()
		{
			Manager.AudioInstance audioInstance = null;
			bool flag = true;
			while (flag)
			{
				object obj = Manager.deferredSetDataQueue;
				lock (obj)
				{
					flag = (Manager.deferredSetDataQueue.Count > 0);
					if (!flag)
					{
						break;
					}
					audioInstance = Manager.deferredSetDataQueue.Dequeue();
				}
				audioInstance.audioClip.SetData(audioInstance.dataToSet, 0);
				Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loaded);
				audioInstance.audioClip = null;
				audioInstance.dataToSet = null;
			}
			object obj2 = Manager.deferredSetFail;
			lock (obj2)
			{
				while (Manager.deferredSetFail.Count > 0)
				{
					audioInstance = Manager.deferredSetFail.Dequeue();
					Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Failed);
				}
			}
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x001E41EC File Offset: 0x001E25EC
		private static void EnsureInstanceExists()
		{
			if (!Manager.managerInstance)
			{
				Manager.managerInstance = new GameObject("Runtime AudioClip Loader Manger singleton instance");
				Manager.managerInstance.hideFlags = HideFlags.HideAndDontSave;
				Manager.managerInstance.AddComponent<Manager>();
			}
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x001E4226 File Offset: 0x001E2626
		public static void SetAudioClipLoadState(AudioClip audioClip, AudioDataLoadState newLoadState)
		{
			Manager.audioLoadState[audioClip] = newLoadState;
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x001E4238 File Offset: 0x001E2638
		public static AudioDataLoadState GetAudioClipLoadState(AudioClip audioClip)
		{
			AudioDataLoadState result = AudioDataLoadState.Failed;
			if (audioClip != null)
			{
				result = audioClip.loadState;
				Manager.audioLoadState.TryGetValue(audioClip, out result);
			}
			return result;
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x001E4273 File Offset: 0x001E2673
		public static void SetAudioClipLoadType(AudioClip audioClip, AudioClipLoadType newLoadType)
		{
			Manager.audioClipLoadType[audioClip] = newLoadType;
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x001E4284 File Offset: 0x001E2684
		public static AudioClipLoadType GetAudioClipLoadType(AudioClip audioClip)
		{
			AudioClipLoadType result = (AudioClipLoadType)(-1);
			if (audioClip != null)
			{
				result = audioClip.loadType;
				Manager.audioClipLoadType.TryGetValue(audioClip, out result);
			}
			return result;
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x001E42C0 File Offset: 0x001E26C0
		private static string GetExtension(string filePath)
		{
			return Path.GetExtension(filePath).Substring(1).ToLower();
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x001E42E8 File Offset: 0x001E26E8
		public static bool IsSupportedFormat(string filePath)
		{
			return Manager.supportedFormats.Contains(Manager.GetExtension(filePath));
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x001E4310 File Offset: 0x001E2710
		public static AudioFormat GetAudioFormat(string filePath)
		{
			AudioFormat result = AudioFormat.unknown;
			try
			{
				result = (AudioFormat)Enum.Parse(typeof(AudioFormat), Manager.GetExtension(filePath), true);
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x001E4364 File Offset: 0x001E2764
		public static void ClearCache()
		{
			Manager.cache.Clear();
		}

		// Token: 0x020009DE RID: 2526
		private class AudioInstance
		{
			// Token: 0x04002425 RID: 9253
			public AudioClip audioClip;

			// Token: 0x04002426 RID: 9254
			public CustomAudioFileReader reader;

			// Token: 0x04002427 RID: 9255
			public float[] dataToSet;

			// Token: 0x04002428 RID: 9256
			public int samplesCount;

			// Token: 0x04002429 RID: 9257
			public Stream streamToDisposeOnceDone;

			// Token: 0x170008AF RID: 2223
			// (get) Token: 0x060038A2 RID: 14498 RVA: 0x001E437C File Offset: 0x001E277C
			public int channels
			{
				get
				{
					return this.reader.WaveFormat.Channels;
				}
			}

			// Token: 0x170008B0 RID: 2224
			// (get) Token: 0x060038A3 RID: 14499 RVA: 0x001E43A4 File Offset: 0x001E27A4
			public int sampleRate
			{
				get
				{
					return this.reader.WaveFormat.SampleRate;
				}
			}

			// Token: 0x060038A4 RID: 14500 RVA: 0x001E43CC File Offset: 0x001E27CC
			public static implicit operator AudioClip(Manager.AudioInstance ai)
			{
				return ai.audioClip;
			}
		}
	}
}
