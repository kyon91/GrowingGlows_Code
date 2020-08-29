using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using kyon;

namespace kyon
{
    public class RankingButton : SingletonMonoBehaviour<RetryButton>
    {
        private SimpleAnimation anim;
        public bool isActive = false;

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
        
        public void ShowRanking()
        {
            if (!isActive) return;
            isActive = false;
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(GameManager.totalRainbowPieceNum);
        }
    }
}