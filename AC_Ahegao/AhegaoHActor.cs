#nullable enable
using System;
using UnityEngine;
using Character;
using HarmonyLib;
using H;
using System.Collections.Generic;
using ILLGAMES.Unity;
using ILLGAMES.Unity.Animations;

using Logging = AC_Ahegao.AhegaoPlugin.Logging;
using Il2CppArrays = Il2CppInterop.Runtime.InteropTypes.Arrays;


namespace AC_Ahegao
{
	public class AhegaoHActor
	{
		/*VARIABLES*/
		//Harmony
		private static Harmony? _ahegaoHActorHooks;

		//Collections
		private static HashSet<HumanFace> _ahegaoFaceHashSet = new HashSet<HumanFace>();
		private static Dictionary<Human, AhegaoHActor> _ahegaoHActorFromHuman = new Dictionary<Human, AhegaoHActor>();
		private static HashSet<FBSBase> _ahegaoFBSBaseHashSet = new HashSet<FBSBase>();
		private static HashSet<EyeLookCalc> _ahegaoEyeLookCalcHashSet = new HashSet<EyeLookCalc>();

		//HActor
		private HActor _hActor;
		private Human _human;
		private HumanDataStatus _fileStatus;
		private HumanFace _face;
		private FBSCtrlEyebrow _fbsCtrlEyebrow;
		private FBSCtrlEyes _fbsCtrlEyes;
		private EyeLookController _eyeLookController;
		private EyeLookCalc _eyeLookCalc;
		private Il2CppArrays.Il2CppStructArray<float> _eyeLookCalcHAngleRate;
		private FBSCtrlMouth _fbsCtrlMouth;
		private CustomFacialExpression _originalFacialExpression;
		private bool _isMan = false;
		private AhegaoState _ahegaoState = AhegaoState.None;

		public HActor HActor { get { return _hActor; } }
		public CustomFacialExpression OriginalFacialExpression { get { return _originalFacialExpression; } }
		public AhegaoState AhegaoState { get { return _ahegaoState; } }

		//Eyes
		private float _eyesShakeTimer = 0f;
		private float _eyesRollTarget;
		private float _eyesCrossTarget;
		private float _eyesRollShakeTarget;
		private float _eyesCrossShakeTargetL;
		private float _eyesCrossShakeTargetR;



		/*METHODS*/
		public bool EnableFacialExpressionChange()
		{
			CustomFacialExpression originalFacialExpression = _originalFacialExpression;
			FBSCtrlMouth fBSCtrlMouth = _fbsCtrlMouth;
			_fbsCtrlEyes.FixedRate = -1f;
			fBSCtrlMouth.FixedRate = -1f;
			fBSCtrlMouth.randScaleMin = originalFacialExpression.mouthWidthMin;
			fBSCtrlMouth.randScaleMax = originalFacialExpression.mouthWidthMax;
			fBSCtrlMouth.openRefValue = originalFacialExpression.mouthWidthVariation;
			//_fileStatus.tearsLv = originalFacialExpression.tearsLv;
			//_face.ChangeHohoAkaRate(new Il2CppSystem.Nullable<float>(originalFacialExpression.blushAmount));
			return _ahegaoFaceHashSet.Remove(_face) | _ahegaoFBSBaseHashSet.Remove(_fbsCtrlEyebrow) | _ahegaoFBSBaseHashSet.Remove(_fbsCtrlEyes) | _ahegaoFBSBaseHashSet.Remove(_fbsCtrlMouth) | _ahegaoEyeLookCalcHashSet.Remove(_eyeLookCalc);
		}
		public bool DisableFacialExpressionChange()
		{
			return _ahegaoFaceHashSet.Add(_face) | _ahegaoFBSBaseHashSet.Add(_fbsCtrlEyebrow) | _ahegaoFBSBaseHashSet.Add(_fbsCtrlEyes) | _ahegaoFBSBaseHashSet.Add(_fbsCtrlMouth) | _ahegaoEyeLookCalcHashSet.Add(_eyeLookCalc);
		}

		public void SetAhegaoState(AhegaoState ahegaoState)
		{
			if (AhegaoPlugin.ahegao.Value == false || (_isMan && AhegaoPlugin.ahegaoMale.Value == false) || (_isMan == false && AhegaoPlugin.ahegaoFemale.Value == false))
			{
				_ahegaoState = AhegaoState.None;
				EnableFacialExpressionChange();
				return;
			}

			if (_ahegaoState != ahegaoState)
			{
				Logging.Info($"Setting {_hActor.fileParam.fullname} ahegao state to {ahegaoState}");
				_ahegaoState = ahegaoState;

				switch (ahegaoState)
				{
					case AhegaoState.None:
					{
						EnableFacialExpressionChange();
						return;
					}

					case AhegaoState.Faintness:
					{
						DisableFacialExpressionChange();
						SetFacialExpression(AhegaoPlugin.AhegaoFaintnessFacialExpression);
						return;
					}

					case AhegaoState.FaintnessSpeed:
					{
						DisableFacialExpressionChange();
						SetFacialExpression(AhegaoPlugin.AhegaoFaintnessSpeedFacialExpression);
						return;
					}

					case AhegaoState.Orgasm:
					{
						DisableFacialExpressionChange();
						SetFacialExpression(AhegaoPlugin.AhegaoOrgasmFacialExpression);
						return;
					}
				}
			}
		}

		public void SetAhegaoSettingType(AhegaoSettingType ahegaoSettingType)
		{
			if (AhegaoPlugin.ahegao.Value == false || (_isMan && AhegaoPlugin.ahegaoMale.Value == false) || (_isMan == false && AhegaoPlugin.ahegaoFemale.Value == false))
			{
				_ahegaoState = AhegaoState.None;
				EnableFacialExpressionChange();
				return;
			}

			switch (ahegaoSettingType)
			{
				case AhegaoSettingType.None: { return; }
				case AhegaoSettingType.Faintness: { SetAhegaoState(AhegaoState.Faintness); return; }
				case AhegaoSettingType.FaintnessSpeed: { SetAhegaoState(AhegaoState.FaintnessSpeed); return; }
				case AhegaoSettingType.Orgasm: { SetAhegaoState(AhegaoState.Orgasm); return; }
				case AhegaoSettingType.PlusOne:
				{
					switch (_ahegaoState)
					{
						case AhegaoState.None: { SetAhegaoState(AhegaoState.Faintness); return; }
						case AhegaoState.Faintness: { SetAhegaoState(AhegaoState.FaintnessSpeed); return; }
						case AhegaoState.FaintnessSpeed: { SetAhegaoState(AhegaoState.Orgasm); return; }
						case AhegaoState.Orgasm: { return; }
						default: { return; }
					}
				}
			}
		}

		public void ForceSetAhegaoState(AhegaoState ahegaoState)
		{
			if (AhegaoPlugin.ahegao.Value == false || (_isMan && AhegaoPlugin.ahegaoMale.Value == false) || (_isMan == false && AhegaoPlugin.ahegaoFemale.Value == false))
			{
				_ahegaoState = AhegaoState.None;
				EnableFacialExpressionChange();
				return;
			}

			_ahegaoState = ahegaoState;

			switch (ahegaoState)
			{
				case AhegaoState.None:
				{
					EnableFacialExpressionChange();
					return;
				}

				case AhegaoState.Faintness:
				{
					DisableFacialExpressionChange();
					SetFacialExpression(AhegaoPlugin.AhegaoFaintnessFacialExpression);
					return;
				}

				case AhegaoState.FaintnessSpeed:
				{
					DisableFacialExpressionChange();
					SetFacialExpression(AhegaoPlugin.AhegaoFaintnessSpeedFacialExpression);
					return;
				}

				case AhegaoState.Orgasm:
				{
					DisableFacialExpressionChange();
					SetFacialExpression(AhegaoPlugin.AhegaoOrgasmFacialExpression);
					return;
				}
			}
		}

		public void ForceSetAhegaoSettingType(AhegaoSettingType ahegaoSettingType)
		{
			if (AhegaoPlugin.ahegao.Value == false || (_isMan && AhegaoPlugin.ahegaoMale.Value == false) || (_isMan == false && AhegaoPlugin.ahegaoFemale.Value == false))
			{
				_ahegaoState = AhegaoState.None;
				EnableFacialExpressionChange();
				return;
			}

			switch (ahegaoSettingType)
			{
				case AhegaoSettingType.None: { return; }
				case AhegaoSettingType.Faintness: { ForceSetAhegaoState(AhegaoState.Faintness); return; }
				case AhegaoSettingType.FaintnessSpeed: { ForceSetAhegaoState(AhegaoState.FaintnessSpeed); return; }
				case AhegaoSettingType.Orgasm: { ForceSetAhegaoState(AhegaoState.Orgasm); return; }
				case AhegaoSettingType.PlusOne:
				{
					switch (_ahegaoState)
					{
						case AhegaoState.None: { ForceSetAhegaoState(AhegaoState.Faintness); return; }
						case AhegaoState.Faintness: { ForceSetAhegaoState(AhegaoState.FaintnessSpeed); return; }
						case AhegaoState.FaintnessSpeed: { ForceSetAhegaoState(AhegaoState.Orgasm); return; }
						case AhegaoState.Orgasm: { return; }
						default: { return; }
					}
				}
			}
		}

		private void SetFacialExpression(CustomFacialExpression customFacialExpression)
		{
			bool wasDisabled = EnableFacialExpressionChange();

			_fbsCtrlEyebrow.ChangePtn(customFacialExpression.eyebrowPtn, true);
			_fbsCtrlEyes.ChangePtn(customFacialExpression.eyesPtn, true);
			_fbsCtrlMouth.ChangePtn(customFacialExpression.mouthPtn, true);

			//_fbsCtrlEyebrow.FixedRate = 1f;
			_fbsCtrlMouth.randScaleMin = customFacialExpression.mouthWidthMin;
			_fbsCtrlMouth.randScaleMax = customFacialExpression.mouthWidthMax;
			_fbsCtrlMouth.openRefValue = customFacialExpression.mouthWidthVariation;

			float newAngleVRate = customFacialExpression.eyesRollAmount;
			_eyesRollTarget = newAngleVRate;
			_eyesRollShakeTarget = newAngleVRate;

			float newEyeAngleHRate = customFacialExpression.eyesCrossAmount;
			float minusNewEyeAngleHRate = -newEyeAngleHRate;
			_eyesCrossTarget = newEyeAngleHRate;
			_eyesCrossShakeTargetL = newEyeAngleHRate;
			_eyesCrossShakeTargetR = minusNewEyeAngleHRate;

			_fileStatus.tearsLv = customFacialExpression.tearsLv;
			_face.ChangeHohoAkaRate(new Il2CppSystem.Nullable<float>(_originalFacialExpression.blushAmount + customFacialExpression.blushAmount));

			if (wasDisabled) DisableFacialExpressionChange();
		}

		public void LateUpdate()
		{
			AhegaoState ahegaoState = _ahegaoState;

			if (ahegaoState != AhegaoState.None && AhegaoPlugin.TryGetAhegaoStateCustomFacialExpression(ahegaoState, out CustomFacialExpression customFacialExpression))
			{
				CustomFacialExpression originalFacialExpression = _originalFacialExpression;
				float eyesOpenMin = customFacialExpression.eyesOpenMin;
				float eyesOpenMax = customFacialExpression.eyesOpenMax;
				bool eyesBlink = customFacialExpression.eyesBlink;
				bool eyesShake = customFacialExpression.eyesShake;
				float eyesShakeAmount = customFacialExpression.eyesShakeAmount;
				float eyesShakeFrequency = 1.001f - customFacialExpression.eyesShakeFrequency;
				float mouthOpenMin = customFacialExpression.mouthOpenMin;
				float mouthOpenMax = customFacialExpression.mouthOpenMax;
				float mouthWidthMin = customFacialExpression.mouthWidthMin;
				float mouthWidthMax = customFacialExpression.mouthWidthMax;
				byte tearsLv = customFacialExpression.tearsLv;
				float blushAmount = customFacialExpression.blushAmount;

				//Update eyes
				FBSCtrlEyes fbsCtrlEyes = _fbsCtrlEyes;
				float correctOpenMax = fbsCtrlEyes._correctOpenMax;
				float eyesOpenRate = fbsCtrlEyes._openRate;
				if (eyesBlink)
				{
					if (correctOpenMax > 0f) fbsCtrlEyes.FixedRate = eyesOpenMax * eyesOpenRate * Mathf.Max(correctOpenMax, eyesOpenMin);
					else fbsCtrlEyes.FixedRate = Mathf.Max(eyesOpenMax * eyesOpenRate, eyesOpenMin);
				}
				else
				{
					fbsCtrlEyes.FixedRate = Mathf.Max(eyesOpenMax * correctOpenMax, eyesOpenMin);
				}

				//Update eyes look
				Il2CppArrays.Il2CppStructArray<float> eyeLookCalcHAngleRate = _eyeLookCalcHAngleRate;
				if (eyesShake)
				{
					if (_eyesShakeTimer > eyesShakeFrequency)
					{
						float shakeOffsetV = Random.Range(-eyesShakeAmount, eyesShakeAmount);
						_eyesRollShakeTarget = Mathf.Clamp_11(Mathf.ClampOffsetRadius(_eyesRollShakeTarget + shakeOffsetV, _eyesRollTarget, eyesShakeAmount));

						float shakeOffsetH = Random.Range(-eyesShakeAmount, eyesShakeAmount);
						_eyesCrossShakeTargetL = Mathf.Clamp_11(Mathf.ClampOffsetRadius(_eyesCrossShakeTargetL + shakeOffsetH, _eyesCrossTarget, eyesShakeAmount));
						_eyesCrossShakeTargetR = Mathf.Clamp_11(Mathf.ClampOffsetRadius(_eyesCrossShakeTargetR + shakeOffsetH, -_eyesCrossTarget, eyesShakeAmount));

						_eyesShakeTimer -= eyesShakeFrequency + (Random.Range(-eyesShakeFrequency, eyesShakeFrequency) * 0.5f);
					}
					else
					{
						_eyesShakeTimer += Time.deltaTime;
					}

					_eyeLookCalc.AngleVRate = _eyesRollShakeTarget;
					eyeLookCalcHAngleRate[0] = _eyesCrossShakeTargetL;
					eyeLookCalcHAngleRate[1] = _eyesCrossShakeTargetR;
				}
				else
				{
					_eyeLookCalc.AngleVRate = _eyesRollTarget;
					eyeLookCalcHAngleRate[0] = _eyesCrossTarget;
					eyeLookCalcHAngleRate[1] = -_eyesCrossTarget;
				}

				//Update mouth
				FBSCtrlMouth fbsCtrlMouth = _fbsCtrlMouth;
				float mouthOpenRate = fbsCtrlMouth._openRate;
				fbsCtrlMouth.FixedRate = Mathf.Max((mouthOpenMax - mouthOpenMin) * mouthOpenRate + mouthOpenMin, mouthOpenMin);
				fbsCtrlMouth.useAjustWidthScale = false;
				fbsCtrlMouth._adjustWidthScale = Mathf.Max((mouthWidthMax - mouthWidthMin) * mouthOpenRate + mouthWidthMin, mouthWidthMin);

				//Update tears and blush
				HumanDataStatus fileStatus = _fileStatus;
				if (fileStatus.tearsLv != tearsLv)
				{
					fileStatus.tearsLv = tearsLv;
				}

				float desiredBlushAmount = originalFacialExpression.blushAmount + blushAmount;
				if (fileStatus.hohoAkaRate != desiredBlushAmount)
				{
					_face.ChangeHohoAkaRate(new Il2CppSystem.Nullable<float>(desiredBlushAmount));
				}
			}
			else
			{
				return;
			}
		}



		/*EVENT HANDLING*/
		private static void OnAhegaoComponentStarted()
		{
			Logging.Info("Creating AhegaoHActor hooks");
			_ahegaoHActorHooks = Harmony.CreateAndPatchAll(typeof(Hooks), AhegaoPlugin.GUID + "AhegaoHActorHooks");
		}

		private static void OnAhegaoComponentDestroyed()
		{
			Logging.Info("Unpatching AhegaoHActor hooks");
			_ahegaoHActorHooks?.UnpatchSelf();

			_ahegaoFaceHashSet.Clear();
			_ahegaoHActorFromHuman.Clear();
			_ahegaoFBSBaseHashSet.Clear();
			_ahegaoEyeLookCalcHashSet.Clear();
		}



		/*INITIALIZATION*/
		public AhegaoHActor(HActor hActor, bool isMan)
		{
			_hActor = hActor;
			_isMan = isMan;

			Human human = hActor.Human;
			HumanFace face = human.face;

			_human = human;
			_fileStatus = human.fileStatus;
			_face = face;
			_fbsCtrlEyebrow = face.eyebrowCtrl;
			_fbsCtrlEyes = face.eyesCtrl;
			_fbsCtrlMouth = face.mouthCtrl;
			_eyeLookController = face.eyeLookCtrl;
			_eyeLookCalc = face.eyeLookCtrl.EyeLookScript;
			_eyeLookCalcHAngleRate = _eyeLookCalc.AngleHRate;
			_originalFacialExpression = new CustomFacialExpression(human);
			_ahegaoHActorFromHuman[human] = this;
		}

		public static void Initialize()
		{
			AhegaoComponent.Started += OnAhegaoComponentStarted;
			AhegaoComponent.Destroyed += OnAhegaoComponentDestroyed;
		}



		/*HOOKS*/
		public static class Hooks
		{
			[HarmonyPrefix]
			[HarmonyPatch(typeof(EyeLookCalc), nameof(EyeLookCalc.EyeUpdateCalc))]
			public static bool EyeLookCalcPreEyeUpdateCalc(EyeLookCalc __instance)
			{
				if (_ahegaoEyeLookCalcHashSet.Contains(__instance)) return false;
				else return true;
			}

			[HarmonyPrefix]
			[HarmonyPatch(typeof(HumanFace), nameof(HumanFace.ChangeEyesPtn))]
			public static bool HumanFacePreChangeEyesPtn(HumanFace __instance)
			{
				if (_ahegaoFaceHashSet.Contains(__instance)) return false;
				else return true;
			}

			[HarmonyPrefix]
			[HarmonyPatch(typeof(FBSBase), nameof(FBSBase.ChangePtn))]
			public static bool FBSBasePreChangePtn(FBSBase __instance)
			{
				if (_ahegaoFBSBaseHashSet.Contains(__instance)) return false;
				else return true;
			}
		}
	}
}