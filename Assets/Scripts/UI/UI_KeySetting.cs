using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_KeySetting : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _currentPlayer1Keys;
    [SerializeField] private TMP_Text[] _curren2Player1Keys;

    [SerializeField] private Button[] _player1KeyButtons;
    [SerializeField] private Button[] _player2KeyButtons;

    [SerializeField] private KeyManager _keyManager;

    private void Start()
    {
        UpdateKeyTexts();
        SetUpButtons();
    }

    private void OnEnable()
    {
        _keyManager = FindAnyObjectByType<KeyManager>();
    }

    private void Update()
    {
        UpdateKeyTexts();
    }

    private void SetUpButtons()
    {
        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            int index = i;

            _player1KeyButtons[i].onClick.AddListener(() =>
            {
                _keyManager.OnChangeKey(PlayerType.Player1, index);
            });

            _player2KeyButtons[i].onClick.AddListener(() =>
            {
                _keyManager.OnChangeKey(PlayerType.Player2, index);
            });
        }
    }

    private void UpdateKeyTexts()
    {
        for (int i = 0; i < _currentPlayer1Keys.Length; i++)
        {
            _currentPlayer1Keys[i].text = KeySetting.player1Keys[(KeyAction)i].ToString();
            _curren2Player1Keys[i].text = KeySetting.player2Keys[(KeyAction)i].ToString();
        }
    }
}
