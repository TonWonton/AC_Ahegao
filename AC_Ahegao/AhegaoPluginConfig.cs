using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;

using Desc = BepInEx.Configuration.ConfigDescription;
using Order = AC_Ahegao.ConfigurationManagerAttributes;


namespace AC_Ahegao
{
	public partial class AhegaoPlugin : BasePlugin
	{
		/*CONFIG*/
		//Ahegao
		public const string AHEGAO = "Ahegao";
		public static ConfigEntry<bool> ahegao = null!;
		public static ConfigEntry<bool> ahegaoFemale = null!;
		public static ConfigEntry<bool> ahegaoMale = null!;
		public static ConfigEntry<int> orgasmAmount = null!;
		public static ConfigEntry<bool> faintnessSpeedConsiderLoopType = null!;
		public static ConfigEntry<float> faintnessSpeedThreshold = null!;
		public static ConfigEntry<AhegaoSettingType> oLoopAhegaoSettingType = null!;
		public static ConfigEntry<AhegaoSettingType> gaugeFeelHitAhegaoSettingType = null!;

		//Ahegao orgasm
		public const string AHEGAO_ORGASM = "Ahegao Orgasm";
		public static ConfigEntry<bool> ahegaoOnOrgasm = null!;
		public static ConfigEntry<int> orgasmEyebrowPtn = null!;
		public static ConfigEntry<int> orgasmEyesPtn = null!;
		public static ConfigEntry<float> orgasmEyesOpenMin = null!;
		public static ConfigEntry<float> orgasmEyesOpenMax = null!;
		public static ConfigEntry<float> orgasmEyesRollAmount = null!;
		public static ConfigEntry<float> orgasmEyesCrossAmount = null!;
		public static ConfigEntry<bool> orgasmEyesBlink = null!;
		public static ConfigEntry<bool> orgasmEyesShake = null!;
		public static ConfigEntry<float> orgasmEyesShakeAmount = null!;
		public static ConfigEntry<float> orgasmEyesShakeFrequency = null!;
		public static ConfigEntry<byte> orgasmTearsLv = null!;
		public static ConfigEntry<int> orgasmMouthPtn = null!;
		public static ConfigEntry<float> orgasmMouthOpenMin = null!;
		public static ConfigEntry<float> orgasmMouthOpenMax = null!;
		public static ConfigEntry<float> orgasmMouthWidthMin = null!;
		public static ConfigEntry<float> orgasmMouthWidthMax = null!;
		public static ConfigEntry<float> orgasmBlushAmount = null!;

		//Ahegao faintness
		public const string AHEGAO_FAINTNESS = "Ahegao Faintness";
		public static ConfigEntry<int> faintnessEyebrowPtn = null!;
		public static ConfigEntry<int> faintnessEyesPtn = null!;
		public static ConfigEntry<float> faintnessEyesOpenMin = null!;
		public static ConfigEntry<float> faintnessEyesOpenMax = null!;
		public static ConfigEntry<float> faintnessEyesRollAmount = null!;
		public static ConfigEntry<float> faintnessEyesCrossAmount = null!;
		public static ConfigEntry<bool> faintnessEyesBlink = null!;
		public static ConfigEntry<bool> faintnessEyesShake = null!;
		public static ConfigEntry<float> faintnessEyesShakeAmount = null!;
		public static ConfigEntry<float> faintnessEyesShakeFrequency = null!;
		public static ConfigEntry<byte> faintnessTearsLv = null!;
		public static ConfigEntry<int> faintnessMouthPtn = null!;
		public static ConfigEntry<float> faintnessMouthOpenMin = null!;
		public static ConfigEntry<float> faintnessMouthOpenMax = null!;
		public static ConfigEntry<float> faintnessMouthWidthMin = null!;
		public static ConfigEntry<float> faintnessMouthWidthMax = null!;
		public static ConfigEntry<float> faintnessBlushAmount = null!;

		//Ahegao faintness speed
		public const string AHEGAO_FAINTNESS_SPEED = "Ahegao Faintness Speed";
		public static ConfigEntry<int> faintnessSpeedEyebrowPtn = null!;
		public static ConfigEntry<int> faintnessSpeedEyesPtn = null!;
		public static ConfigEntry<float> faintnessSpeedEyesOpenMin = null!;
		public static ConfigEntry<float> faintnessSpeedEyesOpenMax = null!;
		public static ConfigEntry<float> faintnessSpeedEyesRollAmount = null!;
		public static ConfigEntry<float> faintnessSpeedEyesCrossAmount = null!;
		public static ConfigEntry<bool> faintnessSpeedEyesBlink = null!;
		public static ConfigEntry<bool> faintnessSpeedEyesShake = null!;
		public static ConfigEntry<float> faintnessSpeedEyesShakeAmount = null!;
		public static ConfigEntry<float> faintnessSpeedEyesShakeFrequency = null!;
		public static ConfigEntry<byte> faintnessSpeedTearsLv = null!;
		public static ConfigEntry<int> faintnessSpeedMouthPtn = null!;
		public static ConfigEntry<float> faintnessSpeedMouthOpenMin = null!;
		public static ConfigEntry<float> faintnessSpeedMouthOpenMax = null!;
		public static ConfigEntry<float> faintnessSpeedMouthWidthMin = null!;
		public static ConfigEntry<float> faintnessSpeedMouthWidthMax = null!;
		public static ConfigEntry<float> faintnessSpeedBlushAmount = null!;

		//Description
		public const string EYEBROW_PTN = "1. Eyebrow pattern (まゆげパターン)";
		public const string EYES_PTN = "2. Eye pattern (目パターン)";
		public const string EYES_MIN_OPEN = "- Eyes min open amount (目オープン MIN)";
		public const string EYES_MAX_OPEN = "- Eyes open amount (目オープン MAX)";
		public const string EYES_ROLL_AMOUNT = "- Eyes roll amount offset (白目Y)";
		public const string EYES_CROSS_AMOUNT = "- Eyes cross amount offset (白目X)";
		public const string EYES_BLINK = "- Eyes blink (目まばたき)";
		public const string EYES_SHAKE = "- Eyes shake (目ゆれ)";
		public const string EYES_SHAKE_AMOUNT = "- Eyes shake amount (目ゆれ量)";
		public const string EYES_SHAKE_FREQUENCY = "- Eyes shake frequency (目ゆれ速度)";
		public const string TEARS_LV = "- Tears level (なみだレベル)";
		public const string MOUTH_PTN = "3. Mouth pattern (口パターン)";
		public const string MOUTH_OPEN_MIN = "- Mouth min open amount (口オープンY MIN)";
		public const string MOUTH_OPEN_MAX = "- Mouth max open amount (口オープンY MAX)";
		public const string MOUTH_WIDTH_MIN = "- Mouth min width open amount (口オープンX MIN)";
		public const string MOUTH_WIDTH_MAX = "- Mouth max width open amount (口オープンX MAX)";
		public const string BLUSH_AMOUNT = "4. Blush amount offset (ほほあか)";

		private static int _currentOrder = 0;
		private static Order CurrentOrder { get { return new Order() { Order = _currentOrder-- }; } }
		private static void ResetOrder() { _currentOrder = 0; }

		private Desc EyebrowPtnDescription => new Desc(string.Empty, new AcceptableValueRange<int>(0, 11), CurrentOrder);
		private Desc EyesPtnDescription => new Desc(string.Empty, new AcceptableValueRange<int>(0, 33), CurrentOrder);
		private Desc EyesOpenMinDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1f), CurrentOrder);
		private Desc EyesOpenMaxDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1f), CurrentOrder);
		private Desc EyesRollAmountDescription => new Desc(string.Empty, new AcceptableValueRange<float>(-1.2f, 1.2f), CurrentOrder);
		private Desc EyesCrossAmountDescription => new Desc(string.Empty, new AcceptableValueRange<float>(-1.2f, 1.2f), CurrentOrder);
		private Desc EyesBlinkDescription => new Desc(string.Empty, null, CurrentOrder);
		private Desc EyesShakeDescription => new Desc(string.Empty, null, CurrentOrder);
		private Desc EyesShakeAmountDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 0.8f), CurrentOrder);
		private Desc EyesShakeFrequencyDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1.001f), CurrentOrder);
		private Desc TearsLvDescription => new Desc(string.Empty, new AcceptableValueList<byte>(0, 1, 2, 3), CurrentOrder);
		private Desc MouthPtnDescription => new Desc(string.Empty, new AcceptableValueRange<int>(0, 27), CurrentOrder);
		private Desc MouthOpenMinDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1f), CurrentOrder);
		private Desc MouthOpenMaxDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1f), CurrentOrder);
		private Desc MouthWidthMinDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1f), CurrentOrder);
		private Desc MouthWidthMaxDescription => new Desc(string.Empty, new AcceptableValueRange<float>(0f, 1f), CurrentOrder);
		private Desc BlushAmountDescription => new Desc(string.Empty, new AcceptableValueRange<float>(-1.6f, 3.4f), CurrentOrder);

		private void InitializeConfig()
		{
			//Ahegao
			ahegao = Config.Bind(AHEGAO, "Ahegao (アヘ顔)", true, new Desc(string.Empty, null, CurrentOrder));
			ahegaoOnOrgasm = Config.Bind(AHEGAO, "Ahegao on orgasm (オーガズムアヘ顔)", true, new Desc(string.Empty, null, CurrentOrder));
			ahegaoFemale = Config.Bind(AHEGAO, "Ahegao for female characters (女性アヘ顔)", true, new Desc(string.Empty, null, CurrentOrder));
			ahegaoMale = Config.Bind(AHEGAO, "Ahegao for male characters (男性アヘ顔)", false, new Desc(string.Empty, null, CurrentOrder));
			orgasmAmount = Config.Bind(AHEGAO, "Orgasm amount for faintness", 3, new Desc(string.Empty, new AcceptableValueRange<int>(1, 10), CurrentOrder));
			faintnessSpeedConsiderLoopType = Config.Bind(AHEGAO, "Single faintness speed threshold", true, new Desc(string.Empty, null, CurrentOrder));
			faintnessSpeedThreshold = Config.Bind(AHEGAO, "Faintness speed threshold (Faintness speed スピードリミット)", 0.8f, new Desc(string.Empty, new AcceptableValueRange<float>(0f, 3f), CurrentOrder));
			oLoopAhegaoSettingType = Config.Bind(AHEGAO, "Override ahegao type in OLoop with (OLoopアヘ顔オーバーライド)", AhegaoSettingType.FaintnessSpeed, new Desc(string.Empty, null, CurrentOrder));
			gaugeFeelHitAhegaoSettingType = Config.Bind(AHEGAO, "Override ahegao type in gauge hit with (ゲージフヒットアヘ顔オーバーライド)", AhegaoSettingType.None, new Desc(string.Empty, null, CurrentOrder));
			ResetOrder();

			//Ahegao orgasm
			orgasmEyebrowPtn = Config.Bind(AHEGAO_ORGASM, EYEBROW_PTN, 8, EyebrowPtnDescription);
			orgasmEyesPtn = Config.Bind(AHEGAO_ORGASM, EYES_PTN, 5, EyesPtnDescription);
			orgasmEyesOpenMin = Config.Bind(AHEGAO_ORGASM, EYES_MIN_OPEN, 0.42f, EyesOpenMinDescription);
			orgasmEyesOpenMax = Config.Bind(AHEGAO_ORGASM, EYES_MAX_OPEN, 0.62f, EyesOpenMaxDescription);
			orgasmEyesRollAmount = Config.Bind(AHEGAO_ORGASM, EYES_ROLL_AMOUNT, 0.56f, EyesRollAmountDescription);
			orgasmEyesCrossAmount = Config.Bind(AHEGAO_ORGASM, EYES_CROSS_AMOUNT, 0.48f, EyesCrossAmountDescription);
			orgasmEyesBlink = Config.Bind(AHEGAO_ORGASM, EYES_BLINK, false, EyesBlinkDescription);
			orgasmEyesShake = Config.Bind(AHEGAO_ORGASM, EYES_SHAKE, true, EyesShakeDescription);
			orgasmEyesShakeAmount = Config.Bind(AHEGAO_ORGASM, EYES_SHAKE_AMOUNT, 0.12f, EyesShakeAmountDescription);
			orgasmEyesShakeFrequency = Config.Bind(AHEGAO_ORGASM, EYES_SHAKE_FREQUENCY, 0.964f, EyesShakeFrequencyDescription);
			orgasmTearsLv = Config.Bind(AHEGAO_ORGASM, TEARS_LV, (byte)3, TearsLvDescription);
			orgasmMouthPtn = Config.Bind(AHEGAO_ORGASM, MOUTH_PTN, 18, MouthPtnDescription);
			orgasmMouthOpenMin = Config.Bind(AHEGAO_ORGASM, MOUTH_OPEN_MIN, 0.8f, MouthOpenMinDescription);
			orgasmMouthOpenMax = Config.Bind(AHEGAO_ORGASM, MOUTH_OPEN_MAX, 1f, MouthOpenMaxDescription);
			orgasmMouthWidthMin = Config.Bind(AHEGAO_ORGASM, MOUTH_WIDTH_MIN, 0.52f, MouthWidthMinDescription);
			orgasmMouthWidthMax = Config.Bind(AHEGAO_ORGASM, MOUTH_WIDTH_MAX, 0.68f, MouthWidthMaxDescription);
			orgasmBlushAmount = Config.Bind(AHEGAO_ORGASM, BLUSH_AMOUNT, 1.26f, BlushAmountDescription);
			ResetOrder();

			//Ahegao faintness
			faintnessEyebrowPtn = Config.Bind(AHEGAO_FAINTNESS, EYEBROW_PTN, 5, EyebrowPtnDescription);
			faintnessEyesPtn = Config.Bind(AHEGAO_FAINTNESS, EYES_PTN, 2, EyesPtnDescription);
			faintnessEyesOpenMin = Config.Bind(AHEGAO_FAINTNESS, EYES_MIN_OPEN, 0f, EyesOpenMinDescription);
			faintnessEyesOpenMax = Config.Bind(AHEGAO_FAINTNESS, EYES_MAX_OPEN, 1f, EyesOpenMaxDescription);
			faintnessEyesRollAmount = Config.Bind(AHEGAO_FAINTNESS, EYES_ROLL_AMOUNT, 0.32f, EyesRollAmountDescription);
			faintnessEyesCrossAmount = Config.Bind(AHEGAO_FAINTNESS, EYES_CROSS_AMOUNT, 0.24f, EyesCrossAmountDescription);
			faintnessEyesBlink = Config.Bind(AHEGAO_FAINTNESS, EYES_BLINK, true, EyesBlinkDescription);
			faintnessEyesShake = Config.Bind(AHEGAO_FAINTNESS, EYES_SHAKE, true, EyesShakeDescription);
			faintnessEyesShakeAmount = Config.Bind(AHEGAO_FAINTNESS, EYES_SHAKE_AMOUNT, 0.06f, EyesShakeAmountDescription);
			faintnessEyesShakeFrequency = Config.Bind(AHEGAO_FAINTNESS, EYES_SHAKE_FREQUENCY, 0.12f, EyesShakeFrequencyDescription);
			faintnessTearsLv = Config.Bind(AHEGAO_FAINTNESS, TEARS_LV, (byte)1, TearsLvDescription);
			faintnessMouthPtn = Config.Bind(AHEGAO_FAINTNESS, MOUTH_PTN, 6, MouthPtnDescription);
			faintnessMouthOpenMin = Config.Bind(AHEGAO_FAINTNESS, MOUTH_OPEN_MIN, 0.32f, MouthOpenMinDescription);
			faintnessMouthOpenMax = Config.Bind(AHEGAO_FAINTNESS, MOUTH_OPEN_MAX, 0.8f, MouthOpenMaxDescription);
			faintnessMouthWidthMin = Config.Bind(AHEGAO_FAINTNESS, MOUTH_WIDTH_MIN, 0.48f, MouthWidthMinDescription);
			faintnessMouthWidthMax = Config.Bind(AHEGAO_FAINTNESS, MOUTH_WIDTH_MAX, 0.86f, MouthWidthMaxDescription);
			faintnessBlushAmount = Config.Bind(AHEGAO_FAINTNESS, BLUSH_AMOUNT, 1f, BlushAmountDescription);
			ResetOrder();

			//Ahegao faintness speed
			faintnessSpeedEyebrowPtn = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYEBROW_PTN, 6, EyebrowPtnDescription);
			faintnessSpeedEyesPtn = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_PTN, 9, EyesPtnDescription);
			faintnessSpeedEyesOpenMin = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_MIN_OPEN, 0.68f, EyesOpenMinDescription);
			faintnessSpeedEyesOpenMax = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_MAX_OPEN, 0.86f, EyesOpenMaxDescription);
			faintnessSpeedEyesRollAmount = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_ROLL_AMOUNT, 0.42f, EyesRollAmountDescription);
			faintnessSpeedEyesCrossAmount = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_CROSS_AMOUNT, 0.38f, EyesCrossAmountDescription);
			faintnessSpeedEyesBlink = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_BLINK, true, EyesBlinkDescription);
			faintnessSpeedEyesShake = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_SHAKE, true, EyesShakeDescription);
			faintnessSpeedEyesShakeAmount = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_SHAKE_AMOUNT, 0.1f, EyesShakeAmountDescription);
			faintnessSpeedEyesShakeFrequency = Config.Bind(AHEGAO_FAINTNESS_SPEED, EYES_SHAKE_FREQUENCY, 0.32f, EyesShakeFrequencyDescription);
			faintnessSpeedTearsLv = Config.Bind(AHEGAO_FAINTNESS_SPEED, TEARS_LV, (byte)1, TearsLvDescription);
			faintnessSpeedMouthPtn = Config.Bind(AHEGAO_FAINTNESS_SPEED, MOUTH_PTN, 7, MouthPtnDescription);
			faintnessSpeedMouthOpenMin = Config.Bind(AHEGAO_FAINTNESS_SPEED, MOUTH_OPEN_MIN, 0.32f, MouthOpenMinDescription);
			faintnessSpeedMouthOpenMax = Config.Bind(AHEGAO_FAINTNESS_SPEED, MOUTH_OPEN_MAX, 0.52f, MouthOpenMaxDescription);
			faintnessSpeedMouthWidthMin = Config.Bind(AHEGAO_FAINTNESS_SPEED, MOUTH_WIDTH_MIN, 0.62f, MouthWidthMinDescription);
			faintnessSpeedMouthWidthMax = Config.Bind(AHEGAO_FAINTNESS_SPEED, MOUTH_WIDTH_MAX, 0.82f, MouthWidthMaxDescription);
			faintnessSpeedBlushAmount = Config.Bind(AHEGAO_FAINTNESS_SPEED, BLUSH_AMOUNT, 1.12f, BlushAmountDescription);
			ResetOrder();

			//Register setting changed event
			ahegao.SettingChanged += OnAhegaoSettingChanged;
			ahegaoOnOrgasm.SettingChanged += OnAhegaoSettingChanged;
			ahegaoFemale.SettingChanged += OnAhegaoSettingChanged;
			ahegaoMale.SettingChanged += OnAhegaoSettingChanged;
			orgasmAmount.SettingChanged += OnAhegaoSettingChanged;
			faintnessSpeedConsiderLoopType.SettingChanged += OnAhegaoSettingChanged;
			faintnessSpeedThreshold.SettingChanged += OnAhegaoSettingChanged;
			oLoopAhegaoSettingType.SettingChanged += OnAhegaoSettingChanged;
			gaugeFeelHitAhegaoSettingType.SettingChanged += OnAhegaoSettingChanged;

			orgasmEyebrowPtn.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesPtn.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesOpenMin.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesOpenMax.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesRollAmount.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesCrossAmount.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesBlink.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesShake.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesShakeAmount.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmEyesShakeFrequency.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmTearsLv.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmMouthPtn.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmMouthOpenMin.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmMouthOpenMax.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmMouthWidthMin.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmMouthWidthMax.SettingChanged += OnAhegaoOrgasmSettingChanged;
			orgasmBlushAmount.SettingChanged += OnAhegaoOrgasmSettingChanged;

			faintnessEyebrowPtn.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesPtn.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesOpenMin.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesOpenMax.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesRollAmount.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesCrossAmount.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesBlink.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesShake.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesShakeAmount.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessEyesShakeFrequency.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessTearsLv.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessMouthPtn.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessMouthOpenMin.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessMouthOpenMax.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessMouthWidthMin.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessMouthWidthMax.SettingChanged += OnAhegaoFaintnessSettingChanged;
			faintnessBlushAmount.SettingChanged += OnAhegaoFaintnessSettingChanged;

			faintnessSpeedEyebrowPtn.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesPtn.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesOpenMin.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesOpenMax.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesRollAmount.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesCrossAmount.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesBlink.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesShake.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesShakeAmount.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedEyesShakeFrequency.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedTearsLv.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedMouthPtn.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedMouthOpenMin.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedMouthOpenMax.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedMouthWidthMin.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedMouthWidthMax.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
			faintnessSpeedBlushAmount.SettingChanged += OnAhegaoFaintnessSpeedSettingChanged;
		}
	}

#pragma warning disable 0169, 0414, 0649
	internal sealed class ConfigurationManagerAttributes
	{
		/// <summary>
		/// Should the setting be shown as a percentage (only use with value range settings).
		/// </summary>
		public bool? ShowRangeAsPercent;

		/// <summary>
		/// Custom setting editor (OnGUI code that replaces the default editor provided by ConfigurationManager).
		/// See below for a deeper explanation. Using a custom drawer will cause many of the other fields to do nothing.
		/// </summary>
		public System.Action<BepInEx.Configuration.ConfigEntryBase> CustomDrawer;

		/// <summary>
		/// Custom setting editor that allows polling keyboard input with the Input (or UnityInput) class.
		/// Use either CustomDrawer or CustomHotkeyDrawer, using both at the same time leads to undefined behaviour.
		/// </summary>
		public CustomHotkeyDrawerFunc CustomHotkeyDrawer;

		/// <summary>
		/// Custom setting draw action that allows polling keyboard input with the Input class.
		/// Note: Make sure to focus on your UI control when you are accepting input so user doesn't type in the search box or in another setting (best to do this on every frame).
		/// If you don't draw any selectable UI controls You can use `GUIUtility.keyboardControl = -1;` on every frame to make sure that nothing is selected.
		/// </summary>
		/// <example>
		/// CustomHotkeyDrawer = (ConfigEntryBase setting, ref bool isEditing) =>
		/// {
		///     if (isEditing)
		///     {
		///         // Make sure nothing else is selected since we aren't focusing on a text box with GUI.FocusControl.
		///         GUIUtility.keyboardControl = -1;
		///                     
		///         // Use Input.GetKeyDown and others here, remember to set isEditing to false after you're done!
		///         // It's best to check Input.anyKeyDown and set isEditing to false immediately if it's true,
		///         // so that the input doesn't have a chance to propagate to the game itself.
		/// 
		///         if (GUILayout.Button("Stop"))
		///             isEditing = false;
		///     }
		///     else
		///     {
		///         if (GUILayout.Button("Start"))
		///             isEditing = true;
		///     }
		/// 
		///     // This will only be true when isEditing is true and you hold any key
		///     GUILayout.Label("Any key pressed: " + Input.anyKey);
		/// }
		/// </example>
		/// <param name="setting">
		/// Setting currently being set (if available).
		/// </param>
		/// <param name="isCurrentlyAcceptingInput">
		/// Set this ref parameter to true when you want the current setting drawer to receive Input events.
		/// The value will persist after being set, use it to see if the current instance is being edited.
		/// Remember to set it to false after you are done!
		/// </param>
		public delegate void CustomHotkeyDrawerFunc(BepInEx.Configuration.ConfigEntryBase setting, ref bool isCurrentlyAcceptingInput);

		/// <summary>
		/// Show this setting in the settings screen at all? If false, don't show.
		/// </summary>
		public bool? Browsable;

		/// <summary>
		/// Category the setting is under. Null to be directly under the plugin.
		/// </summary>
		public string Category;

		/// <summary>
		/// If set, a "Default" button will be shown next to the setting to allow resetting to default.
		/// </summary>
		public object DefaultValue;

		/// <summary>
		/// Force the "Reset" button to not be displayed, even if a valid DefaultValue is available. 
		/// </summary>
		public bool? HideDefaultButton;

		/// <summary>
		/// Force the setting name to not be displayed. Should only be used with a <see cref="CustomDrawer"/> to get more space.
		/// Can be used together with <see cref="HideDefaultButton"/> to gain even more space.
		/// </summary>
		public bool? HideSettingName;

		/// <summary>
		/// Optional description shown when hovering over the setting.
		/// Not recommended, provide the description when creating the setting instead.
		/// </summary>
		public string Description;

		/// <summary>
		/// Name of the setting.
		/// </summary>
		public string DispName;

		/// <summary>
		/// Order of the setting on the settings list relative to other settings in a category.
		/// 0 by default, higher number is higher on the list.
		/// </summary>
		public int? Order;

		/// <summary>
		/// Only show the value, don't allow editing it.
		/// </summary>
		public bool? ReadOnly;

		/// <summary>
		/// If true, don't show the setting by default. User has to turn on showing advanced settings or search for it.
		/// </summary>
		public bool? IsAdvanced;

		/// <summary>
		/// Custom converter from setting type to string for the built-in editor textboxes.
		/// </summary>
		public System.Func<object, string> ObjToStr;

		/// <summary>
		/// Custom converter from string to setting type for the built-in editor textboxes.
		/// </summary>
		public System.Func<string, object> StrToObj;
	}
}