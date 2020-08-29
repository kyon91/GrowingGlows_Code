using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using kyon;

namespace kyon
{
	public class GameTimer
    {
        private static ReactiveProperty<float> gameTimeScale = new ReactiveProperty<float>(1);
        public static IObservable<float> OnTimeScaleChanged { get { return gameTimeScale; } }
        public static bool isPausing = false;

        public static void SetTimeScale(float value)
        {
            gameTimeScale.Value = value;
            if (value == 0)
                isPausing = true;
            else
                isPausing = false;
        }
        public static float GetTimeScale()
        {
            return gameTimeScale.Value;
        }

        public static async UniTask WaitForSeconds(float waitTime)
        {
            float timer = 0;
            bool allow = false;
            IDisposable timerstream = Observable.EveryFixedUpdate().Subscribe(_ =>
            {
                timer += Time.fixedDeltaTime * GetTimeScale();
                if (timer >= waitTime) allow = true;
            });
            await UniTask.WaitUntil(() => allow);
            timerstream.Dispose();
        }

        public interface TimerObject
        {
            void InitTimerObject();
        }
	}
}