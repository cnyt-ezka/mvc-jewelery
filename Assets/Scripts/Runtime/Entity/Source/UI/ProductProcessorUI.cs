using DG.Tweening;
using Runtime.Data.ValueObject.Source;
using TMPro;
using UnityEngine;

namespace Runtime.Entity.Source.UI
{
    public class ProductProcessorUI : MonoBehaviour
    {
        [SerializeField] private bool _showUI;
        [SerializeField] private GameObject _holder;
        [SerializeField] private TextMeshProUGUI _txtCapacity; 
        
        private Tween _scaleUpTween;
        private Tween _fullTween;

        private Vector3 _holderStartScale;
        private Vector3 _txtCapacityStartScale;
        private float _holderStartY;

        private StackVO _vo;

        public void Setup(StackVO vo)
        {
            _holderStartY = _holder.transform.position.y;
            _holderStartScale = _holder.transform.localScale;
            _txtCapacityStartScale = _txtCapacity.transform.localScale;

            _vo = vo;
            
            if (_showUI)
            {
                Show();
                
                UpdateUI();
            }
            else
            {
                Hide();
            }
        }

        public void UpdateUI()
        {
            _txtCapacity.text = $"{_vo.StackableItems.Count}/{_vo.MaxCount}";

            Animate();
        }

        private void Animate()
        {
            if (_vo.IsFull())
            {
                if (_fullTween == null)
                {
                    _fullTween = _holder.transform.DOLocalMoveY(1.1f, .5f)
                        .SetEase(Ease.InQuad)
                        .SetLoops(-1, LoopType.Yoyo)
                        .OnComplete(() =>
                        {
                            _fullTween = null;
                        });
                }
            }
            else
            {
                if (_fullTween != null)
                {
                    _fullTween.Kill();
                    _fullTween = null;
                    _holder.transform.DOLocalMoveY(_holderStartY, .5f);    
                }

                if (_scaleUpTween == null)
                {
                    _holder.transform.localScale = _holderStartScale;
                    _txtCapacity.transform.localScale = _txtCapacityStartScale;
                    
                    _scaleUpTween = _txtCapacity.transform.DOScale(_txtCapacityStartScale * 1.3f, .05f)
                        .SetLoops(2, LoopType.Yoyo)
                        .OnComplete(() =>
                        {
                            _scaleUpTween = null;
                        });
                }
            }
        }

        public void Show()
        {
            _holder.SetActive(true);   
        }

        public void Hide()
        {
            _holder.SetActive(false);
        }
    }
}