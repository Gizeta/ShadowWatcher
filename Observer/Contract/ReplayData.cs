using LitJson;
using System;
using System.Collections.Generic;

namespace ShadowWatcher.Contract
{
    public class ReplayData
    {
        private ReplayData() { }

        public JsonData Data { get; private set; }

        public static ReplayData Parse(string data) => new ReplayData
        {
            Data = JsonMapper.ToObject(data)
        };

        public string Name => Data["name"].ToString();
        public string CountryCode => Data["country_code"].ToString();
        public string Rank => ConstData.Rank[Data["rank"].ToInt()];
        public string Class => ConstData.Class[Data["chara_id"].ToInt() % 10];

        public string OppoName => Data["opponent_name"].ToString();
        public string OppoCountryCode => Data["opponent_country_code"].ToString();
        public string OppoRank => ConstData.Rank[Data["opponent_rank"].ToInt()];
        public string OppoClass => ConstData.Class[Data["opponent_chara_id"].ToInt()];

        public string Result => Data["is_win"].ToBoolean() ? "Win" : "Lose";
        public string Type => Data["is_two_pick"].ToBoolean() ? "2 pick" : "构筑";
        public DateTime Time => new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Data["play_list"]["playlist"][0]["time"].ToLong()).ToLocalTime();

        public static ReplayData Parse(ReplayDetail detail)
        {
            var w = new JsonWriter();
            w.WriteObjectStart();
            w.WritePropertyName("battle_id");
            w.Write(detail.battle_id);
            w.WritePropertyName("seed");
            w.Write(detail.seed);
            w.WritePropertyName("field_id");
            w.Write(detail.field_id);
            w.WritePropertyName("first_turn");
            w.Write(detail.first_turn);

            w.WritePropertyName("play_list");
            w.WriteObjectStart();
            foreach (var key in detail.play_list.Keys)
            {
                if (detail.play_list[key].GetType() == typeof(string))
                {
                    w.WritePropertyName(key);
                    w.Write(detail.play_list[key] as string);
                }
                else
                {
                    w.WritePropertyName(key);
                    w.WriteArrayStart();
                    var list = detail.play_list[key] as List<object>;
                    foreach (Dictionary<string, object> item in list)
                    {
                        w.WriteObjectStart();
                        foreach (var k in item.Keys)
                        {
                            if (item[k].GetType() == typeof(JsonData))
                            {
                                w.WritePropertyName(k);
                                var d = item[k] as JsonData;
                                switch (d.GetJsonType())
                                {
                                    case JsonType.Int:
                                        w.Write(d.ToInt());
                                        break;
                                    case JsonType.Long:
                                        w.Write(d.ToLong());
                                        break;
                                    case JsonType.Double:
                                        w.Write(d.ToDouble());
                                        break;
                                    case JsonType.Boolean:
                                        w.Write(d.ToBoolean());
                                        break;
                                    default:
                                        w.Write(d.ToString());
                                        break;
                                }
                            }
                            else
                            {
                                w.WritePropertyName(k);
                                w.WriteArrayStart();
                                var l = item[k] as List<object>;
                                foreach (Dictionary<string, object> i in l)
                                {
                                    w.WriteObjectStart();

                                    foreach (var k2 in i.Keys)
                                    {
                                        w.WritePropertyName(k2);
                                        var d = i[k2] as JsonData;
                                        switch (d.GetJsonType())
                                        {
                                            case JsonType.Int:
                                                w.Write(d.ToInt());
                                                break;
                                            case JsonType.Long:
                                                w.Write(d.ToLong());
                                                break;
                                            case JsonType.Double:
                                                w.Write(d.ToDouble());
                                                break;
                                            case JsonType.Boolean:
                                                w.Write(d.ToBoolean());
                                                break;
                                            default:
                                                w.Write(d.ToString());
                                                break;
                                        }
                                    }
                                    w.WriteObjectEnd();
                                }
                                w.WriteArrayEnd();
                            }
                        }
                        w.WriteObjectEnd();
                    }
                    w.WriteArrayEnd();
                }
            }
            w.WriteObjectEnd();

            w.WritePropertyName("viewer_id");
            w.Write(detail.viewer_id);
            w.WritePropertyName("name");
            w.Write(detail.name);
            w.WritePropertyName("chara_id");
            w.Write(detail.chara_id);
            w.WritePropertyName("emblem_id");
            w.Write(detail.emblem_id);
            w.WritePropertyName("degree_id");
            w.Write(detail.degree_id);
            w.WritePropertyName("country_code");
            w.Write(detail.country_code);
            w.WritePropertyName("sleeve_id");
            w.Write(detail.sleeve_id);
            w.WritePropertyName("battle_point");
            w.Write(detail.battle_point);
            w.WritePropertyName("master_point");
            w.Write(detail.master_point);
            w.WritePropertyName("rank");
            w.Write(detail.rank);

            w.WritePropertyName("deck");
            w.WriteArrayStart();
            foreach (Dictionary<string, object> item in detail.deck)
            {
                w.WriteObjectStart();
                w.WritePropertyName("idx");
                w.Write(item["idx"] as string);
                w.WritePropertyName("card_id");
                w.Write(item["card_id"] as string);
                w.WriteObjectEnd();
            }
            w.WriteArrayEnd();

            w.WritePropertyName("opponent_viewer_id");
            w.Write(detail.opponent_viewer_id);
            w.WritePropertyName("opponent_name");
            w.Write(detail.opponent_name);
            w.WritePropertyName("opponent_chara_id");
            w.Write(detail.opponent_chara_id);
            w.WritePropertyName("opponent_emblem_id");
            w.Write(detail.opponent_emblem_id);
            w.WritePropertyName("opponent_degree_id");
            w.Write(detail.opponent_degree_id);
            w.WritePropertyName("opponent_country_code");
            w.Write(detail.opponent_country_code);
            w.WritePropertyName("opponent_sleeve_id");
            w.Write(detail.opponent_sleeve_id);
            w.WritePropertyName("opponent_battle_point");
            w.Write(detail.opponent_battle_point);
            w.WritePropertyName("opponent_master_point");
            w.Write(detail.opponent_master_point);
            w.WritePropertyName("opponent_rank");
            w.Write(detail.opponent_rank);

            w.WritePropertyName("opponent_deck");
            w.WriteArrayStart();
            foreach (Dictionary<string, object> item in detail.opponent_deck)
            {
                w.WriteObjectStart();
                w.WritePropertyName("idx");
                w.Write(item["idx"] as string);
                w.WritePropertyName("card_id");
                w.Write(item["card_id"] as string);
                w.WriteObjectEnd();
            }
            w.WriteArrayEnd();

            w.WritePropertyName("is_two_pick");
            w.Write(detail.is_two_pick);
            w.WritePropertyName("is_win");
            w.Write(detail.is_win);

            w.WriteObjectEnd();

            return new ReplayData
            {
                Data = JsonMapper.ToObject(w.ToString())
            };
        }

        public override string ToString() => Data.ToJson();

        public ReplayDetail AssignTo(ReplayDetail detail)
        {
            detail.battle_id = Data["battle_id"].ToLong();
            detail.seed = Data["seed"].ToInt();
            detail.field_id = Data["field_id"].ToInt();
            detail.first_turn = Data["first_turn"].ToInt();

            var playlist = new Dictionary<string, object>();
            var d1 = Data["play_list"];
            foreach (var k1 in d1.Keys)
            {
                if (d1[k1].IsString)
                {
                    playlist.Add(k1, d1[k1].ToString());
                }
                else if (d1[k1].IsArray)
                {
                    var list = new List<object>();
                    var d2 = d1[k1];
                    for (var i = 0; i < d2.Count; i++)
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (var k2 in d2[i].Keys)
                        {
                            if (d2[i][k2].IsArray)
                            {
                                var list2 = new List<object>();
                                var d3 = d2[i][k2];
                                for (var j = 0; j < d3.Count; j++)
                                {
                                    var dict2 = new Dictionary<string, object>();
                                    foreach (var k3 in d3[j].Keys)
                                    {
                                        dict2.Add(k3, d3[j][k3]);
                                    }
                                    list2.Add(dict2);
                                }
                                dict.Add(k2, list2);
                            }
                            else
                            {
                                dict.Add(k2, d2[i][k2]);
                            }
                        }
                        list.Add(dict);
                    }
                    playlist.Add(k1, list);
                }
            }
            detail.play_list = playlist;

            detail.viewer_id = Data["viewer_id"].ToInt();
            detail.name = Data["name"].ToString();
            detail.chara_id = Data["chara_id"].ToInt();
            detail.emblem_id = Data["emblem_id"].ToInt();
            detail.degree_id = Data["degree_id"].ToInt();
            detail.country_code = Data["country_code"].ToString();
            detail.sleeve_id = Data["sleeve_id"].ToInt();
            detail.battle_point = Data["battle_point"].ToInt();
            detail.master_point = Data["master_point"].ToInt();
            detail.rank = Data["rank"].ToInt();

            var deck = new List<object>();
            for (var i = 0; i < Data["deck"].Count; i++)
            {
                var item = new Dictionary<string, object>();
                item.Add("idx", Data["deck"][i]["idx"].ToString());
                item.Add("card_id", Data["deck"][i]["card_id"].ToString());
                deck.Add(item);
            }
            detail.deck = deck;

            detail.opponent_viewer_id = Data["opponent_viewer_id"].ToInt();
            detail.opponent_name = Data["opponent_name"].ToString();
            detail.opponent_chara_id = Data["opponent_chara_id"].ToInt();
            detail.opponent_emblem_id = Data["opponent_emblem_id"].ToInt();
            detail.opponent_degree_id = Data["opponent_degree_id"].ToInt();
            detail.opponent_country_code = Data["opponent_country_code"].ToString();
            detail.opponent_sleeve_id = Data["opponent_sleeve_id"].ToInt();
            detail.opponent_battle_point = Data["opponent_battle_point"].ToInt();
            detail.opponent_master_point = Data["opponent_master_point"].ToInt();
            detail.opponent_rank = Data["opponent_rank"].ToInt();

            var deck2 = new List<object>();
            for (var i = 0; i < Data["opponent_deck"].Count; i++)
            {
                var item = new Dictionary<string, object>();
                item.Add("idx", Data["opponent_deck"][i]["idx"].ToString());
                item.Add("card_id", Data["opponent_deck"][i]["card_id"].ToString());
                deck2.Add(item);
            }
            detail.opponent_deck = deck2;

            return detail;
        }
    }
}
