using LitJson;
using ShadowWatcher.Socket;
using System.Collections.Generic;
using Wizard;
using Wizard.Replay;

namespace ShadowWatcher.Replay
{
    public class ReplayManager
    {
        private GameMgr gameMgr = GameMgr.GetIns();
        private ReplayController repController;

        public void Loop()
        {
            if (gameMgr.GetReplayControl() != repController)
            {
                repController = gameMgr.GetReplayControl();

                if (repController != null)
                {
                    Sender.Send($"ReplayDetail:{getJson(Data.ReplayBattleInfo).ToString()}");
                }
            }
        }

        private JsonWriter getJson(ReplayDetail detail)
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

            w.WritePropertyName("playlist");
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

            w.WriteObjectEnd();
            return w;
        }
    }
}
