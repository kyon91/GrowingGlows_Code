using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using kyon;

namespace kyon
{
	public class RetryButton : SingletonMonoBehaviour<RetryButton>
    {
        private SimpleAnimation anim;
        public bool isActive = false;

        [SerializeField] private GameObject gameOverCanvas;
        [SerializeField] private SimpleAnimation gameOverText;
        [SerializeField] private SimpleAnimation tweetButton;
        [SerializeField] private SimpleAnimation rankingButton;

        [SerializeField] Button button;

        protected override void Awake()
        {
            anim = GetComponent<SimpleAnimation>();
        }

        public void OnPointerEnter(string clipName)
        {
            if (isActive)
                anim.Play(clipName);
        }
        public void OnPointerExit(string clipName)
        {
            if (isActive)
                anim.Play(clipName);
        }

        public void Enable()
        {
            gameOverText.Play("Default");
            anim.Play("Default");
            tweetButton.Play("Default");
            rankingButton.Play("Default");
        }

        public async void Retry()
        {
            if (!isActive) return;
            isActive = false;
            GameManager.Instance.Retry();
            gameOverText.Play("Fade");
            anim.Play("Fade");
            tweetButton.Play("Fade");
            rankingButton.Play("Fade");

            GameManager.Instance.Retry();

            await UniTask.Delay(2000);
        }
    }
}