using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using kyon;

namespace kyon
{
    public class TweetButton : SingletonMonoBehaviour<RetryButton>
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
        
        public void Tweet()
        {
            if (!isActive) return;
            isActive = false;
            var url = "https://twitter.com/intent/tweet?"
            + "text=" + "光のカケラを"+GameManager.totalRainbowPieceNum.ToString()+"個集めた！"
            + "&url=" + "https://unityroom.com/games/growingglow"
            + "&hashtags=" + "unity1week,Growing_Glow";

#if UNITY_EDITOR
            Application.OpenURL(url);
#elif UNITY_WEBGL
            // WebGLの場合は、ゲームプレイ画面と同じウィンドウでツイート画面が開かないよう、処理を変える
            Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
#else
            Application.OpenURL(url);
#endif
        }
    }
}