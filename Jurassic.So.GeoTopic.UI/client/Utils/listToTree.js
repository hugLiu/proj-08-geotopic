/**
 * Created by Administrator on 2017/4/7.
 */
/* Demo rows
 var rows = [{
 "id": 102,
 "parentId": 100,
 "name": "有父节点2"
 },{
 "id": 101,
 "parentId": 100,
 "name": "有父节点1"
 },{
 "id": 100,
 "parentId": null,
 "name": "无父节点"
 },{
 "id": 1,
 "parentId": 0,
 "name": "Foods"
 }, {
 "id": 2,
 "parentId": 1,
 "name": "Fruits"
 }, {
 "id": 3,
 "parentId": 1,
 "name": "Vegetables"
 }];
 */

function listToTree(rows) {
  function exists(rows, parentId) {
    for (var i = 0; i < rows.length; i++) {
      if (rows[i].id == parentId) return true;
    }
    return false;
  }

  var nodes = [];
  // get the top level nodes
  for (var i = 0; i < rows.length; i++) {
    var row = rows[i];
    if (!exists(rows, row.parentId)) {
      nodes.push({
        id: row.id,
        text: row.name
      });
    }
  }

  var toDo = [];
  for (var i = 0; i < nodes.length; i++) {
    toDo.push(nodes[i]);
  }
  while (toDo.length) {
    var node = toDo.shift(); // the parent node
    // get the children nodes
    for (var i = 0; i < rows.length; i++) {
      var row = rows[i];
      if (row.parentId == node.id) {
        var child = {
          id: row.id,
          text: row.name
        };
        if (node.children) {
          node.children.push(child);
        } else {
          node.children = [child];
        }
        toDo.push(child);
      }
    }
  }
  return nodes;
}

export default listToTree