// Copyright 2017 Gizeta
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using ShadowWatcher.Helper;
using Wizard.DeckCardEdit;

namespace ShadowWatcher.Deck
{
    public static class DeckUIHelper
    {
        public enum FILTER_TYPE
        {
            COST = 0,
            CLASS = 6,
            TYPE = 3,
            RARITY = 2,
        }

        public static FilterController GetFilterController(this CardAllListUI ui)
        {
            return ui.GetField("_filter").GetValue(ui) as FilterController;
        }

        public static FilterController GetFilterController(this DeckCardEditUI ui)
        {
            return ui.GetField("_pagingFilter").GetValue(ui) as FilterController;
        }

        private static FilterController.ButtonArray[] GetBtnArray(this FilterController controller)
        {
            return controller.GetField("BtnArray").GetValue(controller) as FilterController.ButtonArray[];
        }

        public static void SwitchFilter(this FilterController controller, FILTER_TYPE type, int index)
        {
            controller.GetMethod("OnClickBtn").Invoke(controller, new object[] { controller.GetBtnArray()[(int)type][index].gameObject });
        }
    }
}
