using TMPro;
using UnityEngine;

namespace GameJamKit.UI
{
    public class GameVersionUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI? versionText;

        private void Awake()
        {
            if (versionText == null) Debug.LogError($"{nameof(GameVersionUI)}: {nameof(versionText)} not found");
        }

        private void Start()
        {
            if (versionText == null) return;
            versionText.text = "v" + Application.version;
        }
    }
}
