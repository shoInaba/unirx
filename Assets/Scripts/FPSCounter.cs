using UnityEngine;
using System.Linq;
using UniRx;
using UniRx.Triggers;

/// <summary> FPS取得 </summary>
public static class FPSCounter {

	const int bufferSize = 5; // バッファサイズ
	public static IReadOnlyReactiveProperty<float> Current { get; private set; }

	static FPSCounter(){ 
		// Currentにイベント登録
		Current = Observable.EveryUpdate()
			.Select(_ => Time.deltaTime)     // Time.deltaTimeに変換
			.Buffer(bufferSize, 1)           // 過去buffaersize分バッファ
			.Select(x => 1.0f / x.Average()) // 平均値から算出
			.ToReadOnlyReactiveProperty();
	}
}