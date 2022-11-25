using DG.Tweening;
using MVC.Base.Runtime.Abstract.View;
using TMPro;
using UnityEngine;

namespace Runtime.Views.Screens.TopBar
{
    public class TopBarScreensView : MVCScreenView
    {
        [SerializeField] private TextMeshProUGUI _txtCoin;

        private Tween _scaleUpTween;

        private int tempMoney = 0;
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
                OnCurrencyChanged(tempMoney++);
        }

        public void OnCurrencyChanged(int totalCoinAmount)
        {
            _txtCoin.text = totalCoinAmount.ToString();

            if (_scaleUpTween == null)
            {
                _scaleUpTween = _txtCoin.transform.DOScale(Vector3.one * 1.3f, 0.05f)
                    .SetLoops(2, LoopType.Yoyo)
                    .OnComplete(() =>
                    {
                        _scaleUpTween = null;
                    });
            }
        }
    }
}
