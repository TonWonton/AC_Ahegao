#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

using LogLevel = BepInEx.Logging.LogLevel;
using System.Collections.Generic;


namespace AC_Ahegao
{
	[BepInProcess(PROCESS_NAME)]
	[BepInPlugin(GUID, PLUGIN_NAME, VERSION)]
	public partial class AhegaoPlugin : BasePlugin
	{
		#region PLUGIN_INFO

		/*PLUGIN INFO*/
		public const string PLUGIN_NAME = "AC_Ahegao";
		public const string COPYRIGHT = "";
		public const string COMPANY = "https://github.com/TonWonton/AC_Ahegao";

		public const string PROCESS_NAME = "Aicomi";
		public const string GUID = "AC_Ahegao";
		public const string VERSION = "1.0.0";

		#endregion



		/*VARIABLES*/
		//Instance
		public static AhegaoPlugin Instance { get; private set; } = null!;
		private static ManualLogSource _log = null!;

		//CustomFacialExpression
		private static Dictionary<AhegaoState, CustomFacialExpression> _customFacialExpressionFromAhegaoState = new Dictionary<AhegaoState, CustomFacialExpression>();
		public static CustomFacialExpression AhegaoOrgasmFacialExpression { get { return _customFacialExpressionFromAhegaoState[AhegaoState.Orgasm]; } }
		public static CustomFacialExpression AhegaoFaintnessFacialExpression { get { return _customFacialExpressionFromAhegaoState[AhegaoState.Faintness]; } }
		public static CustomFacialExpression AhegaoFaintnessSpeedFacialExpression { get { return _customFacialExpressionFromAhegaoState[AhegaoState.FaintnessSpeed]; } }

		public static bool TryGetAhegaoStateCustomFacialExpression(AhegaoState ahegaoState, out CustomFacialExpression customFacialExpression)
		{
			return _customFacialExpressionFromAhegaoState.TryGetValue(ahegaoState, out customFacialExpression);
		}



		/*METHODS*/
		public static bool TryGetAhegaoComponent([MaybeNullWhen(false)] out AhegaoComponent ahegaoComponent)
		{
			ahegaoComponent = AhegaoComponent.Instance;
			return ahegaoComponent != null;
		}

		public static AhegaoComponent GetOrAddAhegaoComponent()
		{
			AhegaoComponent? ahegaoComponent = AhegaoComponent.Instance;

			if (ahegaoComponent != null)
			{
				return ahegaoComponent;
			}
			else
			{
				return Instance.AddComponent<AhegaoComponent>();
			}
		}

		public static void CreateAhegaoOrgasmFacialExpression()
		{
			_customFacialExpressionFromAhegaoState[AhegaoState.Orgasm] = new CustomFacialExpression(AhegaoState.Orgasm);
		}

		public static void CreateAhegaoFaintnessFacialExpression()
		{
			_customFacialExpressionFromAhegaoState[AhegaoState.Faintness] = new CustomFacialExpression(AhegaoState.Faintness);
		}

		public static void CreateAhegaoFaintnessSpeedFacialExpression()
		{
			_customFacialExpressionFromAhegaoState[AhegaoState.FaintnessSpeed] = new CustomFacialExpression(AhegaoState.FaintnessSpeed);
		}



		/*EVENT HANDLING*/
		private static void OnAhegaoSettingChanged(object? sender, EventArgs args)
		{
			if (TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
			{
				ahegaoComponent.ForceUpdateAhegaos();
			}
		}

		private static void OnAhegaoOrgasmSettingChanged(object? sender, EventArgs args)
		{
			CreateAhegaoOrgasmFacialExpression();
			if (TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
			{
				ahegaoComponent.ForceUpdateAhegaos();
			}
		}

		private static void OnAhegaoFaintnessSettingChanged(object? sender, EventArgs args)
		{
			CreateAhegaoFaintnessFacialExpression();
			if (TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
			{
				ahegaoComponent.ForceUpdateAhegaos();
			}
		}

		private static void OnAhegaoFaintnessSpeedSettingChanged(object? sender, EventArgs args)
		{
			CreateAhegaoFaintnessSpeedFacialExpression();
			if (TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
			{
				ahegaoComponent.ForceUpdateAhegaos();
			}
		}



		/*PLUGIN LOAD*/
		public override void Load()
		{
			//Instance
			Instance = this;
			_log = Log;

			//Initialize
			InitializeConfig();
			CreateAhegaoOrgasmFacialExpression();
			CreateAhegaoFaintnessFacialExpression();
			CreateAhegaoFaintnessSpeedFacialExpression();
			AhegaoHActor.Initialize();

			//Create hooks
			Harmony.CreateAndPatchAll(typeof(AhegaoComponent.Hooks), GUID);
			Logging.Info("Loaded");
		}



		//Logging
		public static class Logging
		{
			public static void Log(LogLevel level, string message)
			{
				_log.Log(level, message);
			}

			public static void Fatal(string message)
			{
				_log.LogFatal(message);
			}

			public static void Error(string message)
			{
				_log.LogError(message);
			}

			public static void Warning(string message)
			{
				_log.LogWarning(message);
			}

			public static void Message(string message)
			{
				_log.LogMessage(message);
			}

			public static void Info(string message)
			{
				_log.LogInfo(message);
			}

			public static void Debug(string message)
			{
				_log.LogDebug(message);
			}
		}
	}
}