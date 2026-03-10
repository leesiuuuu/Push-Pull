using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum KeyAction
{
    LEFT = 0,
    RIGHT,
    Jump,
    PUSH,
    PULL,
    KEYCOUNT
}

public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> player1Keys = new Dictionary<KeyAction, KeyCode>();
    public static Dictionary<KeyAction, KeyCode> player2Keys = new Dictionary<KeyAction, KeyCode>();

    public static bool IsGamePad { get; set; } = false;
}

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }

    [field: SerializeField]
    public GameObject CheckSelectPanel { get; private set; }

    [SerializeField] private UI_CheckSelectKey _checkSelectKeyUI;

    [SerializeField]
    private KeyCode[] _player1DefaultKeys = new KeyCode[]
    {
        KeyCode.A,
        KeyCode.D,
        KeyCode.W,
        KeyCode.E,
        KeyCode.R
    };

    [SerializeField]
    private KeyCode[] _player2DefaultKeys = new KeyCode[]
    {
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.UpArrow,
        KeyCode.Period,
        KeyCode.Slash
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (KeySetting.player1Keys.Count == 0 && KeySetting.player2Keys.Count == 0)
        {
            for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
            {
                KeySetting.player1Keys[(KeyAction)i] = _player1DefaultKeys[i];
                KeySetting.player2Keys[(KeyAction)i] = _player2DefaultKeys[i];
            }
        }

        if (_checkSelectKeyUI == null)
            _checkSelectKeyUI = FindAnyObjectByType<UI_CheckSelectKey>();
        if (CheckSelectPanel == null)
            CheckSelectPanel = GameObject.Find("NewKeySeleteCanvas");

        if (PlayerPrefs.HasKey("ControlMode"))
            KeySetting.IsGamePad = PlayerPrefs.GetInt("ControlMode") == 1 ? true : false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_checkSelectKeyUI == null)
            _checkSelectKeyUI = FindAnyObjectByType<UI_CheckSelectKey>();
        if (CheckSelectPanel == null)
            CheckSelectPanel = GameObject.Find("NewKeySeleteCanvas");

        CheckSelectPanel?.SetActive(false);
    }

    private void Start()
    {
        CheckSelectPanel?.SetActive(false);
    }

    public void OnChangeKey(PlayerType playerType, int index)
    {
        CheckSelectPanel.SetActive(true);
        _checkSelectKeyUI.SetPlayerType(playerType);
        _checkSelectKeyUI.SetKeyIndex(index);
    }

    public void OnPlayer1ChangeKey(KeyCode newKey, int index)
    {
        KeySetting.player1Keys[(KeyAction)index] = newKey;
    }

    public void OnPlayer2ChangeKey(KeyCode newKey, int index)
    {
        KeySetting.player2Keys[(KeyAction)index] = newKey;
    }

    public KeyCode GetPlayer1Key(KeyAction keyAction)
    {
        if(KeySetting.player1Keys.Count > 0)
        {
            foreach(var key in KeySetting.player1Keys)
            {
                if (key.Key == keyAction) return key.Value;
            }
        }
        else
        {
            return _player1DefaultKeys[(int)keyAction];
        }
        return KeyCode.None;
    }

	public KeyCode GetPlayer2Key(KeyAction keyAction)
	{
		if (KeySetting.player2Keys.Count > 0)
		{
			foreach (var key in KeySetting.player2Keys)
			{
				if (key.Key == keyAction) return key.Value;
			}
		}
		else
		{
			return _player2DefaultKeys[(int)keyAction];
		}
		return KeyCode.None;
	}
}
