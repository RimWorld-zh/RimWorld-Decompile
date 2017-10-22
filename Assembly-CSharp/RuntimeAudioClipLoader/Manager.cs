using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using Verse;

namespace RuntimeAudioClipLoader
{
	[StaticConstructorOnStartup]
	public class Manager : MonoBehaviour
	{
		private class AudioInstance
		{
			public AudioClip audioClip;

			public CustomAudioFileReader reader;

			public float[] dataToSet;

			public int samplesCount;

			public Stream streamToDisposeOnceDone;

			public int channels
			{
				get
				{
					return this.reader.WaveFormat.Channels;
				}
			}

			public int sampleRate
			{
				get
				{
					return this.reader.WaveFormat.SampleRate;
				}
			}

			public static implicit operator AudioClip(AudioInstance ai)
			{
				return ai.audioClip;
			}
		}

		private static readonly string[] supportedFormats;

		private static Dictionary<string, AudioClip> cache;

		private static Queue<AudioInstance> deferredLoadQueue;

		private static Queue<AudioInstance> deferredSetDataQueue;

		private static Queue<AudioInstance> deferredSetFail;

		private static Thread deferredLoaderThread;

		private static GameObject managerInstance;

		private static Dictionary<AudioClip, AudioClipLoadType> audioClipLoadType;

		private static Dictionary<AudioClip, AudioDataLoadState> audioLoadState;

		static Manager()
		{
			Manager.cache = new Dictionary<string, AudioClip>();
			Manager.deferredLoadQueue = new Queue<AudioInstance>();
			Manager.deferredSetDataQueue = new Queue<AudioInstance>();
			Manager.deferredSetFail = new Queue<AudioInstance>();
			Manager.audioClipLoadType = new Dictionary<AudioClip, AudioClipLoadType>();
			Manager.audioLoadState = new Dictionary<AudioClip, AudioDataLoadState>();
			Manager.supportedFormats = Enum.GetNames(typeof(AudioFormat));
		}

		public static AudioClip Load(string filePath, bool doStream = false, bool loadInBackground = true, bool useCache = true)
		{
			if (!Manager.IsSupportedFormat(filePath))
			{
				Debug.LogError("Could not load AudioClip at path '" + filePath + "' it's extensions marks unsupported format, supported formats are: " + string.Join(", ", Enum.GetNames(typeof(AudioFormat))));
				return null;
			}
			AudioClip audioClip = null;
			if (useCache && Manager.cache.TryGetValue(filePath, out audioClip) && (UnityEngine.Object)audioClip)
			{
				return audioClip;
			}
			StreamReader streamReader = new StreamReader(filePath);
			audioClip = Manager.Load(streamReader.BaseStream, Manager.GetAudioFormat(filePath), filePath, doStream, loadInBackground, true);
			if (useCache)
			{
				Manager.cache[filePath] = audioClip;
			}
			return audioClip;
		}

		public static AudioClip Load(Stream dataStream, AudioFormat audioFormat, string unityAudioClipName, bool doStream = false, bool loadInBackground = true, bool diposeDataStreamIfNotNeeded = true)
		{
			AudioClip audioClip = null;
			CustomAudioFileReader reader = null;
			try
			{
				reader = new CustomAudioFileReader(dataStream, audioFormat);
				AudioInstance audioInstance = new AudioInstance();
				audioInstance.reader = reader;
				audioInstance.samplesCount = (int)(reader.Length / (reader.WaveFormat.BitsPerSample / 8));
				AudioInstance audioInstance2 = audioInstance;
				if (doStream)
				{
					audioClip = (audioInstance2.audioClip = AudioClip.Create(unityAudioClipName, audioInstance2.samplesCount / audioInstance2.channels, audioInstance2.channels, audioInstance2.sampleRate, doStream, (AudioClip.PCMReaderCallback)delegate(float[] target)
					{
						reader.Read(target, 0, target.Length);
					}, (AudioClip.PCMSetPositionCallback)delegate(int target)
					{
						reader.Seek(target, SeekOrigin.Begin);
					}));
					Manager.SetAudioClipLoadType(audioInstance2, AudioClipLoadType.Streaming);
					Manager.SetAudioClipLoadState(audioInstance2, AudioDataLoadState.Loaded);
					return audioClip;
				}
				audioClip = (audioInstance2.audioClip = AudioClip.Create(unityAudioClipName, audioInstance2.samplesCount / audioInstance2.channels, audioInstance2.channels, audioInstance2.sampleRate, doStream));
				if (diposeDataStreamIfNotNeeded)
				{
					audioInstance2.streamToDisposeOnceDone = dataStream;
				}
				Manager.SetAudioClipLoadType(audioInstance2, AudioClipLoadType.DecompressOnLoad);
				Manager.SetAudioClipLoadState(audioInstance2, AudioDataLoadState.Loading);
				if (loadInBackground)
				{
					Queue<AudioInstance> obj = Manager.deferredLoadQueue;
					Monitor.Enter(obj);
					try
					{
						Manager.deferredLoadQueue.Enqueue(audioInstance2);
					}
					finally
					{
						Monitor.Exit(obj);
					}
					Manager.RunDeferredLoaderThread();
					Manager.EnsureInstanceExists();
					return audioClip;
				}
				audioInstance2.dataToSet = new float[audioInstance2.samplesCount];
				audioInstance2.reader.Read(audioInstance2.dataToSet, 0, audioInstance2.dataToSet.Length);
				audioInstance2.audioClip.SetData(audioInstance2.dataToSet, 0);
				Manager.SetAudioClipLoadState(audioInstance2, AudioDataLoadState.Loaded);
				return audioClip;
			}
			catch (Exception ex)
			{
				Manager.SetAudioClipLoadState(audioClip, AudioDataLoadState.Failed);
				Debug.LogError("Could not load AudioClip named '" + unityAudioClipName + "', exception:" + ex);
				return audioClip;
			}
		}

		private static void RunDeferredLoaderThread()
		{
			if (Manager.deferredLoaderThread != null && Manager.deferredLoaderThread.IsAlive)
				return;
			Manager.deferredLoaderThread = new Thread(new ThreadStart(Manager.DeferredLoaderMain));
			Manager.deferredLoaderThread.IsBackground = true;
			Manager.deferredLoaderThread.Start();
		}

		private static void DeferredLoaderMain()
		{
			AudioInstance audioInstance = null;
			bool flag = true;
			long num = 100000L;
			while (true)
			{
				if (!flag && num <= 0)
					break;
				num--;
				Queue<AudioInstance> obj = Manager.deferredLoadQueue;
				Monitor.Enter(obj);
				try
				{
					flag = (Manager.deferredLoadQueue.Count > 0);
					if (flag)
					{
						audioInstance = Manager.deferredLoadQueue.Dequeue();
						goto IL_0051;
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
				continue;
				IL_0051:
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
					Queue<AudioInstance> obj2 = Manager.deferredSetDataQueue;
					Monitor.Enter(obj2);
					try
					{
						Manager.deferredSetDataQueue.Enqueue(audioInstance);
					}
					finally
					{
						Monitor.Exit(obj2);
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					Queue<AudioInstance> obj3 = Manager.deferredSetFail;
					Monitor.Enter(obj3);
					try
					{
						Manager.deferredSetFail.Enqueue(audioInstance);
					}
					finally
					{
						Monitor.Exit(obj3);
					}
				}
			}
		}

		private void Update()
		{
			AudioInstance audioInstance = null;
			bool flag = true;
			for (; flag; audioInstance.audioClip.SetData(audioInstance.dataToSet, 0), Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loaded), audioInstance.audioClip = null, audioInstance.dataToSet = null)
			{
				Queue<AudioInstance> obj = Manager.deferredSetDataQueue;
				Monitor.Enter(obj);
				try
				{
					flag = (Manager.deferredSetDataQueue.Count > 0);
					if (flag)
					{
						audioInstance = Manager.deferredSetDataQueue.Dequeue();
						continue;
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
				break;
			}
			Queue<AudioInstance> obj2 = Manager.deferredSetFail;
			Monitor.Enter(obj2);
			try
			{
				while (Manager.deferredSetFail.Count > 0)
				{
					audioInstance = Manager.deferredSetFail.Dequeue();
					Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Failed);
				}
			}
			finally
			{
				Monitor.Exit(obj2);
			}
		}

		private static void EnsureInstanceExists()
		{
			if (!(UnityEngine.Object)Manager.managerInstance)
			{
				Manager.managerInstance = new GameObject("Runtime AudioClip Loader Manger singleton instance");
				Manager.managerInstance.hideFlags = HideFlags.HideAndDontSave;
				Manager.managerInstance.AddComponent<Manager>();
			}
		}

		public static void SetAudioClipLoadState(AudioClip audioClip, AudioDataLoadState newLoadState)
		{
			Manager.audioLoadState[audioClip] = newLoadState;
		}

		public static AudioDataLoadState GetAudioClipLoadState(AudioClip audioClip)
		{
			AudioDataLoadState result = AudioDataLoadState.Failed;
			if ((UnityEngine.Object)audioClip != (UnityEngine.Object)null)
			{
				result = audioClip.loadState;
				Manager.audioLoadState.TryGetValue(audioClip, out result);
			}
			return result;
		}

		public static void SetAudioClipLoadType(AudioClip audioClip, AudioClipLoadType newLoadType)
		{
			Manager.audioClipLoadType[audioClip] = newLoadType;
		}

		public static AudioClipLoadType GetAudioClipLoadType(AudioClip audioClip)
		{
			AudioClipLoadType result = (AudioClipLoadType)(-1);
			if ((UnityEngine.Object)audioClip != (UnityEngine.Object)null)
			{
				result = audioClip.loadType;
				Manager.audioClipLoadType.TryGetValue(audioClip, out result);
			}
			return result;
		}

		private static string GetExtension(string filePath)
		{
			return Path.GetExtension(filePath).Substring(1).ToLower();
		}

		public static bool IsSupportedFormat(string filePath)
		{
			return Manager.supportedFormats.Contains(Manager.GetExtension(filePath));
		}

		public static AudioFormat GetAudioFormat(string filePath)
		{
			AudioFormat result = AudioFormat.unknown;
			try
			{
				result = (AudioFormat)(int)Enum.Parse(typeof(AudioFormat), Manager.GetExtension(filePath), true);
				return result;
			}
			catch
			{
				return result;
			}
		}

		public static void ClearCache()
		{
			Manager.cache.Clear();
		}
	}
}
