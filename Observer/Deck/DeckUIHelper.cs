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
