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

namespace ShadowWatcher
{
    public static class Settings
    {
        public const int RECORD_ENEMY_CARD = 0x1;
        public const int RECORD_PLAYER_CARD = 0x2;
        public const int ENHANCE_REPLAY = 0x4;
        public const int SHOW_SUMMON_CARD = 0x8;
        public const int COPY_ANIMATED_CARD_FIRST = 0x10;
        public const int SHOW_COUNTDOWN = 0x20;
        public const int KEYBOARD_FILTER_SHORTCUT = 0x40;

        public static bool RecordEnemyCard { get; set; } = true;
        public static bool RecordPlayerCard { get; set; } = true;
        public static bool EnhanceReplay { get; set; } = true;
        public static bool ShowSummonCard { get; set; } = true;
        public static bool CopyAnimatedCardFirst { get; set; } = true;
        public static bool ShowCountdown { get; set; } = true;
        public static bool KeyboardFilterShortcut { get; set; } = true;

        public static void Parse(string data)
        {
            int flag = int.Parse(data);
            RecordEnemyCard = (flag & RECORD_ENEMY_CARD) > 0;
            RecordPlayerCard = (flag & RECORD_PLAYER_CARD) > 0;
            EnhanceReplay = (flag & ENHANCE_REPLAY) > 0;
            ShowSummonCard = (flag & SHOW_SUMMON_CARD) > 0;
            CopyAnimatedCardFirst = (flag & COPY_ANIMATED_CARD_FIRST) > 0;
            ShowCountdown = (flag & SHOW_COUNTDOWN) > 0;
            KeyboardFilterShortcut = (flag & KEYBOARD_FILTER_SHORTCUT) > 0;
        }

        public static new string ToString()
        {
            int flag = 0;
            flag |= RecordEnemyCard ? RECORD_ENEMY_CARD : 0;
            flag |= RecordPlayerCard ? RECORD_PLAYER_CARD : 0;
            flag |= EnhanceReplay ? ENHANCE_REPLAY : 0;
            flag |= ShowSummonCard ? SHOW_SUMMON_CARD : 0;
            flag |= CopyAnimatedCardFirst ? COPY_ANIMATED_CARD_FIRST : 0;
            flag |= ShowCountdown ? SHOW_COUNTDOWN : 0;
            flag |= KeyboardFilterShortcut ? KEYBOARD_FILTER_SHORTCUT : 0;
            return flag.ToString();
        }
    }
}
