
export default function HighlightString () {
  this.KeyWords = null
  this.CssBegin = "<font style='color:red'>"
  this.CssEnd = '</font>'
  this.Text = null
  this.formatKeyword = function (keyword) {
    var keyword2 = keyword.trim()
    keyword2 = keyword2.replace('|', '\|')
    return keyword2
  }
  this.refreshContent = function (contentID) {
    var content
    if (contentID) {
      content = document.getElementById(contentID).innerHTML
    } else {
      content = this.Text
    }
    var regKeywords = '('
    for (var i = 0; i < this.KeyWords.length; i++) {
      if (i > 0) regKeywords += '|'
      regKeywords += this.formatKeyword(this.KeyWords[i])
    }
    regKeywords += ')'
    var regex = new RegExp(regKeywords, 'gi')
    var newContent = content.replace(regex, this.CssBegin + '$1' + this.CssEnd)
    if (contentID) {
      document.getElementById(contentID).innerHTML = newContent
      return null
    }
    return newContent
  }
};
