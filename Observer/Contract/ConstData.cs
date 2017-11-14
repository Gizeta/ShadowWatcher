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

using System.Collections.Generic;

namespace ShadowWatcher.Contract
{
    public static class ConstData
    {
        public static List<string> Rank = new List<string>
        {
            "",
            "Beginner 0", "Beginner 1","Beginner 2", "Beginner3",
            "D0", "D1", "D2", "D3",
            "C0", "C1", "C2", "C3",
            "B0", "B1", "B2", "B3",
            "A0", "A1", "A2", "A3",
            "AA0", "AA1", "AA2", "AA3",
            "Master", "Grand Master"
        };

        public static List<string> Class = new List<string>
        {
            "",
            "精灵",
            "皇室护卫",
            "巫师",
            "龙",
            "死灵术士",
            "血族",
            "主教"
        };
    }
}
