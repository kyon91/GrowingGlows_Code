using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using DG.Tweening;
using kyon;

namespace kyon
{
	public class InfomationCanvas : SingletonMonoBehaviour<InfomationCanvas>
	{
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI rainbowPieceText;
        [SerializeField] private Image rainbowPiece;

        public int rainbowPieceNum;

        private Tween numTween;

        private void Start()
        {
            SetRainbowPieceText();
            SetLevelText();
            GameManager.onLevelChanged.Subscribe(_ =>
            {
                SetLevelText();
            });
        }

        public void SetLevelText()
        {
            levelText.text = GameManager.level.Value.ToString();
        }
        public void SetRainbowPieceText()
        {
            numTween?.Kill();
            numTween = DOTween.To(() => rainbowPieceNum,
                num => rainbowPieceNum = num,
                GameManager.rainbowPieceNum,
                Mathf.Min(Mathf.Abs(GameManager.rainbowPieceNum - rainbowPieceNum) / 10, 1.5f)
                );
        }

        private void Update()
        {
            rainbowPieceText.text = rainbowPieceNum.ToString();
        }
    }
}