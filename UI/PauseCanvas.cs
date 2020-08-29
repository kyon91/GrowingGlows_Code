using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using kyon;

namespace kyon
{
	public class PauseCanvas : MonoBehaviour
	{
        [SerializeField] private GameObject PauseMenu;
        [SerializeField] private GameObject ConfigWindow;
        [SerializeField] private AudioClip menuClip;

        private bool openingMenu = false;

        [SerializeField] private SkillTree skillTree;
        [SerializeField] private ParticleSystem skillTreeButtonParticle;
        [SerializeField] private Button skillTreeExitButton;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
            {
                if (!skillTree.gameObject.activeSelf)
                {
                    if (!ConfigWindow.activeSelf)
                    {
                        if (!openingMenu)
                            OpenMenu();
                        else
                            CloseMenu();
                    }
                    else
                    {
                        ConfigWindow.SetActive(false);
                        SEManager.Instance.Play(menuClip);
                    }
                }
                else
                {
                    skillTreeExitButton.onClick.Invoke();
                }
            }
        }

        public void skillTreeParticle()
        {
            skillTreeButtonParticle.Clear();
            if (skillTree.IsExistsAvailableNode())
                skillTreeButtonParticle.Play();
            else
                skillTreeButtonParticle.Stop();
        }

        public void OpenMenu()
        {
            if (Player.isDead) return;
            if (!GameManager.Instance.isStarted) return;
            if (openingMenu) return;
            SEManager.Instance.Play(menuClip);
            openingMenu = true;
            PauseMenu.SetActive(true);
            skillTreeParticle();
            GameManager.Pause();
        }
        public void CloseMenu()
        {
            if (!openingMenu) return;
            openingMenu = false;
            PauseMenu.SetActive(false);
            GameManager.Continue();
            SEManager.Instance.Play(menuClip);
        }
    }
}