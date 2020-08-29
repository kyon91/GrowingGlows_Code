using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;
using kyon;

namespace kyon
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public static ReactiveProperty<int> level = new ReactiveProperty<int>(1);
        public static IObservable<int> onLevelChanged { get { return level; } }
        public List<int> levelLineList = new List<int>();

        public static int rainbowPieceNum = 0;
        public static int totalRainbowPieceNum = 0;

        public RectTransform fieldWall;
        public EdgeCollider2D wallColliderTop;
        public EdgeCollider2D wallColliderDown;
        public EdgeCollider2D wallColliderLeft;
        public EdgeCollider2D wallColliderRight;
        public static int fieldWidth;
        public static int fieldHeight;
        public static float colliderX;
        public static float colliderY;
        public static int fieldLevel = 1;

        [SerializeField] private GameObject gameOverCanvas;
        [SerializeField] private SkillTree skillTree;
        [SerializeField] private Button skillButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private Image menuButtonImage;
        [SerializeField] private TextMeshProUGUI menuText;
        [SerializeField] private Light2D menuButtonLight;
        [SerializeField] private ParticleSystem menuButtonParticle;
        [SerializeField] private ParticleSystem skillTreeButtonParticle;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI levelNumText;
        [SerializeField] private TextMeshProUGUI rainbowPieceNumText;
        [SerializeField] private Image rainbowPiece;
        [SerializeField] private Image rainbowPieceWhite;
        [SerializeField] private Light2D rainbowPieceLight;
        [SerializeField] private StartBall StartBall;
        public bool highRightMenuButton = false;
        private bool isGet30 = false;
        private bool isGet150 = false;
        private bool isGet800 = false;
        private bool isGet1500 = false;

        public bool isStarted = false;

        public bool isDebug = false;

        [SerializeField] private AudioClip TitleBGM;

        private void Start()
        {
            fieldWidth = 1304;
            fieldHeight = 734;
            fieldWall.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fieldWidth);
            fieldWall.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fieldHeight);

            Vector2[] colliderPoints = wallColliderTop.points;
            colliderPoints[0] = new Vector2(-640, 355);
            colliderPoints[1] = new Vector2(640, 355);
            wallColliderTop.points = colliderPoints;

            colliderPoints = wallColliderDown.points;
            colliderPoints[0] = new Vector2(-640, -355);
            colliderPoints[1] = new Vector2(640, -355);
            wallColliderDown.points = colliderPoints;

            colliderPoints = wallColliderLeft.points;
            colliderPoints[0] = new Vector2(-640, 355);
            colliderPoints[1] = new Vector2(-640, -355);
            wallColliderLeft.points = colliderPoints;

            colliderPoints = wallColliderRight.points;
            colliderPoints[0] = new Vector2(640, 355);
            colliderPoints[1] = new Vector2(640, -355);
            wallColliderRight.points = colliderPoints;

            fieldLevel = 1;

            BGMManager.Instance.PlayBGM(null, TitleBGM);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X) && Instance.isDebug) SetField(2);
            if (Input.GetKeyDown(KeyCode.C) && Instance.isDebug) SetField(3);
            if (Input.GetKeyDown(KeyCode.V) && Instance.isDebug) SetField(1);
        }

        public void GameOver()
        {
            DarkenMenuButton();
            DOTween.To(() => menuButtonImage.color, num => menuButtonImage.color = num, new Color(1, 1, 1, 0), 1.5f);
            DOTween.To(() => menuText.color, num => menuText.color = num, new Color(1, 1, 1, 0), 1.5f);
            RetryButton.Instance.Enable();
        }

        public void GameStart()
        {
            menuButton.interactable = true;
            DOTween.To(() => menuButtonImage.color, num => menuButtonImage.color = num, new Color(1, 1, 1, 0.5f), 1.5f);
            DOTween.To(() => menuText.color, num => menuText.color = num, new Color(1, 1, 1, 0.5f), 1.5f);
            DOTween.To(() => levelText.color, num => levelText.color = num, new Color(1, 1, 1, 1), 1.5f);
            DOTween.To(() => levelNumText.color, num => levelNumText.color = num, new Color(1, 1, 1, 1), 1.5f);
            DOTween.To(() => rainbowPieceNumText.color, num => rainbowPieceNumText.color = num, new Color(1, 1, 1, 1), 1.5f);
            DOTween.To(() => rainbowPiece.color, num => rainbowPiece.color = num, new Color(1, 1, 1, 1), 1.5f);
            DOTween.To(() => rainbowPieceWhite.color, num => rainbowPieceWhite.color = num, new Color(1, 1, 1, 0.5f), 1.5f);
            DOTween.To(() => rainbowPieceLight.intensity, num => rainbowPieceLight.intensity = num, 1, 1.5f);
        }

        public void Retry()
        {
            isStarted = false;
            fieldLevel = 1;
            SetField(1);
            level.Value = 1;
            InfomationCanvas.Instance.SetLevelText();
            rainbowPieceNum = 0;
            totalRainbowPieceNum = 0;
            InfomationCanvas.Instance.SetRainbowPieceText();
            Player.Instance.Retry();
            PlayerShot.Instance.ResetList();
            skillTree.ResetNode();
            Title.Instance.Retry();
            BallManager.Instance.ResetBallManager();

            foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
            {
                ball.GetComponent<Ball>().DeathImmidiately();
            }
            foreach (GameObject piece in GameObject.FindGameObjectsWithTag("RainbowPiece"))
            {
                piece.SetActive(false);
            }

            menuButton.interactable = false;
            DOTween.To(() => menuButtonImage.color, num => menuButtonImage.color = num, new Color(1, 1, 1, 0), 1.5f);
            DOTween.To(() => menuText.color, num => menuText.color = num, new Color(1, 1, 1, 0), 1.5f);
            DOTween.To(() => levelText.color, num => levelText.color = num, new Color(1, 1, 1, 0), 1.5f);
            DOTween.To(() => levelNumText.color, num => levelNumText.color = num, new Color(1, 1, 1, 0), 1.5f);
            DOTween.To(() => rainbowPieceNumText.color, num => rainbowPieceNumText.color = num, new Color(1, 1, 1, 0), 1.5f);
            DOTween.To(() => rainbowPiece.color, num => rainbowPiece.color = num, new Color(1, 1, 1, 0), 0);
            DOTween.To(() => rainbowPieceWhite.color, num => rainbowPieceWhite.color = num, new Color(1, 1, 1, 0), 1.5f);
            DOTween.To(() => rainbowPieceLight.intensity, num => rainbowPieceLight.intensity = num, 0, 1.5f);

            DarkenMenuButton();
            isGet30 = false;
            isGet150 = false;
            isGet800 = false;
            isGet1500 = false;

            Instantiate(StartBall, Vector3.zero, Quaternion.identity);
        }

        public Vector2 GetTopRightCornerPos()
        {
            Vector2 pos = new Vector2(8, 4);
            switch (fieldLevel)
            {
                case 1:
                    pos = new Vector2(5.5f, 2);
                    break;
                case 2:
                    pos = new Vector2(9.5f, 9.5f);
                    break;
                case 3:
                    pos = new Vector2(11f, 11f);
                    break;
            }

            return pos;
        }

        public static void Pause()
        {
            GameTimer.SetTimeScale(0);
        }
        public static void Continue()
        {
            GameTimer.SetTimeScale(1);
        }

        public void GetRainbowPiece()
        {
            if (Player.isDead) return;

            rainbowPieceNum++;
            totalRainbowPieceNum++;
            InfomationCanvas.Instance.SetRainbowPieceText();
            if (level.Value <= 20)
            {
                if (totalRainbowPieceNum >= levelLineList[level.Value - 1])
                {
                    LevelUp();
                }
            }
            else
            {
                if (totalRainbowPieceNum >= level.Value * 1000 - 17000)
                {
                    LevelUp();
                }
            }

            if (!isGet30 && rainbowPieceNum >= 30)
            {
                HighLightMenuButton();
                isGet30 = true;
            }
            if (!isGet150 && rainbowPieceNum >= 150)
            {
                HighLightMenuButton();
                isGet150 = true;
            }
            if (!isGet800 && rainbowPieceNum >= 800)
            {
                HighLightMenuButton();
                isGet800 = true;
            }
            if (!isGet1500 && rainbowPieceNum >= 1500)
            {
                HighLightMenuButton();
                isGet1500 = true;
            }
        }

        public void LevelUp()
        {
            level.Value++;
            InfomationCanvas.Instance.SetLevelText();
            if (level.Value == 3) HighLightMenuButton();
            if (level.Value >= 5 && fieldLevel == 1)
            {
                fieldLevel++;
                SetField(2);
            }
            if (level.Value >= 10 && fieldLevel == 2)
            {
                fieldLevel++;
                SetField(3);
            }
        }

        public void HighLightMenuButton()
        {
            if (highRightMenuButton) return;
            highRightMenuButton = true;
            menuButtonImage.color = new Color(1, 1, 1, 1);
            menuText.color = new Color(1, 1, 1, 1);
            DOTween.To(() => menuButtonLight.pointLightOuterRadius, num => menuButtonLight.pointLightOuterRadius = num, 8, 1.5f);
            menuButtonParticle.Play();
            skillTreeButtonParticle.Play();
        }
        public void DarkenMenuButton()
        {
            if (!highRightMenuButton) return;
            highRightMenuButton = false;
            menuButtonImage.color = new Color(1, 1, 1, 0.5f);
            menuText.color = new Color(1, 1, 1, 0.5f);
            DOTween.To(() => menuButtonLight.pointLightOuterRadius, num => menuButtonLight.pointLightOuterRadius = num, 0, 1.5f);
            menuButtonParticle.Stop();
            skillTreeButtonParticle.Stop();
        }

        public void SetField(int fieldLevel)
        {
            int targetFieldWidth = fieldWidth;
            int targetFieldHeight = fieldHeight;
            float targetColliderX = colliderX;
            float targetColliderY = colliderY;
            switch (fieldLevel)
            {
                case 1:
                    fieldWidth = 1304;
                    fieldHeight = 734;
                    colliderX = 640;
                    colliderY = 355;
                    targetFieldWidth = 1304;
                    targetFieldHeight = 734;
                    targetColliderX = 640;
                    targetColliderY = 355;
                    CameraMove.Instance.ResetCamera();
                    break;
                case 2:
                    fieldWidth = 1304;
                    fieldHeight = 734;
                    colliderX = 640;
                    colliderY = 355;
                    targetFieldWidth = 2000;
                    targetFieldHeight = 2000;
                    targetColliderX = 988;
                    targetColliderY = 988;
                    CameraMove.Instance.SwitchSeek();
                    break;
                case 3:
                    fieldWidth = 2000;
                    fieldHeight = 2000;
                    colliderX = 988;
                    colliderY = 988;
                    targetFieldWidth = 2500;
                    targetFieldHeight = 2500;
                    targetColliderX = 1238;
                    targetColliderY = 1238;
                    break;
            }
            IDisposable setter = Observable.EveryUpdate().Subscribe(_ =>
            {
                fieldWall.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fieldWidth);
                fieldWall.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fieldHeight);
                Vector2[] colliderPoints = wallColliderTop.points;
                colliderPoints[0] = new Vector2(-colliderX, colliderY);
                colliderPoints[1] = new Vector2(colliderX, colliderY);
                wallColliderTop.points = colliderPoints;

                colliderPoints = wallColliderDown.points;
                colliderPoints[0] = new Vector2(-colliderX, -colliderY);
                colliderPoints[1] = new Vector2(colliderX, -colliderY);
                wallColliderDown.points = colliderPoints;

                colliderPoints = wallColliderLeft.points;
                colliderPoints[0] = new Vector2(-colliderX, colliderY);
                colliderPoints[1] = new Vector2(-colliderX, -colliderY);
                wallColliderLeft.points = colliderPoints;

                colliderPoints = wallColliderRight.points;
                colliderPoints[0] = new Vector2(colliderX, colliderY);
                colliderPoints[1] = new Vector2(colliderX, -colliderY);
                wallColliderRight.points = colliderPoints;
            });

            Sequence sequence = DOTween.Sequence()
                .Append(DOTween.To(() => fieldWidth, num => fieldWidth = num, targetFieldWidth, 2))
                .Join(DOTween.To(() => fieldHeight, num => fieldHeight = num, targetFieldHeight, 2))
                .Join(DOTween.To(() => colliderX, num => colliderX = num, targetColliderX, 2))
                .Join(DOTween.To(() => colliderY, num => colliderY = num, targetColliderY, 2))
                .AppendCallback(() => setter.Dispose());
        }
    }
}