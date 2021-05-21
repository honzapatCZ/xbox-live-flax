// Copyright (C) 2021 Nejcraft Do Not Redistribute

using System;
using System.Collections.Generic;
using FlaxEditor;
using FlaxEditor.GUI.ContextMenu;
using FlaxEngine;

namespace XboxLiveFlax
{
    /// <summary>
    /// XboxLiveEditorPlugin Script.
    /// </summary>
    public class XboxLiveEditorPlugin : EditorPlugin
    {
        private ContextMenuButton prefabReplacerBtn;

        /// <inheritdoc />
        public override void InitializeEditor()
        {
            base.InitializeEditor();

            prefabReplacerBtn = Editor.UI.MenuWindow.ContextMenu.AddButton("Xbox Live Helper", () => new XboxLiveHelperWindow().Show());
        }
        /// <inheritdoc />
        public override void Deinitialize()
        {
            prefabReplacerBtn?.Dispose();
            prefabReplacerBtn = null;

            base.Deinitialize();
        }
    }
}
