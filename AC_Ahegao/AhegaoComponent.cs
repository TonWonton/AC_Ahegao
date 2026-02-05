#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using H;

using Logging = AC_Ahegao.AhegaoPlugin.Logging;


namespace AC_Ahegao
{
	public class AhegaoComponent : MonoBehaviour
	{
		/*VARIABLES*/
		//Instance
		public static AhegaoComponent? Instance { get; private set; }

		//HScene
		private HScene _hScene = null!;
		private FlagControl _flagControl = null!;
		private HScene.HStatus.Faintness _faintness = null!;
		private Gauge _femaleGauge = null!;
		private Gauge _maleGauge = null!;

		private int _femaleOrgasmCount = 0;
		private int _maleOrgasmCount = 0;
		private bool _isNowOrgasm = false;
		private bool _isNowFemaleOrgasm = false;
		private bool _isNowMaleOrgasm = false;
		private bool _isNowFaintness = false;
		private bool _isNowFemaleFaintness = false;
		private bool _isNowMaleFaintness = false;
		private bool _isNowOLoop = false;
		private bool _isNowGaugeHit = false;
		private bool _isNowSpeedProc = false;
		private bool _isNowFFeelProc = false;
		private bool _isNowMFeelProc = false;
		private int _mode = -1;
		private int _modeCtrl = -1;
		private int _loopType = -1;
		private float _speed = 0f;

		private List<AhegaoHActor> _ahegaoHActorList = new List<AhegaoHActor>();
		private List<AhegaoHActor> _femaleAhegaoHActorList = new List<AhegaoHActor>();
		private List<AhegaoHActor> _maleAhegaoHActorList = new List<AhegaoHActor>();

		//Events
		public static event Action? Started;
		public static event Action? Destroyed;



		/*METHODS*/
		//Process state
		private void ProcessAhegao()
		{
			Logging.Info("Processing ahegao");
			//Get info
			AhegaoSettingType oLoopAhegaoSettingType = AhegaoPlugin.oLoopAhegaoSettingType.Value;
			AhegaoSettingType gaugeFeelHitAhegaoSettingType = AhegaoPlugin.gaugeFeelHitAhegaoSettingType.Value;

			//Process orgasm
			if (_isNowOrgasm && AhegaoPlugin.ahegaoOnOrgasm.Value)
			{
				if (_isNowFemaleOrgasm)
				{
					foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
					{
						femaleAhegaoHActor.SetAhegaoState(AhegaoState.Orgasm);
					}
				}
				if (_isNowMaleOrgasm)
				{
					foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
					{
						maleAhegaoHActor.SetAhegaoState(AhegaoState.Orgasm);
					}
				}
			}

			//Process OLoop
			else if (_isNowOLoop && oLoopAhegaoSettingType != AhegaoSettingType.None && oLoopAhegaoSettingType != AhegaoSettingType.PlusOne)
			{
				if (_isNowFFeelProc)
				{
					foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
					{
						femaleAhegaoHActor.SetAhegaoSettingType(oLoopAhegaoSettingType);
					}
				}
				if (_isNowMFeelProc)
				{
					foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
					{
						maleAhegaoHActor.SetAhegaoSettingType(oLoopAhegaoSettingType);
					}
				}
			}

			//Process gauge hit
			else if (_isNowGaugeHit && gaugeFeelHitAhegaoSettingType != AhegaoSettingType.None && gaugeFeelHitAhegaoSettingType != AhegaoSettingType.PlusOne)
			{
				if (_isNowFFeelProc)
				{
					foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
					{
						femaleAhegaoHActor.SetAhegaoSettingType(gaugeFeelHitAhegaoSettingType);
					}
				}
				if (_isNowMFeelProc)
				{
					foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
					{
						maleAhegaoHActor.SetAhegaoSettingType(gaugeFeelHitAhegaoSettingType);
					}
				}
			}

			//Process faintness
			else if (_isNowFaintness)
			{
				if (_isNowSpeedProc)
				{
					if (_isNowFemaleFaintness)
					{
						if (_isNowFFeelProc)
						{
							foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
							{
								femaleAhegaoHActor.SetAhegaoState(AhegaoState.FaintnessSpeed);
							}
						}
						else
						{
							foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
							{
								femaleAhegaoHActor.SetAhegaoState(AhegaoState.Faintness);
							}
						}
					}
					else
					{
						foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
						{
							femaleAhegaoHActor.SetAhegaoState(AhegaoState.None);
						}
					}
					if (_isNowMaleFaintness)
					{
						if (_isNowMFeelProc)
						{
							foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
							{
								maleAhegaoHActor.SetAhegaoState(AhegaoState.FaintnessSpeed);
							}
						}
						else
						{
							foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
							{
								maleAhegaoHActor.SetAhegaoState(AhegaoState.Faintness);
							}
						}
					}
					else
					{
						foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
						{
							maleAhegaoHActor.SetAhegaoState(AhegaoState.None);
						}
					}
				}
				else
				{
					if (_isNowFemaleFaintness)
					{
						foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
						{
							femaleAhegaoHActor.SetAhegaoState(AhegaoState.Faintness);
						}
					}
					else
					{
						foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
						{
							femaleAhegaoHActor.SetAhegaoState(AhegaoState.None);
						}
					}
					if (_isNowMaleFaintness)
					{
						foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
						{
							maleAhegaoHActor.SetAhegaoState(AhegaoState.Faintness);
						}
					}
					else
					{
						foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
						{
							maleAhegaoHActor.SetAhegaoState(AhegaoState.None);
						}
					}
				}
			}

			//If no condition met
			else
			{
				foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
				{
					ahegaoHActor.SetAhegaoState(AhegaoState.None);
				}
			}

			if (_isNowOLoop && oLoopAhegaoSettingType == AhegaoSettingType.PlusOne)
			{
				if (_isNowFFeelProc)
				{
					foreach (AhegaoHActor femaleHActor in _femaleAhegaoHActorList)
					{
						femaleHActor.SetAhegaoSettingType(AhegaoSettingType.PlusOne);
					}
				}
				if (_isNowMFeelProc)
				{
					foreach (AhegaoHActor maleHActor in _maleAhegaoHActorList)
					{
						maleHActor.SetAhegaoSettingType(AhegaoSettingType.PlusOne);
					}
				}
			}

			if (_isNowGaugeHit && gaugeFeelHitAhegaoSettingType == AhegaoSettingType.PlusOne)
			{
				if (_isNowFFeelProc)
				{
					foreach (AhegaoHActor femaleHActor in _femaleAhegaoHActorList)
					{
						femaleHActor.SetAhegaoSettingType(AhegaoSettingType.PlusOne);
					}
				}
				if (_isNowMFeelProc)
				{
					foreach (AhegaoHActor maleHActor in _maleAhegaoHActorList)
					{
						maleHActor.SetAhegaoSettingType(AhegaoSettingType.PlusOne);
					}
				}
			}
		}

		public void ForceUpdateAhegaos()
		{
			if (AhegaoPlugin.ahegao.Value)
			{
				Logging.Info("Force updating ahegaos");
				//Get info
				AhegaoSettingType oLoopAhegaoSettingType = AhegaoPlugin.oLoopAhegaoSettingType.Value;
				AhegaoSettingType gaugeFeelHitAhegaoSettingType = AhegaoPlugin.gaugeFeelHitAhegaoSettingType.Value;

				//Process orgasm
				if (_isNowOrgasm && AhegaoPlugin.ahegaoOnOrgasm.Value)
				{
					if (_isNowFemaleOrgasm)
					{
						foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
						{
							femaleAhegaoHActor.ForceSetAhegaoState(AhegaoState.Orgasm);
						}
					}
					if (_isNowMaleOrgasm)
					{
						foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
						{
							maleAhegaoHActor.ForceSetAhegaoState(AhegaoState.Orgasm);
						}
					}
				}

				//Process OLoop
				else if (_isNowOLoop && oLoopAhegaoSettingType != AhegaoSettingType.None && oLoopAhegaoSettingType != AhegaoSettingType.PlusOne)
				{
					if (_isNowFFeelProc)
					{
						foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
						{
							femaleAhegaoHActor.ForceSetAhegaoSettingType(oLoopAhegaoSettingType);
						}
					}
					if (_isNowMFeelProc)
					{
						foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
						{
							maleAhegaoHActor.ForceSetAhegaoSettingType(oLoopAhegaoSettingType);
						}
					}
				}

				//Process gauge hit
				else if (_isNowGaugeHit && gaugeFeelHitAhegaoSettingType != AhegaoSettingType.None && gaugeFeelHitAhegaoSettingType != AhegaoSettingType.PlusOne)
				{
					if (_isNowFFeelProc)
					{
						foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
						{
							femaleAhegaoHActor.ForceSetAhegaoSettingType(gaugeFeelHitAhegaoSettingType);
						}
					}
					if (_isNowMFeelProc)
					{
						foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
						{
							maleAhegaoHActor.ForceSetAhegaoSettingType(gaugeFeelHitAhegaoSettingType);
						}
					}
				}

				//Process faintness
				else if (_isNowFaintness)
				{
					if (_isNowSpeedProc)
					{
						if (_isNowFemaleFaintness)
						{
							if (_isNowFFeelProc)
							{
								foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
								{
									femaleAhegaoHActor.ForceSetAhegaoState(AhegaoState.FaintnessSpeed);
								}
							}
							else
							{
								foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
								{
									femaleAhegaoHActor.ForceSetAhegaoState(AhegaoState.Faintness);
								}
							}
						}
						else
						{
							foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
							{
								femaleAhegaoHActor.ForceSetAhegaoState(AhegaoState.None);
							}
						}
						if (_isNowMaleFaintness)
						{
							if (_isNowMFeelProc)
							{
								foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
								{
									maleAhegaoHActor.ForceSetAhegaoState(AhegaoState.FaintnessSpeed);
								}
							}
							else
							{
								foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
								{
									maleAhegaoHActor.ForceSetAhegaoState(AhegaoState.Faintness);
								}
							}
						}
						else
						{
							foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
							{
								maleAhegaoHActor.ForceSetAhegaoState(AhegaoState.None);
							}
						}
					}
					else
					{
						if (_isNowFemaleFaintness)
						{
							foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
							{
								femaleAhegaoHActor.ForceSetAhegaoState(AhegaoState.Faintness);
							}
						}
						else
						{
							foreach (AhegaoHActor femaleAhegaoHActor in _femaleAhegaoHActorList)
							{
								femaleAhegaoHActor.ForceSetAhegaoState(AhegaoState.None);
							}
						}
						if (_isNowMaleFaintness)
						{
							foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
							{
								maleAhegaoHActor.ForceSetAhegaoState(AhegaoState.Faintness);
							}
						}
						else
						{
							foreach (AhegaoHActor maleAhegaoHActor in _maleAhegaoHActorList)
							{
								maleAhegaoHActor.ForceSetAhegaoState(AhegaoState.None);
							}
						}
					}
				}

				//If no condition met
				else
				{
					foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
					{
						ahegaoHActor.ForceSetAhegaoState(AhegaoState.None);
					}
				}

				if (_isNowOLoop && oLoopAhegaoSettingType == AhegaoSettingType.PlusOne)
				{
					if (_isNowFFeelProc)
					{
						foreach (AhegaoHActor femaleHActor in _femaleAhegaoHActorList)
						{
							femaleHActor.ForceSetAhegaoSettingType(AhegaoSettingType.PlusOne);
						}
					}
					if (_isNowMFeelProc)
					{
						foreach (AhegaoHActor maleHActor in _maleAhegaoHActorList)
						{
							maleHActor.ForceSetAhegaoSettingType(AhegaoSettingType.PlusOne);
						}
					}
				}

				if (_isNowGaugeHit && gaugeFeelHitAhegaoSettingType == AhegaoSettingType.PlusOne)
				{
					if (_isNowFFeelProc)
					{
						foreach (AhegaoHActor femaleHActor in _femaleAhegaoHActorList)
						{
							femaleHActor.ForceSetAhegaoSettingType(AhegaoSettingType.PlusOne);
						}
					}
					if (_isNowMFeelProc)
					{
						foreach (AhegaoHActor maleHActor in _maleAhegaoHActorList)
						{
							maleHActor.ForceSetAhegaoSettingType(AhegaoSettingType.PlusOne);
						}
					}
				}
			}
			else
			{
				foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
				{
					ahegaoHActor.ForceSetAhegaoState(AhegaoState.None);
				}
			}
		}

		//Orgasm
		private void OnOrgasmProc()
		{
			_isNowOrgasm = true;
			int newFemaleOrgasmCount = _flagControl.Counter.GetFemaleOrgasmTotal();
			int newMaleOrgasmCount = _flagControl.Counter.GetMaleOrgasmTotal();
			int previousFemaleOrgasmCount = _femaleOrgasmCount;
			int previousMaleOrgasmCount = _maleOrgasmCount;

			_femaleOrgasmCount = newFemaleOrgasmCount;
			_maleOrgasmCount = newMaleOrgasmCount;

			if (previousFemaleOrgasmCount != newFemaleOrgasmCount)
			{
				_isNowFemaleOrgasm = true;
			}

			if (previousMaleOrgasmCount != newMaleOrgasmCount)
			{
				_isNowMaleOrgasm = true;
				if (_maleOrgasmCount >= AhegaoPlugin.orgasmAmount.Value)
				{
					OnMaleFaintnessProc();
				}
				else
				{
					OnMaleFaintnessEndProc();
				}
			}

			ProcessAhegao();
		}

		private void OnOrgasmEndProc()
		{
			_isNowOrgasm = false;
			_isNowFemaleOrgasm = false;
			_isNowMaleOrgasm = false;

			ProcessAhegao();
		}

		//Faintness
		private void OnFemaleFaintnessProc()
		{
			_isNowFaintness = true;
			_isNowFemaleFaintness = true;
			ProcessAhegao();
		}

		private void OnFemaleFaintnessEndProc()
		{
			_isNowFemaleFaintness = false;
			if (_isNowMaleFaintness == false) _isNowFaintness = false;
			ProcessAhegao();
		}

		private void OnMaleFaintnessProc()
		{
			_isNowFaintness = true;
			_isNowMaleFaintness = true;
			ProcessAhegao();
		}

		private void OnMaleFaintnessEndProc()
		{
			_isNowMaleFaintness = false;
			if (_isNowFemaleFaintness == false) _isNowFaintness = false;
			ProcessAhegao();
		}

		private void OnFFeelProc()
		{
			_isNowFFeelProc = true;
			_isNowMFeelProc = false;
			ProcessAhegao();
		}

		private void OnMFeelProc()
		{
			_isNowMFeelProc = true;
			_isNowFFeelProc = false;
			ProcessAhegao();
		}

		private void OnBothFeelProc()
		{
			_isNowFFeelProc = true;
			_isNowMFeelProc = true;
			ProcessAhegao();
		}

		private void OnFeelEndProc()
		{
			_isNowFFeelProc = false;
			_isNowMFeelProc = false;
			ProcessAhegao();
		}

		//Loop type
		private void OnSLoopProc()
		{
			_isNowOLoop = false;
			ProcessAhegao();
		}

		private void OnWLoopProc()
		{
			_isNowOLoop = false;
			ProcessAhegao();
		}

		private void OnOLoopProc()
		{
			_isNowOLoop = true;
			ProcessAhegao();
		}

		private void OnLoopProcEnd()
		{
			_isNowOLoop = false;
			ProcessAhegao();
		}

		//Gauge hit
		private void OnGaugeHitProc()
		{
			_isNowGaugeHit = true;
			ProcessAhegao();
		}

		private void OnGaugeHitProcEnd()
		{
			_isNowGaugeHit = false;
			ProcessAhegao();
		}

		//Speed
		private void OnSpeedProc()
		{
			_isNowSpeedProc = true;
			ProcessAhegao();
		}

		private void OnSpeedProcEnd()
		{
			_isNowSpeedProc = false;
			ProcessAhegao();
		}

		//Update
		private void ResetAllAhegaos()
		{
			foreach (AhegaoHActor ahegaoHActor in _ahegaoHActorList)
			{
				ahegaoHActor.SetAhegaoState(AhegaoState.None);
			}
		}

		private void AddAhegaoHActor(HActor? hActor, bool isMan)
		{
			//Create and add AhegaoHActor
			if (hActor != null)
			{
				AhegaoHActor ahegaoHActor = new AhegaoHActor(hActor, isMan);
				_ahegaoHActorList.Add(ahegaoHActor);

				if (isMan)
				{
					_maleAhegaoHActorList.Add(ahegaoHActor);
				}
				else
				{
					_femaleAhegaoHActorList.Add(ahegaoHActor);
				}
			}
		}

		private void Update()
		{
			//Process orgasm
			FlagControl flagControl = _flagControl;
			bool isNowOrgasm = flagControl.IsNowOrgasm;
			if (isNowOrgasm != _isNowOrgasm)
			{
				if (isNowOrgasm) OnOrgasmProc();
				else OnOrgasmEndProc();
			}

			//Process faintness
			bool isNowFemaleFaintness = _faintness.IsOn;
			if (isNowFemaleFaintness != _isNowFemaleFaintness)
			{
				if (isNowFemaleFaintness) OnFemaleFaintnessProc();
				else OnFemaleFaintnessEndProc();
			}

			//Process ProcBase mode
			int mode = _hScene._mode;
			int modeCtrl = _hScene._modeCtrl;
			if (mode != _mode)
			{
				_mode = mode;
				_modeCtrl = modeCtrl;

				switch (mode)
				{
					case 0: OnFFeelProc(); break; //Caress
					case 1: OnMFeelProc(); break; //Service
					case 2: OnBothFeelProc(); break; //Both
					case 4: OnFFeelProc(); break; //Masturbation
					case 6: OnFFeelProc(); break; //Les
					case 7: //3p
					{
						switch (modeCtrl)
						{
							case 1: OnMFeelProc(); break; //Service
							case 2: OnMFeelProc(); break; //Service
							case 4: OnBothFeelProc(); break; //Both
							default: OnBothFeelProc(); Logging.Warning($"Unknown feel proc for mode {mode} modeCtrl {modeCtrl}"); break; //Default to both
						}
						break;
					}
					case 9: //5p
					{
						switch (modeCtrl)
						{
							case 1: OnMFeelProc(); break; //Service
							case 3: OnBothFeelProc(); break; //Both
							default: OnBothFeelProc(); Logging.Warning($"Unknown feel proc for mode {mode} modeCtrl {modeCtrl}"); break; //Default to both
						}

						break;
					}

					default: OnBothFeelProc(); Logging.Warning($"Unknown feel proc for mode {mode} modeCtrl {modeCtrl}"); break; //Default to both
				}
			}

			//Process loop type
			int loopType = flagControl.LoopType;
			int previousLoopType = _loopType;
			if (loopType != _loopType)
			{
				_loopType = loopType;
				Logging.Info($"LoopType changed to {_loopType}, current speed = {flagControl.Speed}");

				switch (loopType)
				{
					case 0: OnSLoopProc(); break;
					case 1: OnWLoopProc(); break;
					case 2: OnOLoopProc(); break;
					case -1: OnLoopProcEnd(); break;
					default: OnLoopProcEnd(); Logging.Warning($"Unknown loop type: {loopType}"); break;
				}

				if (loopType == -1 && _isNowSpeedProc)
				{
					OnSpeedProcEnd();
				}
			}

			//Process gauge hit
			bool gaugeHit = _femaleGauge.Hit || _maleGauge.Hit;
			if (gaugeHit != _isNowGaugeHit)
			{
				if (gaugeHit) OnGaugeHitProc();
				else OnGaugeHitProcEnd();
			}

			//Process speed
			float speed = flagControl.Speed;
			float previousSpeed = _speed;
			if (speed != _speed && loopType != -1)
			{
				_speed = speed;

				float speedThreshold = AhegaoPlugin.faintnessSpeedThreshold.Value;
				float currentEffectiveSpeed;
				float previousEffectiveSpeed;

				//Calculate effective speed considering loop type
				if (AhegaoPlugin.faintnessSpeedConsiderLoopType.Value)
				{
					currentEffectiveSpeed = (loopType == 2) ? speed + 2f : speed;
					previousEffectiveSpeed = (previousLoopType == 2) ? previousSpeed + 2f : previousSpeed;
				}
				else
				{
					currentEffectiveSpeed = (loopType == 1) ? speed - 1f : speed;
					previousEffectiveSpeed = (previousLoopType == 1) ? previousSpeed - 1f : previousSpeed;
				}

				if (currentEffectiveSpeed >= speedThreshold && previousEffectiveSpeed < speedThreshold)
				{
					OnSpeedProc();
				}
				else if (currentEffectiveSpeed < speedThreshold && previousEffectiveSpeed >= speedThreshold)
				{
					OnSpeedProcEnd();
				}
			}
		}

		private void LateUpdate()
		{
			List<AhegaoHActor> ahegaoHActorList = _ahegaoHActorList;
			int ahegaoHActorCount = ahegaoHActorList.Count;
			for (int i = 0; i < ahegaoHActorCount; i++)
			{
				ahegaoHActorList[i].LateUpdate();
			}
		}



		//Initialization
		private void Start()
		{
			Logging.Info("Starting AhegaoComponent");
			//Get HActors
			Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<HActor> hActors = _hScene._hActorReceivers;
			foreach (HActor hActor in hActors)
			{
				AddAhegaoHActor(hActor, false);
			}

			Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<HActor> hActorsMen = _hScene._hActorAttackers;
			foreach (HActor hActor in hActorsMen)
			{
				AddAhegaoHActor(hActor, true);
			}

			//Invoke event
			Started?.Invoke();
			Logging.Info("AhegaoComponent started");
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Logging.Info("Destroying AhegaoComponent");
				Instance = null;
				Destroyed?.Invoke();
			}
		}

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(this);
			}
		}



		//Component specific hooks
		public static class Hooks
		{
			[HarmonyPrefix]
			[HarmonyPatch(typeof(HResult), "EvaluationResult")]
			[HarmonyPatch(typeof(HScene), "RestoreActors")]
			public static void HScenePreEnd()
			{
				if (AhegaoPlugin.TryGetAhegaoComponent(out AhegaoComponent? ahegaoComponent))
				{
					ahegaoComponent.ResetAllAhegaos();
					Destroy(ahegaoComponent);
				}
			}

			[HarmonyPostfix]
			[HarmonyPatch(typeof(ProcBase), nameof(ProcBase.Initialize))]
			public static void ProcBasePostInitialize(ProcBase __instance)
			{
				if (Instance == null)
				{
					AhegaoComponent ahegaoComponent = AhegaoPlugin.GetOrAddAhegaoComponent();
					ahegaoComponent._hScene = __instance._hScene;
					ahegaoComponent._flagControl = ahegaoComponent._hScene.CtrlFlag;
					ahegaoComponent._faintness = __instance._status.FaintnessState;
					ahegaoComponent._femaleGauge = __instance._gaugeF;
					ahegaoComponent._maleGauge = __instance._gaugeM;
				}
			}
		}
	}
}