﻿using Loderunner.Service;
using UnityEngine;

namespace Loderunner.Gameplay
{
    public class LadderView : View<LadderPresenter>
    {
        [SerializeField] private Transform _bottom;
        [SerializeField] private Transform _top;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var characterView = collider.gameObject.GetComponent<ICharacterView>();
            
            if (characterView != null)
            {
                _presenter.CharacterEnterLadder(characterView, _bottom.position, _top.position);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            var characterView = collider.gameObject.GetComponent<ICharacterView>();
            
            if (characterView != null)
            {
                _presenter.PlayerExitLadder(characterView);
            }
        }
    }
}