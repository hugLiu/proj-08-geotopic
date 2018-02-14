using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Jurassic.So.GeoTopic.Database.Models;
using Jurassic.So.GeoTopic.DataService.Models;

namespace Jurassic.So.GeoTopic.DataService.Tool
{
    public class JurassicConvert
    {
        public static CardDefModel ToCardDef(CardViewModel cardView)
        {
            IList<Row> Rows = new List<Row>();
            int rowNum = cardView.component.Select(s => s.rowNum).Max();
            int colNum = cardView.component.Select(s => s.columnNum).Max();

            CardDefModel cardDef = new CardDefModel
            {
                cells = new List<CellModel>(),
                layout = new RootModel
                {
                    Rows = Rows
                }
            };

            for (int i = 0; i <= rowNum; i++)
            {
                Row row = new Row();
                row.Cols = new List<Col>();
                for (int j = 0; j <= colNum; j++)
                {
                    Col col = new Col();
                    col.Rows = new List<Row>();
                    var components =
                        cardView.component
                            .Where(s => s.rowNum == i && s.columnNum == j);
                    if(components.Count() < 1)
                        continue;
                    int index = components.Select(s => s.index).Max();
                    for (int k = 0; k <= index; k++)
                    {
                        Row minRow = new Row();
                        minRow.Cols = new List<Col>();
                        Col minCol = new Col();
                        minCol.Rows = new List<Row>();
                        Component component = cardView.component
                            .FirstOrDefault(s => s.rowNum == i && s.columnNum == j && s.index == k);
                        if (component != null)
                        {
                            minCol.CellId = component.param.id;
                            CellModel cellModel = new CellModel
                            {
                                id = component.param.id,
                                type = component.param.type,
                                url = component.param.url,
                                title = component.param.title,
                                param = component.param.param
                            };
                            cardDef.cells.Add(cellModel);
                        }
                        minRow.Cols.Add(minCol);
                        col.Rows.Add(minRow);
                    }
                    row.Cols.Add(col);
                }
                Rows.Add(row);
            }

            return cardDef;
        }

        public static CardViewModel ToCardView(CardDefModel cardDef)
        {
            List<Component> components = new List<Component>();
            List<string> ways = new List<string>();
            var rows = cardDef.layout.Rows;
            int rowNum = rows.Count;
            for (int i = 0; i < rowNum; i++)
            {
                int colNum = rows[i].Cols.Count;
                for (int j = 0; j < colNum; j++)
                {
                    int minRowNum = rows[i].Cols[j].Rows.Count;
                    if (minRowNum > 0)
                    {
                        for (int k = 0; k < minRowNum; k++)
                        {
                            string cellId = rows[i].Cols[j].Rows[k].Cols[0].CellId;
                            CellModel cellModel = cardDef.cells.FirstOrDefault(c => c.id == cellId);
                            Component component = new Component
                            {
                                index = k,
                                rowNum = i,
                                columnNum = j,
                                param = new Node
                                {
                                    id = cellModel.id,
                                    type = cellModel.type,
                                    url = cellModel.url,
                                    title = cellModel.title,
                                    param = cellModel.param
                                }                               
                            };
                            components.Add(component);
                        }
                    }
                    else
                    {
                        string cellId = rows[i].Cols[j].CellId;
                        CellModel cellModel = cardDef.cells.FirstOrDefault(c => c.id == cellId);
                        Component component = new Component
                        {
                            index = 0,
                            rowNum = i,
                            columnNum = j,
                            param = new Node
                            {
                                id = cellModel.id,
                                type = cellModel.type,
                                url = cellModel.url,
                                title = cellModel.title,
                                param = cellModel.param
                            }
                        };
                        components.Add(component);
                    }
                }
                ways.Add(colNum.ToString());
            }
            CardViewModel cardView = new CardViewModel
            {
                name = "主题1",
                ways = ways,
                component = components
            };
            return cardView;
        }

        //public static Params ToParams(List<Node> pNodes)
        //{
        //    return null;
        //}

        //public static List<Node> ToListNodes(Params pParams)
        //{
        //    return null;
        //}
        //public static Params ToParams(List<Node> pNodes)
        //{
        //    if (pNodes == null)
        //        return null;
        //    Params param = new Params();
        //    if (pNodes.Find(p => p.Key == "method") != null)
        //        param.method = pNodes.Find(p => p.Key == "method").Value;
        //    if (pNodes.Find(p => p.Key == "username") != null)
        //        param.username = pNodes.Find(p => p.Key == "username").Value;
        //    if (pNodes.Find(p => p.Key == "filter") != null)
        //    {
        //        param.filter = new Dictionary<string, IList<Dictionary<string, string>>>();
        //        int filterId = pNodes.Find(p => p.Key == "filter").Id;
        //        foreach (Node node in pNodes.FindAll(p=>p.Pid==filterId))
        //        {
        //            List < Dictionary < string, string>> listDic = new List<Dictionary<string, string>>();
        //            foreach (Node childnode in pNodes.FindAll(p=>p.Pid==node.Id))
        //            {
        //                listDic.Add(new Dictionary<string, string> { { childnode.Key, childnode.Value } });
        //            }
        //            param.filter.Add(node.Key, listDic);
        //        }
        //    }

        //    if (pNodes.Find(p => p.Key == "table") != null)
        //    {
        //        param.table = new Dictionary<string, string>();
        //        int tableId = pNodes.Find(p => p.Key == "table").Id;
        //        foreach (Node node in pNodes.FindAll(p => p.Pid == tableId))
        //        {
        //            param.table.Add(node.Key, node.Value);
        //        }
        //    }

        //    if (pNodes.Find(p => p.Key == "tags") != null)
        //    {
        //        param.tags = new Dictionary<string, string>();
        //        int tagsId = pNodes.Find(p => p.Key == "tags").Id;
        //        foreach (Node node in pNodes.FindAll(p => p.Pid == tagsId))
        //        {
        //            param.tags.Add(node.Key, node.Value);
        //        }
        //    }
        //    return param;
        //}

        //public static List<Node> ToListNodes(Params pParams)
        //{
        //    if (pParams == null)
        //        return null;
        //    int id = 0;
        //    var listNodes = new List<Node>();
        //    listNodes.Add(new Node {Id = ++id, Key = "params", Value = "", Pid = -1});

        //    if (pParams.method != null)
        //    {
        //        listNodes.Add(new Node {Id = ++id, Key = "method", Value = pParams.method, Pid = 1});
        //    }

        //    if (pParams.username != null)
        //    {
        //        listNodes.Add(new Node {Id = ++id, Key = "username", Value = pParams.username, Pid = 1});
        //    }

        //    if (pParams.filter != null && pParams.filter.Count > 0)
        //    {
        //        listNodes.Add(new Node {Id = ++id, Key = "filter", Value = "", Pid = 1});

        //        int filterId = id;
        //        foreach (KeyValuePair<string, IList<Dictionary<string, string>>> keyValuePair in pParams.filter)
        //        {
        //            listNodes.Add(new Node {Id = ++id, Key = keyValuePair.Key, Value = "", Pid = filterId});
        //            int logicId = id;
        //            foreach (Dictionary<string, string> dictionary in keyValuePair.Value)
        //            {
        //                listNodes.AddRange(
        //                    dictionary.Select(
        //                        dic => new Node {Id = ++id, Key = dic.Key, Value = dic.Value, Pid = logicId}));
        //            }
        //        }

        //    }

        //    if (pParams.table != null && pParams.table.Count > 0)
        //    {
        //        listNodes.Add(new Node {Id = ++id, Key = "table", Value = "", Pid = 1});

        //        int tableId = id;
        //        listNodes.AddRange(
        //            pParams.table.Select(
        //                dic => new Node {Id = ++id, Key = dic.Key, Value = dic.Value, Pid = tableId}
        //                ));

        //    }

        //    if (pParams.tags != null && pParams.tags.Count > 0)
        //    {
        //        listNodes.Add(new Node {Id = ++id, Key = "tags", Value = "", Pid = 1});

        //        int tagsId = id;
        //        listNodes.AddRange(
        //            pParams.tags.Select(
        //                dic => new Node {Id = ++id, Key = dic.Key, Value = dic.Value, Pid = tagsId}
        //                ));
        //    }

        //    return listNodes;
        //}
    }
}