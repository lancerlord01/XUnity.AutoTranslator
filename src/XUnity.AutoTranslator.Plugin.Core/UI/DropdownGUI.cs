﻿using System.Collections.Generic;
using UnityEngine;

namespace XUnity.AutoTranslator.Plugin.Core.UI
{

   internal class DropdownGUI<TDropdownOptionViewModel, TSelection>
      where TDropdownOptionViewModel : DropdownOptionViewModel<TSelection>
      where TSelection : class
   {

      private const float MaxHeight = GUIUtil.RowHeight * 5;

      private GUIContent _noSelection;
      private GUIContent _unselect;
      private DropdownViewModel<TDropdownOptionViewModel, TSelection> _viewModel;

      private float _x;
      private float _y;
      private float _width;
      private bool _isShown;
      private Vector2 _scrollPosition;

      public DropdownGUI( float x, float y, float width, DropdownViewModel<TDropdownOptionViewModel, TSelection> viewModel )
      {
         _x = x;
         _y = y;
         _width = width;
         _noSelection = new GUIContent( "----", "<b>SELECT TRANSLATOR</b>\nNo translator is currently selected, which means no new translations will be performed. Please select one from the dropdown." );
         _unselect = new GUIContent( "----", "<b>UNSELECT TRANSLATOR</b>\nThis will unselect the current translator, which means no new translations will be performed." );

         _viewModel = viewModel;
      }

      public void OnGUI()
      {
         bool clicked = GUI.Button( GUIUtil.R( _x, _y, _width, GUIUtil.RowHeight ), _viewModel.CurrentSelection?.Text ?? _noSelection, _isShown ? GUIUtil.NoMarginButtonPressedStyle : GUI.skin.button );
         if( clicked )
         {
            _isShown = !_isShown;
         }

         if( _isShown )
         {
            ShowDropdown( _x, _y + GUIUtil.RowHeight, _width, GUI.skin.button );
         }

         if( !clicked && Event.current.isMouse )
         {
            _isShown = false;
         }
      }

      private void ShowDropdown( float x, float y, float width, GUIStyle buttonStyle )
      {
         var rect = GUIUtil.R( x, y, width, _viewModel.Options.Count * GUIUtil.RowHeight > MaxHeight ? MaxHeight : _viewModel.Options.Count * GUIUtil.RowHeight );

         GUILayout.BeginArea( rect, GUIUtil.NoSpacingBoxStyle );
         _scrollPosition = GUILayout.BeginScrollView( _scrollPosition );

         var style = _viewModel.CurrentSelection == null ? GUIUtil.NoMarginButtonPressedStyle : GUIUtil.NoMarginButtonStyle;
         if( GUILayout.Button( _unselect, style ) )
         {
            _viewModel.Select( null );
            _isShown = false;
         }

         foreach( var option in _viewModel.Options )
         {
            style = option.IsSelected() ? GUIUtil.NoMarginButtonPressedStyle : GUIUtil.NoMarginButtonStyle;
            GUI.enabled = option?.IsEnabled() ?? true;
            if( GUILayout.Button( option.Text, style ) )
            {
               _viewModel.Select( option );
               _isShown = false;
            }
            GUI.enabled = true;
         }

         GUILayout.EndScrollView();
         GUILayout.EndArea();
      }
   }
}
