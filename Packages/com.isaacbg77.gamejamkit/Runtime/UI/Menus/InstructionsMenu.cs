using Eflatun.SceneReference;
using GameJamKit.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameJamKit.UI.Menus
{
    public class InstructionsMenu : MonoBehaviour
    {
        [SerializeField] private SceneReference? mainMenuScene;

        [SerializeField] private GameObject[]? instructionPages;
        [SerializeField] private GameObject? prevButton;
        [SerializeField] private GameObject? nextButton;

        private int currInstructionIndex;

        private void Awake()
        {
            if (mainMenuScene == null) Debug.LogError($"{nameof(InstructionsMenu)}: {nameof(mainMenuScene)} is null");
        }

        private void Start()
        {
            currInstructionIndex = 0;
            instructionPages?[currInstructionIndex].SetActive(true);

            prevButton?.SetActive(false);
            nextButton?.SetActive(true);
        }

        public void NextPage()
        {
            if (instructionPages == null) return;

            instructionPages[currInstructionIndex].SetActive(false);
            currInstructionIndex++;
            instructionPages[currInstructionIndex].SetActive(true);

            if (currInstructionIndex == instructionPages.Length - 1)
            {
                nextButton?.SetActive(false);
                EventSystem.current.SetSelectedGameObject(prevButton);
            }

            if (prevButton is not { activeSelf: true })
                prevButton?.SetActive(true);
        }

        public void PrevPage()
        {
            if (instructionPages == null) return;

            instructionPages[currInstructionIndex].SetActive(false);
            currInstructionIndex--;
            instructionPages[currInstructionIndex].SetActive(true);

            if (currInstructionIndex == 0)
            {
                prevButton?.SetActive(false);
                EventSystem.current.SetSelectedGameObject(nextButton);
            }

            if (nextButton is not { activeSelf: true })
                nextButton?.SetActive(true);
        }

        public void Close()
        {
            if (mainMenuScene == null) return;
            _ = SceneLoader.Instance.LoadSceneAsync(mainMenuScene);
        }
    }
}
