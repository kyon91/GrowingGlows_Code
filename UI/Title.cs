using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Cysharp.Threading.Tasks;
using UniRx;
using kyon;

namespace kyon
{
	public class Title : SingletonMonoBehaviour<Title>
	{
        [SerializeField] private Light2D hexLight;
        private SimpleAnimation anim;

        private void Start()
        {
            anim = GetComponent<SimpleAnimation>();
        }

        private void Update()
        {
            hexLight.transform.Rotate(0, 0, 50 * Time.deltaTime);
        }

        public void StartGame()
        {
            anim.Play("Default");
            OnStartGame.OnNext(Unit.Default);
        }

        public Subject<Unit> OnStartGame = new Subject<Unit>();

        public void Retry()
        {
            anim.Play("Retry");
        }
    }
}