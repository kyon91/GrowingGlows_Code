using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;
using kyon;

namespace kyon
{
	public class RainbowPiece : MonoBehaviour, ObjectPool.IPoolableObject, GameTimer.TimerObject
    {
        private bool seek = false;
        TrailRenderer trail;
        private Transform player;

        private Rigidbody2D rb;
        private Vector2 direction;
        private bool isActive = false;

        [SerializeField] private Collider2D bodyCollider;
        private SpriteRenderer sp;
        private Light2D pointLight;

        Sequence spreadSequence;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            trail = GetComponent<TrailRenderer>();
            sp = GetComponent<SpriteRenderer>();
            pointLight = GetComponent<Light2D>();

            bodyCollider.OnTriggerEnter2DAsObservable().Subscribe(collision =>
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    GameManager.Instance.GetRainbowPiece();
                    trail.Clear();
                    gameObject.SetActive(false);
                }
                //if (collision.gameObject.CompareTag("Wall"))
                //{
                //    spreadSequence?.Kill();
                //}
            });

            InitTimerObject();
        }

        public virtual void InitTimerObject()
        {
            GameTimer.OnTimeScaleChanged.Subscribe(_ =>
            {
                if (spreadSequence != null)
                    spreadSequence.timeScale = GameTimer.GetTimeScale();
            }).AddTo(gameObject);
        }

        private async void OnEnable()
        {
            isActive = true;
            trail.Clear();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            seek = false;

            await UniTask.DelayFrame(1);
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        public void Spread(float radius)
        {
            trail.Clear();
            Vector2 targetPos = (Vector2)transform.position + (Random.insideUnitCircle * radius);
            spreadSequence = DOTween.Sequence()
                .Append(transform.DOMove(targetPos, 1))
                .AppendCallback(() => isActive = true);
        }

        public void SetColor(Color color)
        {
            sp.color = color;
            pointLight.color = color;
        }

        void Update()
        {
            if (isActive)
            {
                direction = (player.position - transform.position).normalized;
                rb.velocity = direction * 8 * GameTimer.GetTimeScale();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //if (collision.gameObject.CompareTag("Player"))
            //    seek = true;
        }
    }
}